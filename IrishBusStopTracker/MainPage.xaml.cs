using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace IrishBusStopTracker
{
	public sealed partial class ListBus : Page
	{
		private List<Transport> listOfStop = new List<Transport>();

		public ListBus()
		{
			this.InitializeComponent();

			
		}

		private void Add_Transport(object sender, RoutedEventArgs e)
		{
			//Add page
		}

		private void View_Busses(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(ListTransport));
		}

		private void View_Trains(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("Test Output");
		}

	}
}