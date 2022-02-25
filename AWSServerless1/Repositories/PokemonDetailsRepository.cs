using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
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
    [DynamoDBTable("pokemon-API-PokemonTable-1LQQO161W484B")]
    public class PokemonTable
    {
        public string PokemonName { get; set; }
        public string PokemonType { get; set; }
    }
    public class PokemonDetailsRepository : IPokemonDetailsRepository
    {
        public static AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private readonly string DynamoTableName = "pokemon-API-PokemonTable-1LQQO161W484B";
        public async Task<PokemonDetails> GetPokemonDetails(string pokemonName)
        {
            var book1 = new PokemonTable()
            {
                PokemonName = "notyou",
                PokemonType = "notme"
            };

            var book2 = new PokemonTable()
            {
                PokemonName = "CDR",
                PokemonType = "CDR"
            };
/*            var request = new GetItemRequest
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
            var pokemonType = response.Item["PokemonType"].S;*/

            DynamoDBContext context = new DynamoDBContext(client);
            var bookBatch = context.CreateBatchWrite<PokemonTable>();
            bookBatch.AddPutItems(new List<PokemonTable> { book1, book2 });
            try
            {
                await bookBatch.ExecuteAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return new PokemonDetails
            {
                Name = "",
                Description = ""
            };


        }
    }
}
