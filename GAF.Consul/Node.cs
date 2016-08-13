using System;
using Newtonsoft.Json;

namespace GAF.Consul
{
	public class Node
	{
		[JsonProperty (PropertyName = "Node")]
		public string NodeName { get; set; }
		public string Address { get; set; }
		public TaggedAddresses TaggedAddresses { get; set; }
	}
}

