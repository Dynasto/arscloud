using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using ArsCloudWeb.Data;

namespace ArsCloudWeb
{
	public class WebRole : RoleEntryPoint
	{
		public override bool OnStart()
		{
			DiagnosticMonitorConfiguration config = DiagnosticMonitor.GetDefaultInitialConfiguration();
			config.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);
			config.DiagnosticInfrastructureLogs.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);
			config.DiagnosticInfrastructureLogs.ScheduledTransferLogLevelFilter = LogLevel.Error;
			DiagnosticMonitor.Start("DiagnosticsConnectionString", config);

			// For information on handling configuration changes
			// see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
			//RoleEnvironment.Changing += RoleEnvironmentChanging;
			CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
			{
				configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
				RoleEnvironment.Changed += (anotherSender, arg) =>
				{
					if(arg.Changes.OfType<RoleEnvironmentConfigurationSettingChange>()
						.Any((change) => (change.ConfigurationSettingName == configName)))
					{
						if(!configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)))
						{
							RoleEnvironment.RequestRecycle();
						}
					}
				};
			});

			ChirpManager.Initialize();

			return base.OnStart();
		}

		private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
		{
			// If a configuration setting is changing
			if(e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
			{
				// Set e.Cancel to true to restart this role instance
				e.Cancel = true;
			}
		}
	}
}
