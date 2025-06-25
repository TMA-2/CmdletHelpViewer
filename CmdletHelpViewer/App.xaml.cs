using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using CmdletHelpViewer.Service;

namespace CmdletHelpViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            // Configure service locator
            ServiceLocator.RegisterSingleton<IDialogService, DialogService>();

            // Create the ViewModel and expose it using the View's DataContext
            Views.MainView view = new Views.MainView();
            view.ContentRendered += new EventHandler(view_ContentRendered);
            view.Show();
        }

        void view_ContentRendered(object sender, EventArgs e)
        {
            Views.MainView view = sender as Views.MainView;
            view.DataContext = new ViewModels.MainViewModel();
        }
    }
}
