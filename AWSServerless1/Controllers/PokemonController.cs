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
        // GET api/values
        [HttpGet]
        public async Task<APIGatewayProxyResponse> GetPokemonDetails()
        {
            try
            {
                var pokemonDetails = await AWSXRayRecorder.Instance.TraceMethod(nameof(GetPokemonDetails), async () =>  await _pokemonDetailsManager.GetPokemonDetails("Charizard"));
                pokemonDetails.PokemonAttacks = _attackDescription.GetPokemonAttackDescription("Charizard");
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
                    Body = "Something went wrong",
                    Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
                };
            }
        }
    }
}
