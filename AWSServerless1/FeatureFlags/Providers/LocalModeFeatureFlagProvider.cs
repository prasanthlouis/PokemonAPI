using PokemonAPI.FeatureFlags.Enums;
using System.Collections.Generic;

namespace PokemonAPI.FeatureFlags.Providers
{
    public class LocalModeFeatureFlagProvider : IFeatureFlagProvider
    {
        public int Priority => (int)FeatureFlagPriority.Medium;
        private Dictionary<string, string> dictionary;

        public LocalModeFeatureFlagProvider(IEnumerable<IFeature> features)
        {
            dictionary = new Dictionary<string, string>();
            foreach(var feature in features)
            {
                dictionary.Add(feature.Title, feature.DefaultValue);
            }
        }

        public string GetFeatureTreatment(IFeature feature)
        {
            if(dictionary.ContainsKey(feature.Key))
            {
                return dictionary[feature.Key];
            }

            return feature.DefaultValue;
        }

        public bool HasFeature(IFeature feature)
        {
            return true;
        }

        public void SetFeatureToggle(IFeature feature, string value)
        {
            if(dictionary.ContainsKey(feature.Key))
            {
                dictionary[feature.Key] = value;
            }
            else
            {
                dictionary.Add(feature.Key, value);
            }
        }
    }
}
