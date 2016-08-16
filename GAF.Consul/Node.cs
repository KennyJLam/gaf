using System;
using System.Runtime.Serialization;

namespace GAF.Consul
{
	[DataContract]
	public class Node
	{
		[DataMember(Name = "Node")]
		public string NodeName { get; set; }
		[DataMember]
		public string Address { get; set; }
		[DataMember]
		public TaggedAddresses TaggedAddresses { get; set; }
	}
}

