using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PokemonAPI.FeatureFlags;
using System;
using System.Collections.Generic;

namespace PokemonAPI.Engines
{
    public interface IAttackDescription
    {

    }
    public class AttackDescription : IAttackDescription
    {
        public AttackDescription(IOptions<ISplitConfigurationOptions> splitConfigOptions, ILogger<AttackDescription> logger)
        {
            if(DateTime.Now > ExpectedExpiration)
            {
                logger.LogWarning($"Feature {Title} has exceeded the expected expiration date. Consider extending the expiration date and cleaning up the flag");
            }

            if(splitConfigOptions?.Value == null)
            {
                logger.LogError($"Missing required splitconfiguration in appsettings.");
                return;
            }

            var splitConfig = splitConfigOptions.Value;
            splitConfig.Features.TryGetValue("PokemonAttacks", out Dictionary<string, string> featureConfig);
            if(featureConfig == null)
            {
                logger.LogError("Missing split config for feature LookbackAuth");
                return;
            }

            featureConfig.TryGetValue("Name", out string featureNameFromConfig);
            if(!string.IsNullOrWhiteSpace(featureNameFromConfig))
            {
                Title = featureNameFromConfig;
            }
        }

        public string Key = "anonymous";
        public string Explanation => "Determined whether to display pokemon attacks or not";
        public string DefaultValue => "off";
        public string Title { get; set; }
        public DateTime ExpectedExpiration => new DateTime(2022, 12, 01);
    }
}
