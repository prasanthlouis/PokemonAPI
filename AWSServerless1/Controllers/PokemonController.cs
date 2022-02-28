using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Core.Strategies;
using Microsoft.AspNetCore.Mvc;
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
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonDetailsManager _pokemonDetailsManager;
        private readonly IAttackDescription _attackDescription;
        public PokemonController(IFeatureAwareFactory featureAwareFactory, IPokemonDetailsManager pokemonDetailsManager, IAttackDescriptionStrategy attackDescriptionStrategy)
        {
            _pokemonDetailsManager = pokemonDetailsManager;
            _attackDescription = featureAwareFactory.CreateAttackDescriptionFactory();
        }
        // GET
        [HttpGet]
        public async Task<APIGatewayProxyResponse> GetPokemonDetails()
        {
            try
            { 
                string pokemonName = "";
                var apiGatewayProxyRequest = HttpContext?.Items?[Amazon.Lambda.AspNetCoreServer.AbstractAspNetCoreFunction.LAMBDA_REQUEST_OBJECT] as APIGatewayProxyRequest;
                var queryStringPokemonName = apiGatewayProxyRequest?.QueryStringParameters["name"];
                if (queryStringPokemonName != null)
                {
                    pokemonName = queryStringPokemonName.Trim();
                }
                if(string.IsNullOrWhiteSpace(pokemonName))
                {
                    pokemonName = "Clefairy";
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
            catch (Exception ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = $"Something went wrong {ex.Message} {ex.InnerException}",
                    Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
                };
            }
        }
    }
}
