using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using XRMStatus;

namespace StatusMonitorService
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
			ServiceBase[] ServicesToRun;
			
			var appSettings = ConfigurationManager.AppSettings;  
			
			ServicesToRun = new ServiceBase[]
			{
				new WindowsService()
			};
			ServiceBase.Run(ServicesToRun);
		}
	}
}
