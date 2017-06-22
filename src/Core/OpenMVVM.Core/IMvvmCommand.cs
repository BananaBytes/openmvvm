namespace OpenMVVM.Core
{
    using System.Windows.Input;

    public interface IMvvmCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}