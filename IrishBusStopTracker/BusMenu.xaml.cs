using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace IrishBusStopTracker
{
	public sealed partial class BusTransport : Page
	{
		private List<Transport> listOfStop = new List<Transport>();

		public BusTransport()
		{
			this.InitializeComponent();

			this.SizeChanged += MainPage_SizeChanged;

			// 522691 - gHotel Dublin Road (Galway)
			// 522961 - Opposite Londis Dublin Road (Galway)
			// 522811 - GMIT Dublin Road (Galway)
			// 524351 - Opposite Glenina Heights (Galway)
			// GALWY  - Galway Train Station
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(MainMenu));
		}

		async void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.SizeChanged -= MainPage_SizeChanged;
			await Display();
		}

		private async Task Display()
		{
			try
			{
				Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
				Windows.Storage.StorageFile busStopIDsFile = await storageFolder.GetFileAsync("BusIDs.txt");

				// Loop values
				int i, k;
				string imageBusOp = "";
				int BusStatus = 0;
				int ObjectRetrieval = 5;

				string text = await Windows.Storage.FileIO.ReadTextAsync(busStopIDsFile);

				string[] BusStopID = text.Split(new char[0]);

				for (i = 0; i < BusStopID.Length; i++)
				{

					HttpClient client = new HttpClient();
					client.BaseAddress = new Uri("https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=" + BusStopID[i] + "&format=json");
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					HttpResponseMessage response = client.GetAsync("https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=" + BusStopID[i] + "&format=json").Result;
					var result = response.Content.ReadAsStringAsync().Result;

					var obj = JsonConvert.DeserializeObject<RootObject>(result);

					string[] ssize = (obj.ToString()).Split(new char[0]);

					try
					{

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
								listOfStop.Add(new Transport { StopID = obj.Stopid, Route = ssize[0 + (k * ObjectRetrieval)], ArrivalTime = ssize[1 + (k * ObjectRetrieval)], Duetime = ssize[2 + (k * ObjectRetrieval)], Destination = ssize[3 + (k * ObjectRetrieval)], ImageOperator = imageBusOp });
							}
							else if (Int32.Parse(ssize[2 + (k * ObjectRetrieval)]) == 1)
							{
								listOfStop.Add(new Transport { StopID = obj.Stopid, Route = ssize[0 + (k * ObjectRetrieval)], ArrivalTime = ssize[1 + (k * ObjectRetrieval)], Duetime = ssize[2 + (k * ObjectRetrieval)] + " Minute", Destination = ssize[3 + (k * ObjectRetrieval)], ImageOperator = imageBusOp });
							}
							else
							{
								listOfStop.Add(new Transport { StopID = obj.Stopid, Route = ssize[0 + (k * ObjectRetrieval)], ArrivalTime = ssize[1 + (k * ObjectRetrieval)], Duetime = ssize[2 + (k * ObjectRetrieval)] + " Minutes", Destination = ssize[3 + (k * ObjectRetrieval)], ImageOperator = imageBusOp });
							}
						}

						if (obj.Numberofresults == 0)
						{
							BusStatus++;
						}

					}
					catch (Exception e)
					{
						BusStatus++;
					}

					if (BusStopID.Length == BusStatus)
					{
						imageBusOp = "https://scontent-dub4-1.xx.fbcdn.net/v/t1.0-9/29025830_1727100117346019_1771119452012675072_n.jpg?oh=fd2dad187d5fdfe393123ae90fde4f21&oe=5B30FAAD";
						listOfStop.Add(new Transport { Route = "No", Destination = "busses", Duetime = "operating", ImageOperator = imageBusOp });
					}

					var resultCVS = from act in listOfStop group act by act.StopID into grp orderby grp.Key select grp;
					cvsActivities.Source = resultCVS;
				}
			}
			catch (FileNotFoundException)
			{
				var Error = new Errors();
				Error.ErrorCode = "Please enter an ID before viewing stops";
				this.Frame.Navigate(typeof(AddTransport), Error);
			}
		}
	}
}
