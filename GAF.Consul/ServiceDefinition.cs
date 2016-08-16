using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GAF.Consul
{
	[DataContract]
	public class ServiceDefinition
	{
		public ServiceDefinition ()
		{
			Tags = new List<string> ();
			Check = new CheckDefinition ();
		}
		[DataMember]
		public string Id { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public List<string> Tags { get; set; }
		[DataMember]
		public string Address { get; set; }
		[DataMember]
		public int Port { get; set; }
		[DataMember]
		public CheckDefinition Check { get; set; }

	}

	[DataContract]
	public class CheckDefinition
	{
		public CheckDefinition ()
		{
		}

		[DataMember]
		public string Script { get; set; }
		[DataMember]
		public string HTTP { get; set; }
		[DataMember]
		public string TCP { get; set; }
		[DataMember]
		public string Interval { get; set; }
		[DataMember]
		public string Timeout { get; set; }
		[DataMember]
		public int TTL { get; set; }
		[DataMember]
		public string Notes { set; get; }
	}
}


/*

{
  "ID": "redis1",
  "Name": "redis",
  "Tags": [
    "master",
    "v1"
  ],
  "Address": "127.0.0.1",
  "Port": 8000,
  "Check": {
    "Script": "/usr/local/bin/check_redis.py",
    "HTTP": "http://localhost:5000/health",
    "Interval": "10s",
    "TTL": "15s"
  }
}

*/
