using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GAF.Consul
{
	[DataContract]
	public class TaggedAddresses
	{
		[DataMember]
		public string wan { get; set; }
	}
}

