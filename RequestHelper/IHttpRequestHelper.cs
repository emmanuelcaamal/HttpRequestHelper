namespace RequestHelper
{
    public interface IHttpRequestHelper
    {
        /// <summary>
        /// Permite crear la base para crear una solicitud HttpClient
        /// </summary>
        /// <param name="Key">Clave del HttpClient registrado en RegisterAPI</param>
        /// <returns>Retorna una interfaz de tipo IHttpClientMethod</returns>
        IHttpClientMethod CreateClient(string Key);
    }
}
