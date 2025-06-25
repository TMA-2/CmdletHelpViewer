using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CmdletHelpViewer.Models;
using System.Windows.Input;
using CmdletHelpViewer.AttachedCommandBehavior;


namespace CmdletHelpViewer.ViewModels
{
    public class WindowParameterDetailViewModel : WorkspaceViewModel
    {
        private CmdletParameter cmdletParameter;

        public event EventHandler NextCommandRequest;

        public event EventHandler PreviousCommandRequest;

        public CmdletParameter CmdletParameter
        {
            get { return cmdletParameter; }
            set
            {
                cmdletParameter = value;
                OnPropertyChanged("CmdletParameter");
            }
        }

        public WindowParameterDetailViewModel(CmdletParameter cmdletParameter)
        {
            CmdletParameter = cmdletParameter;
            NextCommand = new DelegateCommand(OnNextCommandRequest);
            PreviousCommand = new DelegateCommand(OnPreviousCommandRequest);
        
        }

        public ICommand NextCommand
        {
            get;
            private set;
        }

        public ICommand PreviousCommand
        {
            get;
            private set;
        }

        private void OnNextCommandRequest(object parameter)
        {
            EventHandler handler = this.NextCommandRequest;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void OnPreviousCommandRequest(object parameter)
        {
            EventHandler handler = this.PreviousCommandRequest;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
