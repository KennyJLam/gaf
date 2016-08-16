using System;
using System.Runtime.Serialization;

namespace GAF.Consul
{
	[DataContract]
	public class Service
	{
		[DataMember]
		public string ID { get; set; }
		[DataMember(Name = "Service")]
		public string ServiceName { get; set; }
		[DataMember]
		public object Tags { get; set; }
		[DataMember]
		public string Address { get; set; }
		[DataMember]
		public int Port { get; set; }
	}
}

