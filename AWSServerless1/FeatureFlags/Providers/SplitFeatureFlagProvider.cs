using Microsoft.Extensions.Logging;
using PokemonAPI.Engines;
using PokemonAPI.Factories.AttackDescription;
using PokemonAPI.FeatureFlags.Enums;
using PokemonAPI.Ifx;

namespace PokemonAPI.FeatureFlags.Providers
{
    public class SplitFeatureFlagProvider : IFeatureFlagProvider
    {
        private readonly ISplitCreator _splitCreator;
        private readonly ILogger<SplitFeatureFlagProvider> _logger;
        public int Priority => (int)FeatureFlagPriority.Minimum;

        public SplitFeatureFlagProvider(ISplitCreator splitCreator)
        {
            _splitCreator = splitCreator;
        }
        public string GetFeatureTreatment(IFeature feature)
        {
            var treatment = _splitCreator.SplitClient.GetTreatment(feature.Key, feature.Title);
        if(string.IsNullOrWhiteSpace(treatment) || treatment == "control")
            {
                _logger.LogError($"Could not determine treatment for {feature}");
                return feature.DefaultValue;
            }
            return treatment;
        }

        public bool HasFeature(IFeature feature)
        {
            if (feature is IAttackDescription)
                return true;
            return false;
        }
    }
}
