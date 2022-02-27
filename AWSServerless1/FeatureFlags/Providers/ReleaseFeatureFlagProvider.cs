using PokemonAPI.FeatureFlags.Enums;

namespace PokemonAPI.FeatureFlags.Providers
{
    public class ReleaseFeatureFlagProvider : IFeatureFlagProvider
    {
        public int Priority => (int)FeatureFlagPriority.Maximum;

        public string GetFeatureTreatment(IFeature feature)
        {
            return "off";
        }

        public bool HasFeature(IFeature feature)
        {
            return true;
        }
    }
}
