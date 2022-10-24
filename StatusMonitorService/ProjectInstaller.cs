using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace StatusMonitorService
{
	[RunInstaller(true)]
	public partial class ServiceInstaller : System.Configuration.Install.Installer
	{
		private readonly ServiceProcessInstaller processInstaller;
		private readonly System.ServiceProcess.ServiceInstaller serviceInstaller;
		
		public ServiceInstaller()
		{
			InitializeComponent();
			
			processInstaller = new ServiceProcessInstaller();
			serviceInstaller = new System.ServiceProcess.ServiceInstaller();
		}

		private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
		{
			using (ServiceController sc = new ServiceController("StatusMonitor"))
			{
				sc.Start();
			}
		}
	}
}
