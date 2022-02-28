

using PokemonAPI.FeatureFlags;
using PokemonAPI.Ifx;
using PokemonAPI.Managers;
using System.Collections.Generic;
using System.Linq;

namespace PokemonAPI.Factories.AttackDescription
{
    public interface IAttackDescriptionFactory
    {
        IAttackDescription CreateAttackDescription();
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
        public IAttackDescription CreateAttackDescription()
        {
            if(_strategy == null)
            {
                _strategy = GetAttackDescriptionStrategy();
            }

            return new AttackDescription(_strategy);
        }

        private IAttackDescriptionStrategy GetAttackDescriptionStrategy()
        {
            var treatment = _featureFlagTreatment.GetFeatureTreatment(_featureFlag._attackDescription);
            return _strategies.FirstOrDefault(x => x.Key == treatment);
        }
    }
}
