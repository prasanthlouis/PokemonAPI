using System;

namespace PokemonAPI.FeatureFlags.Providers
{
    public interface IFeature
    {
        string Key { get; }
        string Title { get; set; }
        string Explanation { get; set; }
        string DefaultValue { get; }
        DateTime ExepectedExpiration { get; }
    }
}
