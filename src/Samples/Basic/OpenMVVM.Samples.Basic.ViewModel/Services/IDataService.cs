namespace OpenMVVM.Samples.Basic.ViewModel.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using OpenMVVM.Samples.Basic.ViewModel.Model;

    public interface IDataService
    {
        Task<IEnumerable<Repository>> GetRepositoriesAsync();
    }
}
