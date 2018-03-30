using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace IrishBusStopTracker
{
	/*	
	 *	This class is responsible for adding transport types supported by the TFI API
	 *	This class allows the adding of Trains/Luas/Buses
	 *	This class sorts codes by those types of transports
	 */

	public sealed partial class AddTransport : Page
	{
		public AddTransport()
		{
			this.InitializeComponent();
		}

		// Function that handles the Main Menu button press
		// Transfers the user to the main menu when pressed
		private void Main_Menu(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(MainMenu));
		}


		private async void Submit(object sender, RoutedEventArgs e)
		{
			// Clears error message on run
			errorLabel.Text = String.Empty;

			// File path settings
			Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
			Windows.Storage.StorageFile fileToSave = null;

			// Pre-defined variables
			String busFile = "BusIDs.txt";
			String trainFile = "TrainIDs.txt";
			String luasFile = "LuasIDs.txt";
			int ObjectRetrieval = 4;

			// Connects to the url and downloads all the JSON text
			// Alters the url by using the code the user
			// Stores JSON data in the BusStatusObject.cs
			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri("https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=" + textBoxAdd.Text + "&format=json");
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			HttpResponseMessage response = client.GetAsync("https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=" + textBoxAdd.Text + "&format=json").Result;
			var result = response.Content.ReadAsStringAsync().Result;
			var obj = JsonConvert.DeserializeObject<RootObject>(result);

			// If the Errorcode equals '0' then the url is either not valid or no transports types are due
			if (obj.Errorcode.Contains("0"))
			{
				try
				{
					// Gets obj var instance of BustStatusObject.cs by it's ToString Method and splits values into array
					string[] ssize = (obj.ToString()).Split(new char[0]); 

					// If 5th value of the array equals 'X' or 'BE' or 'bac' then the transport type is a bus
					if (ssize[ObjectRetrieval].Equals("X") || ssize[ObjectRetrieval].Equals("BE") || ssize[ObjectRetrieval].Equals("bac"))
					{
						// Sets file path
						var BusfileLocal = await ApplicationData.Current.LocalFolder.GetFileAsync(busFile);
						fileToSave = await storageFolder.GetFileAsync(busFile);

						// Get file contents
						string busFileContents = await Windows.Storage.FileIO.ReadTextAsync(fileToSave); 

						if (!busFileContents.Contains(textBoxAdd.Text)) // busFileContents is parsed to see if code is not present
						{
							// Writes code to file
							await Windows.Storage.FileIO.AppendTextAsync(fileToSave, textBoxAdd.Text + Environment.NewLine);

							// Moves user to BusTransport page
							this.Frame.Navigate(typeof(BusTransport));
						}
						else // Else if the code is already entered the user will be told that it already exists
						{
							// Informs user code is already present
							errorLabel.Text = "ID \"" + textBoxAdd.Text + "\" has already been entered";
						}
					}
					else if (ssize[ObjectRetrieval].Contains("ir")) // Else if 5th value of the array equals 'ir' then the transport type is a Train
					{
						// Sets file path
						var TrainfileLocal = await ApplicationData.Current.LocalFolder.GetFileAsync(trainFile);
						fileToSave = await storageFolder.GetFileAsync(trainFile);

						// Get file contents
						string trainFileContents = await Windows.Storage.FileIO.ReadTextAsync(fileToSave);

						if (!trainFileContents.Contains(textBoxAdd.Text)) // trainFileContents is parsed to see if code is not present
						{
							// Writes code to file
							await Windows.Storage.FileIO.AppendTextAsync(fileToSave, textBoxAdd.Text + Environment.NewLine);

							// Moves user to TrainMenu page
							this.Frame.Navigate(typeof(TrainMenu));
						}
						else // Else if the code is already entered the user will be told that it already exists
						{
							// Informs user code is already present
							errorLabel.Text = "ID \"" + textBoxAdd.Text + "\" has already been entered";
						}
					}
					else if (ssize[ObjectRetrieval].Contains("LUAS")) // Else if 5th value of the array equals 'LUAS' then the transport type is a Luas line
					{
						// Sets file path
						var LuasfileLocal = await ApplicationData.Current.LocalFolder.GetFileAsync(luasFile);
						fileToSave = await storageFolder.GetFileAsync(luasFile);

						// Get file contents
						string luasFileContents = await Windows.Storage.FileIO.ReadTextAsync(fileToSave);

						if (!luasFileContents.Contains(textBoxAdd.Text)) // luasFileContents is parsed to see if code is not present
						{
							// Writes code to file
							await Windows.Storage.FileIO.AppendTextAsync(fileToSave, textBoxAdd.Text + Environment.NewLine);

							// Moves user to LuasMenu page
							this.Frame.Navigate(typeof(LuasMenu));
						}
						else
						{
							// Informs user code is already present
							errorLabel.Text = "ID \"" + textBoxAdd.Text + "\" has already been entered";
						}
					}
					else // If none if the above outter if statements is true then no JSON data was returned
					{
						// Informs user that no data was returned
						errorLabel.Text = "ID \"" + textBoxAdd.Text + "\" has encountered an error";
					}
				}
				catch (FileNotFoundException) // If file reading encounters a 'FileNotFoundException' it means no file is present
				{
					// Gets obj var instance of BustStatusObject.cs by it's ToString Method and splits values into array
					string[] ssize = (obj.ToString()).Split(new char[0]);

					// If 5th value of the array equals 'X' or 'BE' or 'bac' then the transport type is a bus
					if (ssize[ObjectRetrieval].Equals("X") || ssize[ObjectRetrieval].Equals("BE") || ssize[ObjectRetrieval].Equals("bac"))
					{
						// Creates folder for buses
						fileToSave = await storageFolder.CreateFileAsync(busFile, Windows.Storage.CreationCollisionOption.ReplaceExisting);

						// Writes code to file
						await Windows.Storage.FileIO.AppendTextAsync(fileToSave, textBoxAdd.Text + Environment.NewLine);

						// Moves user to TrainMenu page
						this.Frame.Navigate(typeof(BusTransport));
					}
					else if (ssize[ObjectRetrieval].Contains("ir")) // Else if 5th value of the array equals 'ir' then the transport type is a Train
					{
						// Creates folder for trains
						fileToSave = await storageFolder.CreateFileAsync(trainFile, Windows.Storage.CreationCollisionOption.ReplaceExisting);

						// Writes code to file
						await Windows.Storage.FileIO.AppendTextAsync(fileToSave, textBoxAdd.Text + Environment.NewLine);

						// Moves user to TrainMenu page
						this.Frame.Navigate(typeof(TrainMenu));
					}
					else if (ssize[ObjectRetrieval].Contains("LUAS")) // Else if 5th value of the array equals 'LUAS' then the transport type is a Luas line
					{
						// Creates folder for trains
						fileToSave = await storageFolder.CreateFileAsync(luasFile, Windows.Storage.CreationCollisionOption.ReplaceExisting);

						// Writes code to file
						await Windows.Storage.FileIO.AppendTextAsync(fileToSave, textBoxAdd.Text + Environment.NewLine);

						// Moves user to TrainMenu page
						this.Frame.Navigate(typeof(LuasMenu));
					}
				}
			}
			else if (string.IsNullOrEmpty(textBoxAdd.Text)) // else if the user does not enter any data
			{
				// User is informed they must not leave it blank
				errorLabel.Text = "You must enter an ID to continue";
			}
			else if (textBoxAdd.Text.Contains(" "))// else if the user has any spaces in the code
			{
				// User is informed they must not have any spaces
				errorLabel.Text = "ID cannot contain any spaces";
			}
			else // this will run if no other statement is true
			{
				// User is informed the code didin't work
				errorLabel.Text = "ID \"" + textBoxAdd.Text + "\" is not a valid stopID.\nOr no transports are due at this time.\nPlease ensure code is correct,\nand try again later";
			}

			// Clears textbox after button code runs
			textBoxAdd.Text = String.Empty;
		}

		// Used when no codes have been entered by the user on other transport pages.
		// If no code has been entered they will be redirected to this function 
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			// Sets up navigated page
			base.OnNavigatedTo(e);

			// Gets error that the user encountered on other page
			var error = (Errors)e.Parameter;

			// Sets error message by using error from other page
			try
			{
				errorLabel.Text = error.ErrorCode;
			}
			catch (NullReferenceException)
			{
				errorLabel.Text = "";
			}
		}
	}
}
