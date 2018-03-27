using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IrishBusStopTracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TrainMenu : Page
    {
		private List<TransportTrain> listOfStop = new List<TransportTrain>();

		public TrainMenu()
        {
			this.InitializeComponent();

			this.SizeChanged += MainPage_SizeChanged;
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
				Windows.Storage.StorageFile busStopIDsFile = await storageFolder.GetFileAsync("TrainIDs.txt");

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
							if (ssize[0 + (k * ObjectRetrieval)].Contains("ir"))
							{
								imageBusOp = "https://www.railjournal.com/media/k2/items/cache/8625251b6ea82455a3caf137b4aea8ab_XL.jpg?t=943938000";
							}
							else
							{
								imageBusOp = "https://st2.depositphotos.com/3068703/6369/v/950/depositphotos_63698389-stock-ilglustration-no-bus-sign-icon-great.jpg";
							}

							if (ssize[2 + (k * ObjectRetrieval)].Contains("Due"))
							{
								listOfStop.Add(new TransportTrain { StopID = obj.Stopid, Route = ssize[0 + (k * ObjectRetrieval)], ArrivalTime = ssize[1 + (k * ObjectRetrieval)], Duetime = ssize[2 + (k * ObjectRetrieval)], Destination = ssize[3 + (k * ObjectRetrieval)], ImageOperator = imageBusOp });
							}
							else if (Int32.Parse(ssize[2 + (k * ObjectRetrieval)]) == 1)
							{
								listOfStop.Add(new TransportTrain { StopID = obj.Stopid, Route = ssize[0 + (k * ObjectRetrieval)], ArrivalTime = ssize[1 + (k * ObjectRetrieval)], Duetime = ssize[2 + (k * ObjectRetrieval)] + " Minute", Destination = ssize[3 + (k * ObjectRetrieval)], ImageOperator = imageBusOp });
							}
							else
							{
								listOfStop.Add(new TransportTrain { StopID = obj.Stopid, Route = ssize[0 + (k * ObjectRetrieval)], ArrivalTime = ssize[1 + (k * ObjectRetrieval)], Duetime = ssize[2 + (k * ObjectRetrieval)] + " Minutes", Destination = ssize[3 + (k * ObjectRetrieval)], ImageOperator = imageBusOp });
							}
						}

						if (obj.Numberofresults == 0)
						{
							BusStatus++;
						}

					}
					catch (Exception)
					{
						BusStatus++;
					}

					if (BusStopID.Length == BusStatus)
					{
						imageBusOp = "https://scontent-dub4-1.xx.fbcdn.net/v/t1.0-9/29025830_1727100117346019_1771119452012675072_n.jpg?oh=fd2dad187d5fdfe393123ae90fde4f21&oe=5B30FAAD";
						listOfStop.Add(new TransportTrain { Route = "No", Destination = "busses", Duetime = "operating", ImageOperator = imageBusOp });
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

	public class TransportTrain
	{
		public string StopID { get; set; }
		public string Route { get; set; }
		public string ArrivalTime { get; set; }
		public string Duetime { get; set; }
		public string Destination { get; set; }
		public string ImageOperator { get; set; }
	}
}
