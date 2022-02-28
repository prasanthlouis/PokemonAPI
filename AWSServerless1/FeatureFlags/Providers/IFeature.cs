using System;

namespace PokemonAPI.FeatureFlags.Providers
{
    public interface IFeature
    {
        string Key { get; }
        string Title { get; set; }
        string Explanation { get; }
        string DefaultValue { get; }
        DateTime ExpectedExpiration { get; }
    }
}
