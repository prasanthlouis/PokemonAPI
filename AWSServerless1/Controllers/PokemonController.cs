using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.AspNetCore.Mvc;
using PokemonAPI.Managers;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace AWSServerless1.Controllers
{
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonDetailsManager _pokemonDetailsManager;
        public PokemonController(IPokemonDetailsManager pokemonDetailsManager)
        {
            _pokemonDetailsManager = pokemonDetailsManager;
        }
        // GET api/values
        [HttpGet]
        public APIGatewayProxyResponse GetPokemonDetails()
        {
            var pokemonDetails = _pokemonDetailsManager.GetPokemonDetails("Charizard");

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = JsonSerializer.Serialize(pokemonDetails),
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };

            return response;
        }
    }
}
