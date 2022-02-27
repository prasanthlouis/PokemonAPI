using PokemonAPI.Engines;
using PokemonAPI.FeatureFlags;
using PokemonAPI.Ifx;
using PokemonAPI.Managers;
using System.Collections.Generic;
using System.Linq;

namespace PokemonAPI.Factories.AttackDescription
{
    public interface IAttackDescriptionFactory
    {
        string CreateAttackDescription(string pokemonName);
    }
    public class AttackDescriptionFactory : IAttackDescriptionFactory
    {
        private IEnumerable<IAttackDescriptionStrategy> _strategies;
        private IFeatureFlag _featureFlag;
        private IFeatureFlagTreatment _featureFlagTreatment;
        private IAttackDescriptionStrategy _strategy;
        public AttackDescriptionFactory(IEnumerable<IAttackDescriptionStrategy> strategies, IFeatureFlag featureFlag, IFeatureFlagTreatment featureFlagTreatment)
        {
            _featureFlag = featureFlag;
            _strategies = strategies;
            _featureFlagTreatment = featureFlagTreatment;
        }
        public string CreateAttackDescription(string pokemonName)
        {
            if(_strategy == null)
            {
                _strategy = GetAttackDescriptionStrategy();
            }

            return _strategy.GetPokemonAttackDescription(pokemonName);
        }

        private IAttackDescriptionStrategy GetAttackDescriptionStrategy()
        {
            var treatment = _featureFlagTreatment.GetFeatureTreatment(_featureFlag._attackDescription);
            return _strategies.FirstOrDefault(x => x.Key == treatment);
        }
    }
}
