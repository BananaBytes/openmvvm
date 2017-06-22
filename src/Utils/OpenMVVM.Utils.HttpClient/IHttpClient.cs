namespace OpenMVVM.Web
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IHttpClient
    {
        Task<string> GetRawAsync(string url, CancellationToken cancellationToken);

        Task<ServiceResult<T>> GetJsonAsync<T>(string url, CancellationToken cancellationToken);

        Task<T> GetXmlAsync<T>(string url, CancellationToken cancellationToken);

        Task<Tuple<HttpStatusCode, string>> CallAsync(
            string verb,
            string url,
            IEnumerable<KeyValuePair<string, string>> headers,
            IEnumerable<KeyValuePair<string, string>> content,
            CancellationToken cancellationToken);

        Task<ServiceResult<bool>> PostJsonAsync<TData>(
            string url,
            IEnumerable<KeyValuePair<string, string>> headers,
            TData data,
            CancellationToken cancellationToken);

        Task<ServiceResult<bool>> CallJsonAsync<T>(
            string verb,
            string url,
            IEnumerable<KeyValuePair<string, string>> headers,
            HttpContent content,
            CancellationToken cancellationToken);
    }
}
