using System.Collections.Generic;

namespace PokemonAPI.FeatureFlags
{
    public interface ISplitConfigurationOptions
    {
        public string ApiKey { get; set; }
        public int? FeatureRefreshRate { get; set; }
        public int? SegmentRefreshmentRate { get; set; }
        public int? BlockOnCreateUntilReadyMs { get; set; }
        public IDictionary<string, Dictionary<string, string>> Features { get; set; }
    }
}
