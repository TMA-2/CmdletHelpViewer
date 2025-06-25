using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using System.IO;
using System.Collections.ObjectModel;
using CmdletHelpViewer.Models;
using System.ComponentModel;
using System.Windows.Data;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CmdletHelpViewer.Service;
using System.Configuration;
using CmdletHelpViewer.AttachedCommandBehavior;


namespace CmdletHelpViewer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string filePath;
        private string fileName;
        private Collection<CmdletCommand> cmdletCommands;
        private readonly IDialogService dialogService;
        private Dictionary<string, XElement> cmdletNamesAndXml;

        public string FilePath
        {
            get { return filePath; }
            private set
            {
                filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        public string FileName
        {
            get { return fileName; }
            private set
            {
                fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        public Collection<CmdletCommand> CmdletCommands
        {
            get { return cmdletCommands; }
            private set
            {
                cmdletCommands = value;
                OnPropertyChanged("CmdletCommands");
            }
        }

        public ObservableCollection<AvalonDock.DocumentContent> DocumentContents
        {
            get;
            private set;
        }

        public Dictionary<string, XElement> CmdletNamesAndXml
        {
            get { return cmdletNamesAndXml; }
            private set
            {
                cmdletNamesAndXml = value;
                OnPropertyChanged("CmdletNamesAndXml");
            }
        }

        private DelegateCommand exitCommand;
        private DelegateCommand treeViewItemMouseDoubleClickCommand;
        private DelegateCommand browseFileCommand;
        private DelegateCommand launcWebSiteCommand;

        #region Constructor

        public MainViewModel(): this(
			ServiceLocator.Resolve<IDialogService>())
        { 
        }

        public MainViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            DocumentContents = new ObservableCollection<AvalonDock.DocumentContent>();
            OpenFileDialogBox(null);
        }

        #endregion

        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new DelegateCommand(Exit); 
                }
                return exitCommand;
            }
        }

        public ICommand TreeViewItemMouseDoubleClickCommand
        {
            get
            {
                if (treeViewItemMouseDoubleClickCommand == null)
                {
                    treeViewItemMouseDoubleClickCommand = new DelegateCommand(TreeViewItemMouseClick);
                }
                return treeViewItemMouseDoubleClickCommand;
            }
        }

        public ICommand BrowseFileCommand
        {
            get
            {
                if (browseFileCommand == null)
                {
                    browseFileCommand = new DelegateCommand(OpenFileDialogBox);
                }
                return browseFileCommand;
            }
        }

        public ICommand LauncWebSiteCommand
        {
            get
            {
                if (launcWebSiteCommand == null)
                {
                    launcWebSiteCommand = new DelegateCommand(WebSiteLaunch);
                }
                return launcWebSiteCommand;
            
            }
          
        }

        private void Exit(object parameter)
        {
            Application.Current.Shutdown();
        }

        private void OpenFileDialogBox(object parameter)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "XML Files (*.xml)|*.xml";
            bool? result = open.ShowDialog();
            if (result.HasValue && result.Value)
            {
                try
                {
                    FilePath = open.FileName;
                    FileInfo fileInfo = new FileInfo(FilePath);
                    FileName = fileInfo.Name;
                    XElement xElement = XElement.Parse(File.ReadAllText(FilePath));
                    Dictionary<string, XElement> cmdletNamesAndXmlTemp = new Dictionary<string, XElement>();
                    IEnumerator<XElement> namesXElements = xElement.Elements().GetEnumerator();//.Elements().Where(i => i.Name.LocalName.ToUpperInvariant() == "NAME").GetEnumerator();
                    while (namesXElements.MoveNext())
                    {
                        XElement cmdletXmlNode = namesXElements.Current;
                        XElement cmdletXElement = cmdletXmlNode.Descendants().First(i => i.Name.LocalName.ToUpperInvariant() == "NAME");
                        if (cmdletXElement != null)
                        {
                            string cmdletName = cmdletXElement.Value.Trim();
                            cmdletNamesAndXmlTemp.Add(cmdletName, cmdletXmlNode);
                        }
                    }
                    CmdletNamesAndXml = cmdletNamesAndXmlTemp;
                    DocumentContents.Clear();
                }
                catch (Exception e)
                {
                    
                    MessageBox.Show("File format is not supported", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void TreeViewItemMouseClick(object parameter)
        {
            try
            {
                KeyValuePair<string, XElement> commandElement = (KeyValuePair<string, XElement>)parameter;
                string commandName = commandElement.Key;
                XElement commandNode = commandElement.Value;
                AvalonDock.DocumentContent container = this.DocumentContents.FirstOrDefault(c => c.Title == commandName);
                if (container == null)
                {
                    if (commandNode != null)
                    {
                        CmdletCommand cmdletCommand = new CmdletCommand()
                        {
                            Name = commandName
                        };
                        HelperClass.ParseXElement(commandNode, cmdletCommand);
                        CmdletCommandViewModel cmdletCommandViewModel = new CmdletCommandViewModel(cmdletCommand, dialogService);
                        container = new AvalonDock.DocumentContent();
                        container.Title = cmdletCommandViewModel.CmdletCommand.Name;
                        container.SetBinding(ContentPresenter.ContentProperty, new Binding(String.Empty));
                        container.DataContext = cmdletCommandViewModel;
                        CmdletHelpViewer.Views.CmdletCommandView vw = new Views.CmdletCommandView();
                        container.Content = vw;
                        BitmapImage bitMapImage = new BitmapImage(new Uri(@"\Images\Powershell.png", UriKind.Relative));
                        ImageSource imgSource = bitMapImage;
                        container.Icon = imgSource;
                        DocumentContents.Add(container);
                    }
                }
                ShowCmdletCommand(container);
            }
            catch
            {
            }
        }

        private void ShowCmdletCommand(AvalonDock.DocumentContent container)
        {
            container.Activate();
        }

        private void WebSiteLaunch(object parameter)
        {
            System.Diagnostics.Process.Start(string.Format("http://www.cerebrata.com/?r=CHV"));
        }
    }
}
