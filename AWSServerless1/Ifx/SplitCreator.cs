﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PokemonAPI.FeatureFlags;
using Splitio.Services.Client.Classes;
using Splitio.Services.Client.Interfaces;
using System;

namespace PokemonAPI.Ifx
{
    public interface ISplitCreator
    {
        ISplitClient SplitClient { get; set; }
    }
    public class SplitCreator : ISplitCreator, IDisposable
    {
        private SplitConfigurationOptions _splitConfig;
        public ISplitClient SplitClient { get; set; }

        public SplitCreator(IOptions<SplitConfigurationOptions> splitConfigOptions, ILogger<SplitCreator> logger)
        {
            _splitConfig = splitConfigOptions.Value;
            var config = new ConfigurationOptions
            {
                FeaturesRefreshRate = _splitConfig.FeatureRefreshRate.Value,
                SegmentsRefreshRate = _splitConfig.SegmentRefreshmentRate.Value
            };

            if(string.IsNullOrWhiteSpace(_splitConfig.ApiKey))
            {
                logger.LogError("SplitConfig apiKey was not found in appsettings");
            }

            try
            {

                var splitFactory = new SplitFactory(_splitConfig.ApiKey, config);
                SplitClient = splitFactory.Client();

                var blockOnCreateUntilReadyMs = _splitConfig.BlockOnCreateUntilReadyMs ?? 1000;
                SplitClient.BlockUntilReady(blockOnCreateUntilReadyMs);
            }
            catch(Exception ex)
            {
                
                SplitClient?.Destroy();
            }

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(SplitClient != null)
                {
                    SplitClient.Destroy();
                    SplitClient = null;
                }
            }
        }

    }
}
