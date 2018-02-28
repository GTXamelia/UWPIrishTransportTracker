using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IrishBusStopTracker
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		private List<Stop> listOfStop = new List<Stop>();

		public MainPage()
		{
			this.InitializeComponent();

			// Loop values
			int i,j,k;

			// 522691 - gHotel Dublin Road (Galway)
			// 522961 - Opposite Londis Dublin Road (Galway)
			// 522811 - GMIT Dublin Road (Galway)
			// 524351 - Opposite Glenina Heights (Galway)

			string[] BusStopID = new string[] { "522691" };

			for (i = 0; i < BusStopID.Length; i++)
			{

				HttpClient client = new HttpClient();
				client.BaseAddress = new Uri("https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=" + BusStopID[i] + "&format=json");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage response = client.GetAsync("https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=" + BusStopID[i] + "&format=json").Result;
				var result = response.Content.ReadAsStringAsync().Result;

				var obj = JsonConvert.DeserializeObject<RootObject>(result);

				string[] ssize = (obj.ToString()).Split(new char[0]);

				Debug.WriteLine(ssize.ToString());

				for (j = 0; j < BusStopID.Length; j++) { 

						for (k = 0; k < obj.Numberofresults; k++)
						{
							listOfStop.Add(new Stop { StopID = obj.Stopid, Route = ssize[0 + (k * 4)], ArrivalTime = ssize[1 + (k * 4)], Duetime = ssize[2 + (k * 4)], Destination = ssize[3 + (k * 4)] });
						}

				}

				MyGridView.ItemsSource = listOfStop;
			}
		}
	}

	public class Stop
	{
		public string StopID { get; set; }
		public string Route { get; set; }
		public string ArrivalTime { get; set; }
		public string Duetime { get; set; }
		public string Destination { get; set; }
	}
}
