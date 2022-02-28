using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Core.Strategies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokemonAPI.Factories.AttackDescription;
using PokemonAPI.Ifx;
using PokemonAPI.Managers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokemonAPI.Controllers
{
    [Route("api/v1/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonDetailsManager _pokemonDetailsManager;
        private readonly IAttackDescription _attackDescription;
        private readonly ILogger<PokemonController> _logger;
        public PokemonController(IFeatureAwareFactory featureAwareFactory, IPokemonDetailsManager pokemonDetailsManager, IAttackDescriptionStrategy attackDescriptionStrategy, ILogger<PokemonController> logger)
        {
            _pokemonDetailsManager = pokemonDetailsManager;
            _attackDescription = featureAwareFactory.CreateAttackDescriptionFactory();
            _logger = logger;
        }

        /// <summary>
        /// Fetches a certain pokemon.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/v1/Pokemon?name=Charizard
        ///
        /// </remarks>
        /// <returns>Details about the pokemon</returns>
        /// <response code="200">Returns the found pokemon</response>
        /// <response code="404">Could not find the pokemon</response> 
        /// <response code="500">Something went wrong</response> 
        [HttpGet]
        public async Task<APIGatewayProxyResponse> GetPokemonDetails()
        { 
                var apiGatewayProxyRequest = HttpContext?.Items?[Amazon.Lambda.AspNetCoreServer.AbstractAspNetCoreFunction.LAMBDA_REQUEST_OBJECT] as APIGatewayProxyRequest;
                var pokemonName = apiGatewayProxyRequest?.QueryStringParameters["name"];
                if (string.IsNullOrWhiteSpace(pokemonName))
                {
                    _logger.LogError("Could not find pokemon name");
                    throw new Exception("Could not find pokemon name");
                }
                var pokemonDetails = await AWSXRayRecorder.Instance.TraceMethod(nameof(GetPokemonDetails), async () =>  await _pokemonDetailsManager.GetPokemonDetails(pokemonName));
                pokemonDetails.PokemonAttacks = _attackDescription.GetPokemonAttackDescription(pokemonName);
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
