﻿using System;

namespace GAF.Consul
{
	public class Check
	{
		public string Node { get; set; }
		public string CheckID { get; set; }
		public string Name { get; set; }
		public string Status { get; set; }
		public string Notes { get; set; }
		public string Output { get; set; }
		public string ServiceID { get; set; }
		public string ServiceName { get; set; }
	}
}
