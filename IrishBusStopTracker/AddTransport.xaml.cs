using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace IrishBusStopTracker
{
	public sealed partial class AddTransport : Page
	{
		public AddTransport()
		{
			this.InitializeComponent();
		}

		private void Main_Menu(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(MainMenu));
		}

		private async void Submit(object sender, RoutedEventArgs e)
		{
			// Clears error message on run
			errorLabel.Text = String.Empty;

			Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
			Windows.Storage.StorageFile fileToSave = null;

			String busFile = "BusIDs.txt";
			String trainFile = "TrainIDs.txt";
			String luasFile = "LuasIDs.txt";
			int ObjectRetrieval = 4;

			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri("https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=" + textBoxAdd.Text + "&format=json");
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			HttpResponseMessage response = client.GetAsync("https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=" + textBoxAdd.Text + "&format=json").Result;
			var result = response.Content.ReadAsStringAsync().Result;

			var obj = JsonConvert.DeserializeObject<RootObject>(result);

			if (obj.Errorcode.Contains("0"))
			{
				try
				{

					string[] ssize = (obj.ToString()).Split(new char[0]);

					if (ssize[ObjectRetrieval].Equals("X") || ssize[ObjectRetrieval].Equals("BE") || ssize[ObjectRetrieval].Equals("bac")) // Buses
					{
						var BusfileLocal = await ApplicationData.Current.LocalFolder.GetFileAsync(busFile);
						fileToSave = await storageFolder.GetFileAsync(busFile);

						string busFileContents = await Windows.Storage.FileIO.ReadTextAsync(fileToSave);

						if (!busFileContents.Contains(textBoxAdd.Text))
						{
							await Windows.Storage.FileIO.AppendTextAsync(fileToSave, textBoxAdd.Text + Environment.NewLine);

							this.Frame.Navigate(typeof(BusTransport));
						}
						else
						{
							errorLabel.Text = "ID \"" + textBoxAdd.Text + "\" has already been entered";
						}
					}
					else if (ssize[ObjectRetrieval].Contains("ir")) // Trains
					{
						var TrainfileLocal = await ApplicationData.Current.LocalFolder.GetFileAsync(trainFile);
						fileToSave = await storageFolder.GetFileAsync(trainFile);

						string trainFileContents = await Windows.Storage.FileIO.ReadTextAsync(fileToSave);

						if (!trainFileContents.Contains(textBoxAdd.Text))
						{
							await Windows.Storage.FileIO.AppendTextAsync(fileToSave, textBoxAdd.Text + Environment.NewLine);

							this.Frame.Navigate(typeof(TrainMenu));
						}
						else
						{
							errorLabel.Text = "ID \"" + textBoxAdd.Text + "\" has already been entered";
						}
					}
					else if (ssize[ObjectRetrieval].Contains("LUAS")) // Trains
					{
						var TrainfileLocal = await ApplicationData.Current.LocalFolder.GetFileAsync(luasFile);
						fileToSave = await storageFolder.GetFileAsync(luasFile);

						string luasFileContents = await Windows.Storage.FileIO.ReadTextAsync(fileToSave);

						if (!luasFileContents.Contains(textBoxAdd.Text))
						{
							await Windows.Storage.FileIO.AppendTextAsync(fileToSave, textBoxAdd.Text + Environment.NewLine);

							this.Frame.Navigate(typeof(LuasMenu));
						}
						else
						{
							errorLabel.Text = "ID \"" + textBoxAdd.Text + "\" has already been entered";
						}
					}
					else
					{

						errorLabel.Text = "ID \"" + textBoxAdd.Text + "\" has encountered an error";
					}
				}
				catch (FileNotFoundException)
				{
					string[] ssize = (obj.ToString()).Split(new char[0]);

					if (ssize[ObjectRetrieval].Equals("X") || ssize[ObjectRetrieval].Equals("BE") || ssize[ObjectRetrieval].Equals("bac")) // Buses
					{
						fileToSave = await storageFolder.CreateFileAsync(busFile, Windows.Storage.CreationCollisionOption.ReplaceExisting);

						await Windows.Storage.FileIO.AppendTextAsync(fileToSave, textBoxAdd.Text + Environment.NewLine);

						this.Frame.Navigate(typeof(BusTransport));
					}
					else if (ssize[ObjectRetrieval].Contains("ir")) // Trains
					{
						fileToSave = await storageFolder.CreateFileAsync(trainFile, Windows.Storage.CreationCollisionOption.ReplaceExisting);

						await Windows.Storage.FileIO.AppendTextAsync(fileToSave, textBoxAdd.Text + Environment.NewLine);

						this.Frame.Navigate(typeof(TrainMenu));
					}
					else if (ssize[ObjectRetrieval].Contains("LUAS")) // Trains
					{
						fileToSave = await storageFolder.CreateFileAsync(luasFile, Windows.Storage.CreationCollisionOption.ReplaceExisting);

						await Windows.Storage.FileIO.AppendTextAsync(fileToSave, textBoxAdd.Text + Environment.NewLine);

						this.Frame.Navigate(typeof(TrainMenu));
					}
				}
			}
			else if (string.IsNullOrEmpty(textBoxAdd.Text))
			{
				errorLabel.Text = "You must enter an ID to continue";
			}
			else if (textBoxAdd.Text.Contains(" "))
			{
				errorLabel.Text = "ID cannot contain any spaces";
			}
			else
			{
				errorLabel.Text = "ID \"" + textBoxAdd.Text + "\" is not a valid stopID";
			}

			// Clears textbox after button code runs
			textBoxAdd.Text = String.Empty;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			var error = (Errors)e.Parameter;

			try
			{
				errorLabel.Text = error.ErrorCode;
			}
			catch (NullReferenceException)
			{

			}
		}
	}
}
