using System.Net.Http;

namespace RequestHelper
{
    public class HttpRequestHelper : IHttpRequestHelper
    {
        private readonly IHttpClientFactory _clientFactory;

        public HttpRequestHelper(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Permite crear la base para crear una solicitud HttpClient
        /// </summary>
        /// <param name="Key">Clave del HttpClient registrado en RegisterAPI</param>
        /// <returns>Retorna una interfaz de tipo IHttpClientMethod</returns>
        public IHttpClientMethod CreateClient(string Key)
        {
            var httpClient = _clientFactory.CreateClient(Key);
            return new HttpClientMethod(httpClient);
        }
    }
}
