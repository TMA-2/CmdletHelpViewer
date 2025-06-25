using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CmdletHelpViewer.AttachedCommandBehavior
{
    public class DelegateCommand : ICommand
    {
        Func<object, bool> canExecute;
        Action<object> executeAction;
        bool canExecuteCache;

        public DelegateCommand(Action<object> executeAction)
        {
            this.executeAction = executeAction;
        }

        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecute)
        {
            this.executeAction = executeAction;
            this.canExecute = canExecute;
        }

        #region ICommand Members
        public bool CanExecute(object parameter)
        {
            if (canExecute != null)
            {
                bool temp = canExecute(parameter);
                if (canExecuteCache != temp)
                {
                    canExecuteCache = temp;
                    if (CanExecuteChanged != null)
                    {
                        CanExecuteChanged(this, new EventArgs());
                    }
                }
                return canExecuteCache;
            }
            return true;
        }

        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            executeAction(parameter);
        }
        #endregion
    }
}
