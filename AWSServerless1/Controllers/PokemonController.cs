using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.AspNetCore.Mvc;
using PokemonAPI.Managers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

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
        public async Task<APIGatewayProxyResponse> GetPokemonDetails()
        {

                var pokemonDetails = await _pokemonDetailsManager.GetPokemonDetails("Charizard");
               
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
