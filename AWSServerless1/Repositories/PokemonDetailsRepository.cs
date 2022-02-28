using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Logging;
using PokemonAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonAPI.Repositories
{
    public interface IPokemonDetailsRepository
    {
        Task<PokemonDetails> GetPokemonDetails(string pokemonName);
    }

    public class PokemonDetailsRepository : IPokemonDetailsRepository
    {
        public static AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private readonly string DynamoTableName = "TinyPokemonTable";
        private readonly ILogger<PokemonDetailsRepository> _logger;

        public PokemonDetailsRepository(ILogger<PokemonDetailsRepository> logger)
        {
            _logger = logger;
        }
        public async Task<PokemonDetails> GetPokemonDetails(string pokemonName)
        {
            //TODO: Call this only during initial cloudformation create
            await PopulateDynamoDb();

            var request = new GetItemRequest
            {
                TableName = DynamoTableName,
                Key = new Dictionary<string, AttributeValue>()
            {
                { "PokemonName", new AttributeValue {
                      S = pokemonName
                  } }
            },
                ConsistentRead = true
            };
            var response = await client.GetItemAsync(request);
            var retrievedPokemonName = response.Item["PokemonName"].S;
            var pokemonType = response.Item["PokemonType"].S;

            return new PokemonDetails
            {
                Name = retrievedPokemonName,
                Description = pokemonType
            };


        }

        private async Task PopulateDynamoDb()
        {
            var charizard = new PokemonTable()
            {
                PokemonName = "Charizard",
                PokemonType = "It's a dragon!"
            };

            var chansey = new PokemonTable()
            {
                PokemonName = "Chansey",
                PokemonType = "It's a winged fairy pokemon!"
            };


            DynamoDBContext context = new DynamoDBContext(client);
            var bookBatch = context.CreateBatchWrite<PokemonTable>();
            bookBatch.AddPutItems(new List<PokemonTable> { charizard, chansey });
            try
            {
                await bookBatch.ExecuteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
