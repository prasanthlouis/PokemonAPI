using System.Collections.Generic;

namespace PokemonAPI.Managers
{
    public interface IAttackDescriptionStrategy
    {
        Dictionary<string, string> GetHeaders(string pokemonName);
        string GetPokemonAttackDescription(string pokemonName);
        string Key { get; }
    }
    public class AttackDescriptionManager : IAttackDescriptionStrategy
    {
        public Dictionary<string, string> GetHeaders(string pokemonName)
        {
            return new Dictionary<string, string>()
            {
                { "PokemonName", pokemonName }
            };
        }

        public string GetPokemonAttackDescription(string pokemonName)
        {
            return $"Sample Attack Description of {pokemonName}";
        }
    }

    public class AttackDescriptionManagerDisabled : IAttackDescriptionStrategy
    {
        public Dictionary<string, string> GetHeaders(string pokemonName)
        {
            return new Dictionary<string, string>();

        }

        public string GetPokemonAttackDescription(string pokemonName)
        {
            return "Coming soon!";
        }
    }
}
