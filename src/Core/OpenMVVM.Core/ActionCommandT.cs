namespace OpenMVVM.Core
{
    using System;

    public class ActionCommand<T> : IMvvmCommand
    {
        private readonly Func<T, bool> canExecute;

        private readonly Action<T> execute;

        public ActionCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (this.canExecute == null)
            {
                return true;
            }

            if (parameter is T)
            {
                return this.canExecute.Invoke((T)parameter);
            }
            return this.canExecute.Invoke(default(T));
        }

        public void Execute(object parameter)
        {
            if (!this.CanExecute(parameter) || this.execute == null)
            {
                return;
            }

            if (parameter is T)
            {
                this.execute((T)parameter);
            }
            else
            {
                this.execute(default(T));
            }
        }

        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
