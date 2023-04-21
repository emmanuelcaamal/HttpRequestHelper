using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RequestHelper
{
    public interface IHttpClientMethod
    {
        /// <summary>
        /// Permite agregar el cuerpo de una solicitud
        /// </summary>
        /// <param name="body">Objeto con información correspondiente al cuerpo de la solicitud</param>
        /// <returns>Retorna una interfaz de tipo IHttpClientMethod</returns>
        IHttpClientMethod AddBody(object body);

        /// <summary>
        /// Permite agregar encabezados a un solicitud
        /// </summary>
        /// <param name="name">Indica el nombre del encabezado</param>
        /// <param name="value">Indica el valor del encabezado</param>
        /// <returns>Retorna una interfaz de tipo IHttpClientMethod</returns>
        IHttpClientMethod AddHeader(string name, string value);

        /// <summary>
        /// Permite reemplazar el valor de un parámetro existente en la Uri
        /// </summary>
        /// <param name="key">Indica la clave del parámetro que será reemplazado, ejemplo: {id}</param>
        /// <param name="value">Indica el valor del parámetro</param>
        /// <returns>Retorna una interfaz de tipo IHttpClientMethod</returns>
        IHttpClientMethod AddPathParameter(string key, string value);

        /// <summary>
        /// Permite agregar parámetros en la query de la solicitud
        /// </summary>
        /// <param name="name">Indica el nombre del parámetro</param>
        /// <param name="value">Indica el valor del parámetro</param>
        /// <returns>Retorna una interfaz de tipo IHttpClientMethod</returns>
        IHttpClientMethod AddQueryParameter(string name, string value);

        /// <summary>
        /// Ejecuta una solicitud HttpClient
        /// </summary>
        /// <param name="method">Indica el tipo de método de la solcitud</param>
        /// <param name="resource">Indica la dirección del recurso URI</param>
        /// <returns>Retorna un objeto HttpResponseMessage que será procesado del lado del cliente</returns>
        /// <exception cref="Exception"></exception>
        Task<HttpResponseMessage> ExecuteAsync(HttpMethod method, string resource);
    }
}
