using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IrishBusStopTracker
{
	// Reference used by the 3 main pages 'BusMenu.xaml', 'TrainMenu.xaml' and 'LuasMenu.xaml'
	public class ListGroupStyleSelector : GroupStyleSelector
	{
		protected override GroupStyle SelectGroupStyleCore(object group, uint level)
		{
			return (GroupStyle)App.Current.Resources["listViewGroupStyle"];
		}
	}

	// Error object used to transfer error details between pages
	public class Errors
	{
		public string ErrorCode { get; set; }
	}

	// Transport object used to store details needed to bind data to xaml from RootObject
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
