

using PokemonAPI.Models;
using PokemonAPI.Repositories;

namespace PokemonAPI.Engines
{
    public interface IPokemonDetailsEngine
    {
        PokemonDetails GetPokemonDetails(string pokemonName);
    }
    public class PokemonDetailsEngine : IPokemonDetailsEngine
    {
        public IPokemonDetailsRepository _pokemonDetailsRepository;

        public PokemonDetailsEngine(IPokemonDetailsRepository pokemonDetailsRepository)
        {
            _pokemonDetailsRepository = pokemonDetailsRepository;
        }

        public PokemonDetails GetPokemonDetails(string pokemonName)
        {
            var pokemonDetails = _pokemonDetailsRepository.GetPokemonDetails(pokemonName);
            return pokemonDetails;
        }

    }
}
