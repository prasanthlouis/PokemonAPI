namespace PokemonAPI.FeatureFlags.Providers
{
    public interface IFeatureFlagProvider
    {
        int Priority { get; }
        string GetFeatureTreatment(IFeature feature);
        bool HasFeature(IFeature feature);
    }
}
