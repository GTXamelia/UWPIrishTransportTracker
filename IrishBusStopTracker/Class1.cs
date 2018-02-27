﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrishBusStopTracker
{
	public class Result
	{
		public string Arrivaldatetime { get; set; }
		public string Duetime { get; set; }
		public string Departuredatetime { get; set; }
		public string Departureduetime { get; set; }
		public string Scheduledarrivaldatetime { get; set; }
		public string Scheduleddeparturedatetime { get; set; }
		public string Destination { get; set; }
		public string Destinationlocalized { get; set; }
		public string Origin { get; set; }
		public string Originlocalized { get; set; }
		public string Direction { get; set; }
		public string Operator { get; set; }
		public string Additionalinformation { get; set; }
		public string Lowfloorstatus { get; set; }
		public string Route { get; set; }
		public string Sourcetimestamp { get; set; }
		public string Monitored { get; set; }
	}

	// This class contains header information about the JSON data
	public class RootObject
	{
		public string Errorcode { get; set; }
		public string Errormessage { get; set; }
		public int Numberofresults { get; set; }
		public string Stopid { get; set; }
		public string Timestamp { get; set; }
		public List<Result> Results { get; set; }
	}
}
