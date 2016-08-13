using System;
using System.Collections.Generic;

namespace GAF.Consul
{
	public class ServiceDefinition
	{
		public ServiceDefinition ()
		{
			Tags = new List<string> ();
			Check = new CheckDefinition ();
		}

		public string Id { get; set; }
		public string Name { get; set; }
		public List<string> Tags { get; set; }
		public string Address { get; set; }
		public int Port { get; set; }
		public CheckDefinition Check { get; set; }

	}

	public class CheckDefinition
	{
		public CheckDefinition ()
		{
		}

		public string Script { get; set; }
		public string HTTP { get; set; }
		public string TCP { get; set; }
		public string Interval { get; set; }
        public string Timeout { get; set; }
		public int TTL { get; set; }
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
