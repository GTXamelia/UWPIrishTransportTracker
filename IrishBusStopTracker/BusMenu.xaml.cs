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
		// Instance of the Transport object in MainFunction.cs
		private List<Transport> listOfStop = new List<Transport>();

		// Sets up page
		public BusTransport()
		{
			this.InitializeComponent();

			this.SizeChanged += MainPage_SizeChanged;
		}

		// Main Menu button to move the user back to the main menu when clicked
		private void Main_Menu(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(MainMenu));
		}

		// Sets up page
		async void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.SizeChanged -= MainPage_SizeChanged;
			await Display();
		}

		// main class for reading file for codes
		private async Task Display()
		{
			try
			{
				// File settings
				Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
				Windows.Storage.StorageFile busStopIDsFile = await storageFolder.GetFileAsync("BusIDs.txt");

				// Variables used
				int i, k;
				string imageBusOp = "";
				int BusStatus = 0;
				int ObjectRetrieval = 5;

				// Read file and store data in string format
				string text = await Windows.Storage.FileIO.ReadTextAsync(busStopIDsFile);

				// split data into an array of codes
				string[] BusStopID = text.Split(new char[0]);

				// For loop that runs depending on data from the split array 'BusStopID'
				for (i = 0; i < BusStopID.Length; i++)
				{
					// Connects to the url and downloads all the JSON text
					// Alters the url by using the code the user
					// Stores JSON data in the BusStatusObject.cs
					HttpClient client = new HttpClient();
					client.BaseAddress = new Uri("https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=" + BusStopID[i] + "&format=json");
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					HttpResponseMessage response = client.GetAsync("https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=" + BusStopID[i] + "&format=json").Result;
					var result = response.Content.ReadAsStringAsync().Result;
					var obj = JsonConvert.DeserializeObject<RootObject>(result);

					// Splits the ToString method from the obj var by spaces
					string[] ssize = (obj.ToString()).Split(new char[0]);

					try
					{
						// For loop that runs depending on the number of buses types found at that stop
						for (k = 0; k < obj.Numberofresults; k++)
						{
							if (ssize[4 + (k * ObjectRetrieval)].Contains("X")) // If the array contains 'X' 
							{
								// 'X' operator is for ExpressWay Buses
								imageBusOp = "http://www.buseireann.ie/img/pictures/1405694022_content_main.jpg"; // Sets img to be that of ExpressWay bus
							}
							else if (ssize[4 + (k * ObjectRetrieval)].Contains("BE")) // If the array contains 'BE' 
							{
								// 'BE' operator is for Bus Éireann Buses
								imageBusOp = "https://c2.staticflickr.com/8/7354/13473687104_6f57f4749f_b.jpg"; // Sets img to be that of Bus Éireann bus
							}
							else if (ssize[4 + (k * ObjectRetrieval)].Contains("bac")) // If the array contains 'bac' 
							{
								// 'bac' operator is for Dublin Bus Buses
								imageBusOp = "http://www.echo.ie/images/Dublin_Bus_27_stock.jpg"; // Sets img to be that of Dublin Bus
							}
							else // Else if bus type is unknown
							{
								// Sets image to generic no bus image
								imageBusOp = "https://st2.depositphotos.com/3068703/6369/v/950/depositphotos_63698389-stock-ilglustration-no-bus-sign-icon-great.jpg";
							}

							// This if statement block is for better due times.
							// Will display 'Due' or '1 Minute' or '2 Minutes, 3 Minutes, 4 Minutes......'
							if (ssize[2 + (k * ObjectRetrieval)].Contains("Due"))
							{
								// Adds contents to an instance of the Tranport object
								listOfStop.Add(new Transport { StopID = obj.Stopid, Route = ssize[0 + (k * ObjectRetrieval)], ArrivalTime = ssize[1 + (k * ObjectRetrieval)], Duetime = ssize[2 + (k * ObjectRetrieval)], Destination = ssize[3 + (k * ObjectRetrieval)], ImageOperator = imageBusOp });
							}
							else if (Int32.Parse(ssize[2 + (k * ObjectRetrieval)]) == 1) // If due time equals '1'
							{
								// Adds contents to an instance of the Tranport object with due time is singular '1' Also adds the image from if statement block above
								listOfStop.Add(new Transport { StopID = obj.Stopid, Route = ssize[0 + (k * ObjectRetrieval)], ArrivalTime = ssize[1 + (k * ObjectRetrieval)], Duetime = ssize[2 + (k * ObjectRetrieval)] + " Minute", Destination = ssize[3 + (k * ObjectRetrieval)], ImageOperator = imageBusOp });
							}
							else // Else the due time must be above 1
							{
								// Adds contents to an instance of the Tranport object with due time is multiple '2','3','4'..... Also adds the image from if statement block above
								listOfStop.Add(new Transport { StopID = obj.Stopid, Route = ssize[0 + (k * ObjectRetrieval)], ArrivalTime = ssize[1 + (k * ObjectRetrieval)], Duetime = ssize[2 + (k * ObjectRetrieval)] + " Minutes", Destination = ssize[3 + (k * ObjectRetrieval)], ImageOperator = imageBusOp });
							}
						}

						// If obj is empty (No buses found)
						if (obj.Numberofresults == 0)
						{
							// Increment counter
							BusStatus++;
						}

					}
					catch (Exception e) // if any error is encountered
					{
						// Increment counter
						BusStatus++;
					}

					// If all the codes return no data then this code will run
					if (BusStopID.Length == BusStatus)
					{
						// Set image to be a no bus logo
						imageBusOp = "https://st2.depositphotos.com/3068703/6369/v/950/depositphotos_63698389-stock-ilglustration-no-bus-sign-icon-great.jpg";

						// Uses Transport object to add the words 'No buses operating' to inform the user that none of the verified codes they entered have buses due
						// This happens mostly when buses stop running past 11:59PM
						listOfStop.Add(new Transport { Route = "No", Destination = "busses", Duetime = "operating", ImageOperator = imageBusOp });
					}

					// Set up grouping
					var resultCVS = from act in listOfStop group act by act.StopID into grp orderby grp.Key select grp;
					cvsActivities.Source = resultCVS;
				}
			}
			catch (FileNotFoundException) // If no file was found means the user has yet to enter a valid bus code
			{
				// Set up error var
				var Error = new Errors();

				// Write to error var telling uuser to enter a code before continuing
				Error.ErrorCode = "Please enter an ID before viewing stops";

				// Move user to AddTransport page with the error code
				this.Frame.Navigate(typeof(AddTransport), Error);
			}
		}
	}
}
