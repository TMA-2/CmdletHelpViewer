using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;

namespace CmdletHelpViewer.Service
{
    class DialogService : IDialogService
    {
        private HashSet<FrameworkElement> views;

        public DialogService()
        {
            views = new HashSet<FrameworkElement>();
        }

        public void Register(System.Windows.FrameworkElement view)
        {
            // Get owner window
            Window owner = view as Window;
            if (owner == null)
            {
                owner = Window.GetWindow(view);
            }

            if (owner == null)
            {
                throw new InvalidOperationException("View is not contained within a Window.");
            }

            // Register for owner window closing, since we then should unregister View reference,
            // preventing memory leaks
            owner.Closed += OwnerClosed;

            views.Add(view);
        
        }

        public void Unregister(System.Windows.FrameworkElement view)
        {
            views.Remove(view);
        }

        public bool? ShowDialog<T>(object ownerViewModel, object viewModel) where T : System.Windows.Window
        {
            // Create dialog and set properties
            T dialog = Activator.CreateInstance<T>();
            dialog.Owner = FindOwnerWindow(ownerViewModel);
            dialog.DataContext = viewModel;

            // Show dialog
            return dialog.ShowDialog();
        }

        public System.Windows.MessageBoxResult ShowMessageBox(object ownerViewModel, string messageBoxText, string caption, System.Windows.MessageBoxButton button, System.Windows.MessageBoxImage icon)
        {
            return MessageBox.Show(FindOwnerWindow(ownerViewModel), messageBoxText, caption, button, icon);
        }

        #region Attached properties

        /// <summary>
        /// Attached property describing whether a FrameworkElement is acting as a View in MVVM.
        /// </summary>
        public static readonly DependencyProperty IsRegisteredViewProperty = DependencyProperty.RegisterAttached(
            "IsRegisteredView",
            typeof(bool),
            typeof(DialogService),
            new UIPropertyMetadata(IsRegisteredViewPropertyChanged));


        /// <summary>
        /// Gets value describing whether FrameworkElement is acting as View in MVVM.
        /// </summary>
        public static bool GetIsRegisteredView(FrameworkElement target)
        {
            return (bool)target.GetValue(IsRegisteredViewProperty);
        }


        /// <summary>
        /// Sets value describing whether FrameworkElement is acting as View in MVVM.
        /// </summary>
        public static void SetIsRegisteredView(FrameworkElement target, bool value)
        {
            target.SetValue(IsRegisteredViewProperty, value);
        }


        /// <summary>
        /// Is responsible for handling IsRegisteredViewProperty changes, i.e. whether
        /// FrameworkElement is acting as View in MVVM or not.
        /// </summary>
        private static void IsRegisteredViewPropertyChanged(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            // The Visual Studio Designer or Blend will run this code when setting the attached
            // property, however at that point there is no IDialogService registered
            // in the ServiceLocator which will cause the Resolve method to throw a ArgumentException.
            if (DesignerProperties.GetIsInDesignMode(target)) return;

            FrameworkElement view = target as FrameworkElement;
            if (view != null)
            {
                // Cast values
                bool newValue = (bool)e.NewValue;
                bool oldValue = (bool)e.OldValue;

                if (newValue)
                {
                    ServiceLocator.Resolve<IDialogService>().Register(view);
                }
                else
                {
                    ServiceLocator.Resolve<IDialogService>().Unregister(view);
                }
            }
        }

        #endregion

        /// <summary>
        /// Handles owner window closed, View service should then unregister all Views acting
        /// within the closed window.
        /// </summary>
        private void OwnerClosed(object sender, EventArgs e)
        {
            Window owner = sender as Window;
            if (owner != null)
            {
                // Find Views acting within closed window
                IEnumerable<FrameworkElement> windowViews = views.Where(view => Window.GetWindow(view) == owner);
                // Unregister Views in window
                foreach (FrameworkElement view in windowViews.ToArray())
                {
                   Unregister(view);
                }
            }
        }

        /// <summary>
        /// Finds window corresponding to specified ViewModel.
        /// </summary>
        private Window FindOwnerWindow(object viewModel)
        {
            FrameworkElement view = views.FirstOrDefault();
            //FrameworkElement view = views.SingleOrDefault(v => ReferenceEquals(v.DataContext, viewModel));
            if (view == null)
            {
               throw new ArgumentException("Viewmodel is not referenced by any registered View.");
            }
            
            // Get owner window
            Window owner = view as Window;
            if (owner == null)
            {
                owner = Window.GetWindow(view);
            }

            // Make sure owner window was found
            if (owner == null)
            {
                throw new InvalidOperationException("View is not contained within a Window.");
            }

            return owner;
        }
    }

   
}
