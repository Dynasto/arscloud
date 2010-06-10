using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using ArsCloud.Azure;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace ArsCloud.Worker
{
	public class WorkerRole : RoleEntryPoint
	{
		public override void Run()
		{
			while(true)
			{
				Uri toResize = ResizeRequestManager.GetResizeRequest();
				if(toResize == null)
				{
					Thread.Sleep(60000);
					continue;
				}
				Image original;
				using(Stream src = AvatarManager.GetReadStream(toResize))
				{
					original = Image.FromStream(src);
				}
				int[] sizes = { 16, 32, 64 };
				foreach(int size in sizes)
				{
					Image thumbnail = new Bitmap(size, size);
					Graphics g = Graphics.FromImage(thumbnail);
					g.CompositingQuality = CompositingQuality.GammaCorrected | CompositingQuality.HighQuality;
					g.SmoothingMode = SmoothingMode.HighQuality;
					g.InterpolationMode = InterpolationMode.HighQualityBicubic;
					Rectangle r = new Rectangle(0, 0, size, size);
					g.DrawImage(original, r);
					using(Stream dst = AvatarManager.GetWriteStream(toResize, size))
					{
						thumbnail.Save(dst, original.RawFormat);
					}
				}
			}
		}

		public override bool OnStart()
		{
			// Set the maximum number of concurrent connections 
			ServicePointManager.DefaultConnectionLimit = 12;

			DiagnosticMonitorConfiguration config = DiagnosticMonitor.GetDefaultInitialConfiguration();
			config.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);
			config.DiagnosticInfrastructureLogs.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);
			config.DiagnosticInfrastructureLogs.ScheduledTransferLogLevelFilter = LogLevel.Error;
			DiagnosticMonitor.Start("DiagnosticsConnectionString", config);

			RoleEnvironment.Changing += RoleEnvironmentChanging;
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

			AvatarManager.Initialize();
			ResizeRequestManager.Initialize();

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
