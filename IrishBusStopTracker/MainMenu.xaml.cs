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
	public sealed partial class MainMenu : Page
	{

		public MainMenu()
		{
			this.InitializeComponent();
		}

		private void Add_Transport(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(AddTransport));
		}

		private void View_Busses(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(BusMenu));
		}

		private void View_Trains(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("Test Output");
		}

	}
}