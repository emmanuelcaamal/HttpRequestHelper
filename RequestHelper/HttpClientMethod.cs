using RequestHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace RequestHelper
{
    public class HttpClientMethod : IHttpClientMethod
    {
        private readonly HttpClient _httpClient;
        private IDictionary<string, string> _queryParameters;
        private IDictionary<string, string> _pathParamters;
        private StringContent? _body;

        public HttpClientMethod(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _queryParameters = new Dictionary<string, string>();
            _pathParamters = new Dictionary<string, string>();
            _body = null;
        }

        /// <summary>
        /// Permite agregar encabezados a un solicitud
        /// </summary>
        /// <param name="name">Indica el nombre del encabezado</param>
        /// <param name="value">Indica el valor del encabezado</param>
        /// <returns>Retorna una interfaz de tipo IHttpClientMethod</returns>
        public IHttpClientMethod AddHeader(string name, string value)
        {
            _httpClient.DefaultRequestHeaders.Add(name, value);
            return this;
        }

        /// <summary>
        /// Permite agregar parámetros en la query de la solicitud
        /// </summary>
        /// <param name="name">Indica el nombre del parámetro</param>
        /// <param name="value">Indica el valor del parámetro</param>
        /// <returns>Retorna una interfaz de tipo IHttpClientMethod</returns>
        public IHttpClientMethod AddQueryParameter(string name, string value)
        {
            _queryParameters.Add(name, value);
            return this;
        }

        /// <summary>
        /// Permite reemplazar el valor de un parámetro existente en la Uri
        /// </summary>
        /// <param name="key">Indica la clave del parámetro que será reemplazado, ejemplo: {id}</param>
        /// <param name="value">Indica el valor del parámetro</param>
        /// <returns>Retorna una interfaz de tipo IHttpClientMethod</returns>
        public IHttpClientMethod AddPathParameter(string key, string value)
        {
            _pathParamters.Add(key, value);
            return this;
        }

        /// <summary>
        /// Permite agregar el cuerpo de una solicitud
        /// </summary>
        /// <param name="body">Objeto con información correspondiente al cuerpo de la solicitud</param>
        /// <returns>Retorna una interfaz de tipo IHttpClientMethod</returns>
        public IHttpClientMethod AddBody(object body)
        {
            string bodySerialized = JsonSerializer.Serialize(body);
            _body = new StringContent(bodySerialized, encoding: Encoding.UTF8, "application/json");
            return this;
        }

        /// <summary>
        /// Ejecuta una solicitud HttpClient
        /// </summary>
        /// <param name="method">Indica el tipo de método de la solcitud</param>
        /// <param name="resource">Indica la dirección del recurso URI</param>
        /// <returns>Retorna un objeto HttpResponseMessage que será procesado del lado del cliente</returns>
        /// <exception cref="Exception"></exception>
        public async Task<HttpResponseMessage> ExecuteAsync(HttpMethod method, string resource)
        {
            return await ProcessRequest(method, resource);
        }

        /// <summary>
        /// Ejecuta una solicitud HttpClient
        /// </summary>
        /// <typeparam name="T">Representa una clase abstracta al que se desea formatear el contenido de la solicitud</typeparam>
        /// <param name="method">Indica el tipo de método de la solcitud</param>
        /// <param name="resource">Indica la dirección del recurso URI</param>
        /// <returns>Retorna un objeto RequestResponseModel con los datos resultantes de la solicitud</returns>
        /// <exception cref="Exception"></exception>
        public async Task<RequestResponseModel<T>> ExecuteAsync<T>(HttpMethod method, string resource)
            where T : class
        {
            try
            {
                var result = new RequestResponseModel<T>();
                var response = await ProcessRequest(method, resource);
                result.Status = (int)response.StatusCode;
                result.ReasonPhrase = response.ReasonPhrase;

                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    if(!string.IsNullOrEmpty(content))
                    {
                        result.Data = JsonSerializer.Deserialize<T>(content.ToString(),
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("HttpClientMethod.ExecuteAsync", ex);
            }
        }

        /// <summary>
        /// Procesa la solicitud Http preconfigurada
        /// </summary>
        /// <param name="method">Indica el tipo de método de la solcitud</param>
        /// <param name="resource">Indica la dirección del recurso URI</param>
        /// <returns>Retorna un objeto HttpResponseMessage que será procesado del lado del cliente</returns>
        /// <exception cref="Exception"></exception>
        private async Task<HttpResponseMessage> ProcessRequest(HttpMethod method, string resource)
        {
            try
            {
                //Incluimos los parametros a la URI
                resource = IncludeQueryParameters(resource);
                //Reemplazamos los parametros en el Path
                resource = ReplacePathParameters(resource);

                if (method == HttpMethod.Post)
                {
                    if (_body == null)
                        throw new ArgumentException("El cuerpo de la solicitud no ha sido definido, use AddBody.");

                    var response = await _httpClient.PostAsync(resource, _body);
                    return response;
                }

                if (method == HttpMethod.Put)
                {
                    if (_body == null)
                        throw new ArgumentException("El cuerpo de la solicitud no ha sido definido, use AddBody.");

                    var response = await _httpClient.PutAsync(resource, _body);
                    return response;
                }

                if (method == HttpMethod.Delete)
                {
                    var response = await _httpClient.DeleteAsync(resource);
                    return response;
                }

                if (method == HttpMethod.Get)
                {
                    var response = await _httpClient.GetAsync(resource);
                    return response;
                }

                throw new ArgumentException($"Metodo {method.ToString()} no soportado.");
            }
            catch (Exception ex)
            {
                throw new Exception("HttpClientMethod.ProcessRequest", ex);
            }
        }

        /// <summary>
        /// Incluye los parámetros en la query del recurso especificados en AddQueryParameter
        /// </summary>
        /// <param name="resource">Direccón del recurso</param>
        /// <returns>Devuelve el mismo recurso con los parámetros en la query</returns>
        private string IncludeQueryParameters(string resource)
        {
            if (_queryParameters.Any())
            {
                var query = HttpUtility.ParseQueryString(string.Empty);
                foreach (var keyValue in _queryParameters)
                {
                    query[keyValue.Key] = keyValue.Value;
                }

                string queryString = query.ToString();
                resource = $"{resource}?{queryString}";
            }

            return resource;
        }

        /// <summary>
        /// Reemplaza los valores de la URI especificados en AddPathParameter
        /// </summary>
        /// <param name="resource">Dirección del recurso</param>
        /// <returns>Devuelve el mismo recurso con los valores del parámetro</returns>
        private string ReplacePathParameters(string resource)
        {
            if (_pathParamters.Any())
            {
                foreach (var keyValue in _pathParamters)
                {
                    resource.Replace(keyValue.Key, keyValue.Value);
                }
            }

            return resource;
        }
    }
}
