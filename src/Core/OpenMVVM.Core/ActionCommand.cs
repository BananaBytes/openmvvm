namespace OpenMVVM.Core
{
    using System;

    public class ActionCommand : IMvvmCommand
    {
        private readonly Func<bool> canExecute;

        private readonly Action execute;

        public ActionCommand(Action execute, Func<bool> canExecute = null)
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
            return this.canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            if (!this.CanExecute(parameter) || this.execute == null)
            {
                return;
            }

            this.execute();
        }

        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}