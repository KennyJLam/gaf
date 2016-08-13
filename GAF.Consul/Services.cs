using System;
using System.Collections.Generic;

namespace GAF.Consul
{
	public class Services
	{
		public Node Node { get; set; }
		public Service Service { get; set; }
		public List<Check> Checks { get; set; }
	}
}


/*	
 * 
 * NOTE: That Node and Service have membernames that match the enclosing class. See attributes on C# classes.
 * 
[
  {
	"Node": {
	  "Node": "foobar",
	  "Address": "10.1.10.12",
	  "TaggedAddresses": {
		"wan": "10.1.10.12"

	  }
	},
	"Service": {
	  "ID": "redis",
	  "Service": "redis",
	  "Tags": null,
	  "Address": "10.1.10.12",
	  "Port": 8000
	},
	"Checks": [
	  {
		"Node": "foobar",
		"CheckID": "service:redis",
		"Name": "Service 'redis' check",
		"Status": "passing",
		"Notes": "",
		"Output": "",
		"ServiceID": "redis",
		"ServiceName": "redis"
	  },
	  {
		"Node": "foobar",
		"CheckID": "serfHealth",
		"Name": "Serf Health Status",
		"Status": "passing",
		"Notes": "",
		"Output": "",
		"ServiceID": "",
		"ServiceName": ""
	  }
	]
  }
]
*/
