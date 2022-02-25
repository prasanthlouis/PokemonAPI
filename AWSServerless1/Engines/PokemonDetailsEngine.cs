

using PokemonAPI.Models;
using PokemonAPI.Repositories;
using System.Threading.Tasks;

namespace PokemonAPI.Engines
{
    public interface IPokemonDetailsEngine
    {
        Task<PokemonDetails> GetPokemonDetails(string pokemonName);


    }
    public class PokemonDetailsEngine : IPokemonDetailsEngine
    {
        public IPokemonDetailsRepository _pokemonDetailsRepository;

        public PokemonDetailsEngine(IPokemonDetailsRepository pokemonDetailsRepository)
        {
            _pokemonDetailsRepository = pokemonDetailsRepository;
        }

        public async Task<PokemonDetails> GetPokemonDetails(string pokemonName)
        {
            return await _pokemonDetailsRepository.GetPokemonDetails(pokemonName);
        }

    }
}
