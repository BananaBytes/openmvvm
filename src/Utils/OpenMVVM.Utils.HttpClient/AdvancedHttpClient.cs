namespace OpenMVVM.Web
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    public class AdvancedHttpClient : IHttpClient
    {
        public bool UseGZip { get; set; }

        public async Task<string> GetRawAsync(string url, CancellationToken cancellationToken)
        {
            var httpClientHandler = new HttpClientHandler();
            //if (this.UseGZip)
            //{
            //    httpClientHandler.AutomaticDecompression = DecompressionMethods.GZip;
            //}

            var client = new HttpClient(httpClientHandler);
            var response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);
            if (response != null && (response.StatusCode == HttpStatusCode.OK))
            {
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            return null;
        }

        public async Task<ServiceResult<T>> GetJsonAsync<T>(string url, CancellationToken cancellationToken)
        {
            var httpClientHandler = new HttpClientHandler();
            //if (this.UseGZip)
            //{
            //    httpClientHandler.AutomaticDecompression = DecompressionMethods.GZip;
            //}
            var client = new HttpClient(httpClientHandler);
            try
            {
                var response = await client.GetAsync(url, cancellationToken); //.ConfigureAwait(false);
                if (response != null && (response.StatusCode == HttpStatusCode.OK))
                {
                    var responseContent = await response.Content.ReadAsStringAsync(); //.ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<T>.CreateError(ex);
            }

            return ServiceResult<T>.CreateError(new HttpRequestException());
        }

        public async Task<T> GetXmlAsync<T>(string url, CancellationToken cancellationToken)
        {
            var httpClientHandler = new HttpClientHandler();
            //if (this.UseGZip)
            //{
            //    httpClientHandler.AutomaticDecompression = DecompressionMethods.GZip;
            //}
            var client = new HttpClient(httpClientHandler);
            var response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);
            if (response != null && (response.StatusCode == HttpStatusCode.OK))
            {
                var serializer = new XmlSerializer(typeof(T));

                using (
                    var stringReader = new StringReader(
                        await response.Content.ReadAsStringAsync().ConfigureAwait(false)))
                {
                    var xmlReader = XmlReader.Create(stringReader);
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
            return default(T);
        }

        public async Task<Tuple<HttpStatusCode, string>> CallAsync(
            string verb,
            string url,
            IEnumerable<KeyValuePair<string, string>> headers,
            IEnumerable<KeyValuePair<string, string>> content,
            CancellationToken cancellationToken)
        {
            return await this.CallAsync(verb, url, headers, new FormUrlEncodedContent(content), cancellationToken);
        }

        public async Task<Tuple<HttpStatusCode, string>> CallAsync(
            string verb,
            string url,
            IEnumerable<KeyValuePair<string, string>> headers,
            HttpContent content,
            CancellationToken cancellationToken)
        {
            var httpClientHandler = new HttpClientHandler();
            //if (this.UseGZip)
            //{
            //    httpClientHandler.AutomaticDecompression = DecompressionMethods.GZip;
            //}

            var client = new HttpClient(httpClientHandler);
            var request = new HttpRequestMessage(new HttpMethod(verb), url);
            if (content != null)
            {
                request.Content = content;
            }

            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
            if (response != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return new Tuple<HttpStatusCode, string>(response.StatusCode, responseContent);
            }

            return null;
        }

        public async Task<ServiceResult<bool>> PostJsonAsync<TData>(
            string url,
            IEnumerable<KeyValuePair<string, string>> headers,
            TData data,
            CancellationToken cancellationToken)
        {
            StringContent content = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                "application/json");
            return await this.CallJsonAsync<TData>("POST", url, headers, content, cancellationToken);
        }

        public async Task<ServiceResult<bool>> CallJsonAsync<T>(
            string verb,
            string url,
            IEnumerable<KeyValuePair<string, string>> headers,
            HttpContent content,
            CancellationToken cancellationToken)
        {
            var httpClientHandler = new HttpClientHandler();
            //if (this.UseGZip)
            //{
            //    httpClientHandler.AutomaticDecompression = DecompressionMethods.GZip;
            //}

            var client = new HttpClient(httpClientHandler);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var request = new HttpRequestMessage(new HttpMethod(verb), url);
            if (content != null)
            {
                request.Content = content;
            }

            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            try
            {
                var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}