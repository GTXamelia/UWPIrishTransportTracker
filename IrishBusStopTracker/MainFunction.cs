﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace IrishBusStopTracker
{
	public class ListGroupStyleSelector : GroupStyleSelector
	{
		protected override GroupStyle SelectGroupStyleCore(object group, uint level)
		{
			return (GroupStyle)App.Current.Resources["listViewGroupStyle"];
		}
	}

	public class Errors
	{
		public string ErrorCode { get; set; }
	}

	public class Transport
	{
		public string StopID { get; set; }
		public string Route { get; set; }
		public string ArrivalTime { get; set; }
		public string Duetime { get; set; }
		public string Destination { get; set; }
		public string ImageOperator { get; set; }
	}
}
