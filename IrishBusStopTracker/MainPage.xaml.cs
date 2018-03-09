using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Windows.UI.Xaml.Controls;


namespace IrishBusStopTracker
{
	public sealed partial class MainPage : Page
	{
		private List<Stop> listOfStop = new List<Stop>();

		public MainPage()
		{
			this.InitializeComponent();

			// Loop values
			int i, k;
			string imageBusOp = "";
			int BusStatus = 0;
			int ObjectRetrieval = 5;

			// 522691 - gHotel Dublin Road (Galway)
			// 522961 - Opposite Londis Dublin Road (Galway)
			// 522811 - GMIT Dublin Road (Galway)
			// 524351 - Opposite Glenina Heights (Galway)

			string[] BusStopID = new string[] { "522691", "522961", "GLGRY", "524351" };

			for (i = 0; i < BusStopID.Length; i++)
			{

				HttpClient client = new HttpClient();
				client.BaseAddress = new Uri("https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=" + BusStopID[i] + "&format=json");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage response = client.GetAsync("https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=" + BusStopID[i] + "&format=json").Result;
				var result = response.Content.ReadAsStringAsync().Result;

				var obj = JsonConvert.DeserializeObject<RootObject>(result);

				string[] ssize = (obj.ToString()).Split(new char[0]);

				try { 

					for (k = 0; k < obj.Numberofresults; k++)
					{
						if (ssize[0 + (k * ObjectRetrieval)].Contains("X"))
						{
							imageBusOp = "http://www.buseireann.ie/img/pictures/1405694022_content_main.jpg";
						}
						else if (ssize[4 + (k * ObjectRetrieval)].Contains("BE"))
						{
							imageBusOp = "https://c2.staticflickr.com/8/7354/13473687104_6f57f4749f_b.jpg";
						}
						else if (ssize[4 + (k * ObjectRetrieval)].Contains("bac"))
						{
							imageBusOp = "http://www.echo.ie/images/Dublin_Bus_27_stock.jpg";
						}
						else
						{
							imageBusOp = "https://st2.depositphotos.com/3068703/6369/v/950/depositphotos_63698389-stock-ilglustration-no-bus-sign-icon-great.jpg";
						}

						if (ssize[2 + (k * ObjectRetrieval)].Contains("Due"))
						{
							listOfStop.Add(new Stop { StopID = obj.Stopid, Route = ssize[0 + (k * ObjectRetrieval)], ArrivalTime = ssize[1 + (k * ObjectRetrieval)], Duetime = ssize[2 + (k * ObjectRetrieval)], Destination = ssize[3 + (k * ObjectRetrieval)], ImageOperator = imageBusOp });
						}
						else if (Int32.Parse(ssize[2 + (k * 5)]) == 1)
						{
							listOfStop.Add(new Stop { StopID = obj.Stopid, Route = ssize[0 + (k * ObjectRetrieval)], ArrivalTime = ssize[1 + (k * ObjectRetrieval)], Duetime = ssize[2 + (k * ObjectRetrieval)] + " Minute", Destination = ssize[3 + (k * ObjectRetrieval)], ImageOperator = imageBusOp });
						}
						else
						{
							listOfStop.Add(new Stop { StopID = obj.Stopid, Route = ssize[0 + (k * ObjectRetrieval)], ArrivalTime = ssize[1 + (k * ObjectRetrieval)], Duetime = ssize[2 + (k * ObjectRetrieval)] + " Minutes", Destination = ssize[3 + (k * ObjectRetrieval)], ImageOperator = imageBusOp });
						}
					}

				}
				catch(Exception e)
				{
					BusStatus++;

					if (BusStatus == 1)
					{
						imageBusOp = "https://st2.depositphotos.com/3068703/6369/v/950/depositphotos_63698389-stock-ilglustration-no-bus-sign-icon-great.jpg";
						listOfStop.Add(new Stop { Route = "No", Destination = "busses", Duetime = "operating", ImageOperator = imageBusOp });
					}
				}

				var resultCVS = from act in listOfStop group act by act.StopID into grp orderby grp.Key select grp;
				cvsActivities.Source = resultCVS;
			}
		}
	}
	

	public class ListGroupStyleSelector : GroupStyleSelector
	{
		protected override GroupStyle SelectGroupStyleCore(object group, uint level)
		{
			return (GroupStyle)App.Current.Resources["listViewGroupStyle"];
		}
	}


	public class Stop
	{
		public string StopID { get; set; }
		public string Route { get; set; }
		public string ArrivalTime { get; set; }
		public string Duetime { get; set; }
		public string Destination { get; set; }
		public string ImageOperator { get; set; }
	}
}