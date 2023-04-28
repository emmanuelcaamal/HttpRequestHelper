using Microsoft.AspNetCore.Mvc;
using RequestHelper.APITest.Models;

namespace RequestHelper.APITest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHttpRequestHelper _requestHelper;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpRequestHelper requestHelper)
        {
            _logger = logger;
            _requestHelper = requestHelper;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            //var response = _requestHelper.CreateClient("PokemonAPI")
            //    .AddPathParameter("{clave}", "aegislash")
            //    .ExecuteAsync(HttpMethod.Get, "api/v2/pokemon-species/{clave}").Result;

            //var response = _requestHelper.CreateClient("PokemonAPI")
            //    .AddQueryParameter("limit", "100")
            //    .AddQueryParameter("offset", "0")
            //    .ExecuteAsync(HttpMethod.Get, "api/v2/pokemon").Result;

            //var content = response.Content.ReadAsStringAsync().Result;

            var response = _requestHelper.CreateClient("PokemonAPI")
                .AddQueryParameter("limit", "100")
                .AddQueryParameter("offset", "0")
                .ExecuteAsync<PokemonResponseModel>(HttpMethod.Get, "api/v2/pokemon").Result;

            return Ok(response);
        }
    }
}