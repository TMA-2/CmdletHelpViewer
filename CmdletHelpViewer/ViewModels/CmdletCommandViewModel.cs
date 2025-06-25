using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CmdletHelpViewer.Models;
using System.Windows.Input;
using CmdletHelpViewer.Views;
using System.Windows;
using CmdletHelpViewer.Service;
using System.ComponentModel;
using System.Windows.Data;
using AvalonDock;
using CmdletHelpViewer.AttachedCommandBehavior;

namespace CmdletHelpViewer.ViewModels
{
    public class CmdletCommandViewModel :  ViewModelBase
    {
        private readonly IDialogService dialogService;

        public CmdletCommand CmdletCommand
        {
            get;
            private set;
        }
        private int parameterIndex;
        public int ParameterIndex
        {
            get { return parameterIndex; }
            set
            {
                parameterIndex = value;
                OnPropertyChanged("ParameterIndex");
            }
            
        }



        public CmdletCommandViewModel(CmdletCommand cmdletCommand)
            : this(
            cmdletCommand,
			ServiceLocator.Resolve<IDialogService>())
        {
        }

        public CmdletCommandViewModel(Models.CmdletCommand cmdletCommand, IDialogService dialogService)
        {
            this.CmdletCommand = cmdletCommand;
            this.dialogService = dialogService;
        }

        
        DelegateCommand viewParameterCommand;
        public ICommand ViewParameterCommand
        {
            get{
               if (viewParameterCommand == null)
                {
                    viewParameterCommand = new DelegateCommand(ViewParameter);
                }
                return viewParameterCommand;
           }
        }
       
        public WindowParameterDetailViewModel WindowParameterDetailViewModel
        {
            get;
            set;
        }

        public void ViewParameter(object parameter)
        {
            CmdletParameter cmdletParameter = parameter as CmdletParameter;
            if (cmdletParameter != null)
            {
                // Create the WindowParameterDetail ViewModel
                WindowParameterDetailViewModel = new ViewModels.WindowParameterDetailViewModel(cmdletParameter);
                WindowParameterDetailViewModel.NextCommandRequest += new EventHandler(WindowParameterDetailViewModel_NextCommandRequest);
                WindowParameterDetailViewModel.PreviousCommandRequest += new EventHandler(WindowParameterDetailViewModel_PreviousCommandRequest);
                // Show the dialog
                dialogService.ShowDialog<WindowParameterDetailView>(this, WindowParameterDetailViewModel);
            }
        }

        void WindowParameterDetailViewModel_PreviousCommandRequest(object sender, EventArgs e)
        {
            WindowParameterDetailViewModel = sender as WindowParameterDetailViewModel;
            ParameterIndex = CmdletCommand.Parameters.IndexOf(WindowParameterDetailViewModel.CmdletParameter);
            if (ParameterIndex > 0)
            {
                ParameterIndex--;
            }
            else
            {
                ParameterIndex = CmdletCommand.Parameters.Count - 1;
            }

            WindowParameterDetailViewModel.CmdletParameter = CmdletCommand.Parameters[ParameterIndex];

        }

       
        void WindowParameterDetailViewModel_NextCommandRequest(object sender, EventArgs e)
        {
            WindowParameterDetailViewModel = sender as WindowParameterDetailViewModel;
            ParameterIndex = CmdletCommand.Parameters.IndexOf(WindowParameterDetailViewModel.CmdletParameter);
            if (ParameterIndex + 1 < CmdletCommand.Parameters.Count)
            {
                ParameterIndex++;
            }else
            {
                ParameterIndex = 0;
            }

            WindowParameterDetailViewModel.CmdletParameter = CmdletCommand.Parameters[ParameterIndex];

            //ICollectionView collectionView = CollectionViewSource.GetDefaultView(CmdletCommand.Parameters);
            //if (collectionView != null)
            //    //collectionView.MoveCurrentTo(CmdletCommand.Parameters[index]);
            //    collectionView.MoveCurrentToPosition(ParameterIndex);
        }
    }
}
