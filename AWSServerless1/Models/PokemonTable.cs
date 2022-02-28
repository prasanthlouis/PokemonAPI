using Amazon.DynamoDBv2.DataModel;

namespace PokemonAPI.Models
{
    [DynamoDBTable("TinyPokemonTable")]
    public class PokemonTable
    {
        public string PokemonName { get; set; }
        public string PokemonType { get; set; }
    }
}
