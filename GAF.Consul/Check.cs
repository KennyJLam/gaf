using System;
using System.Runtime.Serialization;

namespace GAF.Consul
{
	[DataContract]
	public class Check
	{
		[DataMember]
		public string Node { get; set; }
		[DataMember]
		public string CheckID { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public string Status { get; set; }
		[DataMember]
		public string Notes { get; set; }
		[DataMember]
		public string Output { get; set; }
		[DataMember]
		public string ServiceID { get; set; }
		[DataMember]
		public string ServiceName { get; set; }
	}
}

