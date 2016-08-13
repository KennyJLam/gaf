using System;
using Newtonsoft.Json;

namespace GAF.Consul
{
	public class Service
	{
		public string ID { get; set; }
		[JsonProperty (PropertyName = "Service")]
		public string ServiceName { get; set; }
		public object Tags { get; set; }
		public string Address { get; set; }
		public int Port { get; set; }
	}
}

