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
        PokemonDetails GetPokemonDetails(string pokemonName);
    }
    public class PokemonDetailsRepository : IPokemonDetailsRepository
    {
        public PokemonDetails GetPokemonDetails(string pokemonName)
        {
            var pokeDictionary = new Dictionary<string, string>();
            pokeDictionary.Add("Charizard", "Fire, Dragons");
            pokeDictionary.Add("Clefairy", "Flying, fluffy");
            return new PokemonDetails()
            {
                Description = "A winged Dragon",
                Name = pokemonName,
                Fruit = null
            };
        }
    }
}
