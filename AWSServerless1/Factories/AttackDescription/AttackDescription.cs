using PokemonAPI.FeatureFlags.Providers;
using PokemonAPI.Managers;
using System.Collections.Generic;

namespace PokemonAPI.Factories.AttackDescription
{
    public interface IAttackDescription
    {
        Dictionary<string, string> GetHeaders(string pokemonName);
        string GetPokemonAttackDescription(string pokemonName);
    }
    public class AttackDescription : IAttackDescription
    {
        private IAttackDescriptionStrategy _attackDescriptionStrategy;
        public AttackDescription(IAttackDescriptionStrategy attackDescriptionStrategy)
        {
            _attackDescriptionStrategy = attackDescriptionStrategy;
        }
        public Dictionary<string, string> GetHeaders(string pokemonName)
        {
            return _attackDescriptionStrategy.GetHeaders(pokemonName);
        }

        public string GetPokemonAttackDescription(string pokemonName)
        {
            return _attackDescriptionStrategy.GetPokemonAttackDescription(pokemonName);
        }
    }
}
