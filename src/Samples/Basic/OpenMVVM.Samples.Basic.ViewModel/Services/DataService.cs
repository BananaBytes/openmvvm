namespace OpenMVVM.Samples.Basic.ViewModel.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using OpenMVVM.Core.PlatformServices;
    using OpenMVVM.Samples.Basic.ViewModel.Model;

    public class DataService : IDataService
    {
        public const string BaseUrl = "https://api.github.com";

        private readonly IContentDialogService contentDialogService;

        public DataService(IContentDialogService contentDialogService)
        {
            this.contentDialogService = contentDialogService;
        }

        private readonly HttpClient httpClient = new HttpClient();

        public async Task<IEnumerable<Repository>> GetRepositoriesAsync()
        {
            this.httpClient.DefaultRequestHeaders.Accept.Clear();
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            this.httpClient.DefaultRequestHeaders.Add("User-Agent", "OpenMVVM");

            try
            {
                var result = await this.httpClient.GetStringAsync($"{BaseUrl}/repositories");
                return JsonConvert.DeserializeObject<List<Repository>>(result);
            }
            catch (Exception exception)
            {
                await this.contentDialogService.Alert("Error", exception.Message);
            }

            return new List<Repository>();
        }
    }
}
