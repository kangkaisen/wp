using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Draw
{
    public class DelegateCommand : ICommand
    {
        private Action<Object> ExecuteParamAction { get; set; }
        private Action ExecuteAction { get; set; }
        private Func<bool> CanExecuteFunc { get; set; }

        public DelegateCommand(Action execute)
        {
            this.ExecuteAction = execute;
            this.CanExecuteFunc = () => true;
        }

        public DelegateCommand(Action<object> executeParam)
        {
            this.ExecuteParamAction = executeParam;
            this.CanExecuteFunc = () => true;
        }
        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            this.ExecuteAction = execute;
            this.CanExecuteFunc = canExecute;
        }

        public DelegateCommand(Action<object> executeParam, Func<bool> canExecute)
        {
            this.ExecuteParamAction = executeParam;
            this.CanExecuteFunc = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            return this.CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged;
        public void RaiseExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public void Execute(object parameter)
        {
            if (this.ExecuteAction != null)
            {
                this.ExecuteAction();
            }
            else if (this.ExecuteParamAction != null)
            {
                this.ExecuteParamAction(parameter);
            }

        }
    }
}
