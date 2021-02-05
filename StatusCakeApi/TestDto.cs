using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusCakeApi
{
	public class TestDto
	{
		public int TestID { get; set; }
		public bool Paused { get; set; }
		public string TestType { get; set; }
		public string WebsiteName { get; set; }
		public string WebsiteURL { get; set; }
		public int CheckRate { get; set; }
		public int Public { get; set; }
		public string Status { get; set; }
		public string WebsiteHost { get; set; }
		public int NormalisedResponse { get; set; }
		public double Uptime { get; set; }
	}
}
