using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IrishBusStopTracker
{
	public sealed partial class TrainMenu : Page
	{
		// Instance of the Transport object in MainFunction.cs
		private List<Transport> listOfStop = new List<Transport>();

		// Sets up page
		public TrainMenu()
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
				Windows.Storage.StorageFile trainStopIDsFile = await storageFolder.GetFileAsync("TrainIDs.txt");

				// Variables used
				int i, k;
				string imageOperator = "";
				int StatusCounter = 0;
				int ObjectRetrieval = 5;

				// Read file and store data in string format
				string text = await Windows.Storage.FileIO.ReadTextAsync(trainStopIDsFile);

				// split data into an array of codes
				string[] StationID = text.Split(new char[0]);

				// For loop that runs depending on data from the split array 'StationID'
				for (i = 0; i < StationID.Length; i++)
				{
					// Connects to the url and downloads all the JSON text
					// Alters the url by using the code the user
					// Stores JSON data in the TransportJsonParsed.cs
					HttpClient client = new HttpClient();
					client.BaseAddress = new Uri("https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=" + StationID[i] + "&format=json");
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					HttpResponseMessage response = client.GetAsync("https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=" + StationID[i] + "&format=json").Result;
					var result = response.Content.ReadAsStringAsync().Result;
					var obj = JsonConvert.DeserializeObject<RootObject>(result);

					// Splits the ToString method from the obj var by spaces
					string[] ssize = (obj.ToString()).Split(new char[0]);

					try
					{
						// For loop that runs depending on the number of trains found at that station
						for (k = 0; k < obj.Numberofresults; k++)
						{
							if (ssize[4 + (k * ObjectRetrieval)].Contains("ir")) // If the array contains 'ir' 
							{
								// 'X' operator is for Irish Rail Trains
								imageOperator = "https://www.railjournal.com/media/k2/items/cache/8625251b6ea82455a3caf137b4aea8ab_XL.jpg?t=943938000"; // Sets img to be that of irish Rail Dart train
							}
							else
							{
								// Sets image to generic no bus image
								imageOperator = "https://st2.depositphotos.com/3068703/6369/v/950/depositphotos_63698389-stock-ilglustration-no-bus-sign-icon-great.jpg";
							}

							// This if statement block is for better due times.
							// Will display 'Due' or '1 Minute' or '2 Minutes, 3 Minutes, 4 Minutes......'
							if (ssize[2 + (k * ObjectRetrieval)].Contains("Due"))
							{
								// Adds contents to an instance of the Tranport object with due time equaling 'Due' Also adds the image from if statement block above
								listOfStop.Add(new Transport { StopID = obj.Stopid, Route = ssize[0 + (k * ObjectRetrieval)], ArrivalTime = ssize[1 + (k * ObjectRetrieval)], Duetime = ssize[2 + (k * ObjectRetrieval)], Destination = ssize[3 + (k * ObjectRetrieval)], ImageOperator = imageOperator });
							}
							else if (Int32.Parse(ssize[2 + (k * ObjectRetrieval)]) == 1) // If due time equals '1'
							{
								// Adds contents to an instance of the Tranport object with due time is singular '1' Also adds the image from if statement block above
								listOfStop.Add(new Transport { StopID = obj.Stopid, Route = ssize[0 + (k * ObjectRetrieval)], ArrivalTime = ssize[1 + (k * ObjectRetrieval)], Duetime = ssize[2 + (k * ObjectRetrieval)] + " Minute", Destination = ssize[3 + (k * ObjectRetrieval)], ImageOperator = imageOperator });
							}
							else // Else the due time must be above 1
							{
								// Adds contents to an instance of the Tranport object with due time is multiple '2','3','4'..... Also adds the image from if statement block above
								listOfStop.Add(new Transport { StopID = obj.Stopid, Route = ssize[0 + (k * ObjectRetrieval)], ArrivalTime = ssize[1 + (k * ObjectRetrieval)], Duetime = ssize[2 + (k * ObjectRetrieval)] + " Minutes", Destination = ssize[3 + (k * ObjectRetrieval)], ImageOperator = imageOperator });
							}
						}

						// If obj is empty (No trains found)
						if (obj.Numberofresults == 0)
						{
							// Increment counter
							StatusCounter++;
						}

					}
					catch (Exception)  // if any error is encountered
					{
						// Increment counter
						StatusCounter++;
					}

					// If all the codes return no data then this code will run
					if (StationID.Length <= StatusCounter)
					{
						// Set image to be a no bus logo
						imageOperator = "https://scontent-dub4-1.xx.fbcdn.net/v/t1.0-9/29025830_1727100117346019_1771119452012675072_n.jpg?oh=fd2dad187d5fdfe393123ae90fde4f21&oe=5B30FAAD";

						// Uses Transport object to add the words 'No trains operating' to inform the user that none of the verified codes they entered have trains due
						// This happens mostly when trains stop running past 11:59PM
						listOfStop.Add(new Transport { Route = "No", Destination = "trains", Duetime = "operating", ImageOperator = imageOperator });
					}

					// Set up grouping
					var resultCVS = from act in listOfStop group act by act.StopID into grp orderby grp.Key select grp;
					cvsActivities.Source = resultCVS;
				}
			}
			catch (FileNotFoundException) // If no file was not found means the user has yet to enter a valid train code
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
