using Microsoft.Extensions.Logging;
using Splitio.Services.Client.Classes;
using Splitio.Services.Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace ApiThreeLayerArch.Helpers
{
    public class SplitIoFeatureHelper
	{
		private readonly ILogger<SplitIoFeatureHelper> _logger;
		private static ISplitClient _client;

		public SplitIoFeatureHelper(ILogger<SplitIoFeatureHelper> logger)
        {
			_logger = logger;
        }
		/// <summary>
		/// Singleton Split.io client used to get treatments (features)
		/// </summary>
		private static ISplitClient Client
		{
			get
			{
				if (_client != null)
					return _client;

				var config = new ConfigurationOptions();
				var key = ConfigurationManager.AppSettings["SplitIoApiSdkKey"];
				var factory = new SplitFactory(key, config);
				_client = factory.Client();
				try
				{
					var timeout = 10000;
					_client.BlockUntilReady(timeout);
				}
				catch (Exception ex)
				{		
					//_logger.Error(ex, $"Error creating SplitIo client. Error- {ex.Message}");
					throw ex;
				}
				return _client;
			}
		}

		#region Features

		/// <summary>
		/// Get the feature flag for a particular feature key using optional attributes
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="feature"></param>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public static FeatureFlag GetFeatureFlag(int DemoId, string feature, Dictionary<string, object> attributes = null)
		{
			
			var flag = Client.GetTreatment(DemoId.ToString(), feature, attributes);
			if (flag.Equals("on"))
				return FeatureFlag.On;
			if (flag.Equals("off"))
				return FeatureFlag.Off;

			return FeatureFlag.Control;
		}

		/// <summary>
		/// Get a dictionary of feature flags by feature key using optional attributes
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="features"></param>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public static Dictionary<string, FeatureFlag> GetFeatureFlags(int DemoId, List<string> features, Dictionary<string, object> attributes = null)
		{
			var treatments = Client.GetTreatments(DemoId.ToString(), features, attributes);
			var flags = new Dictionary<string, FeatureFlag>();
			foreach (var feature in treatments.Keys)
			{
				var flag = treatments[feature];
				if (flag.Equals("on"))
					flags.Add(feature, FeatureFlag.On);
				else if (flag.Equals("off"))
					flags.Add(feature, FeatureFlag.Off);
				else
					flags.Add(feature, FeatureFlag.Control);
			}

			return flags;
		}

		#endregion Features

		/// <summary>
		/// Destroy the singleton object - when is this called?
		/// </summary>
		public static void Shutdown()
		{
			if (_client != null)
				Client.Destroy();
			_client = null;
		}
	}
}
