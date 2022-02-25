using PokemonAPI.Engines;
using PokemonAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonAPI.Managers
{
    public interface IPokemonDetailsManager
    {
        Task<PokemonDetails> GetPokemonDetails(string pokemonName);
    }
    public class PokemonDetailsManager : IPokemonDetailsManager
    {
        private readonly IPokemonDetailsEngine _pokemonDetailsEngine;
           
        public PokemonDetailsManager(IPokemonDetailsEngine pokemonDetailsEngine)
        {
            _pokemonDetailsEngine = pokemonDetailsEngine;
        }
        public async Task<PokemonDetails> GetPokemonDetails(string pokemonName)
        {
            return await _pokemonDetailsEngine.GetPokemonDetails(pokemonName);
        }
    }
}
