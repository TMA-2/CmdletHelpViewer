using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CmdletHelpViewer.AttachedCommandBehavior;


namespace CmdletHelpViewer.ViewModels
{
    public class WorkspaceViewModel : ViewModelBase
    {

        private DelegateCommand closeCommand;

        public WorkspaceViewModel()
        { 
        
        }

        public ICommand CloseCommand
        {
            get {
                if (closeCommand == null)
                {
                    closeCommand = new DelegateCommand(OnRequestClose);
                }
                return closeCommand;
            } 
        }

        public event EventHandler RequestClose;

        void OnRequestClose(Object parameter)
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
            {
               // handler(this, new RequestCloseEventArgs(){Parameter=parameter});
                handler(this, EventArgs.Empty);
            }
        }
    }

    public class RequestCloseEventArgs : EventArgs
    {
        public Object Parameter
        {
            get;
            set;
        }
    }
}
