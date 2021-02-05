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
	public partial class ProjectInstaller : System.Configuration.Install.Installer
	{
		private readonly ServiceProcessInstaller processInstaller;
		private readonly System.ServiceProcess.ServiceInstaller serviceInstaller;
		
		public ProjectInstaller()
		{
			InitializeComponent();
			
			processInstaller = new ServiceProcessInstaller();
			serviceInstaller = new System.ServiceProcess.ServiceInstaller();
		}

		private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
		{
			// Service will run under system account
			processInstaller.Account = ServiceAccount.LocalSystem;

			// Service will have Automatic Start Type
			serviceInstaller.StartType = ServiceStartMode.Automatic;
		}
	}
}
