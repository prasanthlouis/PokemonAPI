using PokemonAPI.FeatureFlags;
using PokemonAPI.FeatureFlags.Providers;
using System.Collections.Generic;
using System.Linq;

namespace PokemonAPI.Ifx
{
    public interface IFeatureFlagTreatment
    {
        string GetFeatureTreatment(IFeatureFlag feature);
    }
    public class FeatureFlagTreatment : IFeatureFlagTreatment
    {
        private IEnumerable<IFeatureFlagProvider> _featureFlagProviders;
        public FeatureFlagTreatment(IEnumerable<IFeatureFlagProvider> featureFlagProviders)
        {
            _featureFlagProviders = featureFlagProviders;
        }
        public string GetFeatureTreatment(IFeatureFlag feature)
        {
            string treatment = string.Empty;
            var featureFlagProvider = _featureFlagProviders.Where(x => x.HasFeature(feature))
                .OrderByDescending(x => x.Priority)
                .FirstOrDefault();

            if (featureFlagProvider != null)
            {
                treatment = featureFlagProvider.GetFeatureTreatment(feature);
            }
            else
            {
                treatment = feature.DefaultValue;
            }
            return treatment;
        }

    }
}
