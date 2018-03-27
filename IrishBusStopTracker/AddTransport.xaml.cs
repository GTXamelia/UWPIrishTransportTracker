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

			Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
			Windows.Storage.StorageFile fileToSave = null;

			String fileName = "BusIDs.txt";

			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri("https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=" + textBoxAdd.Text + "&format=json");
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			HttpResponseMessage response = client.GetAsync("https://data.dublinked.ie/cgi-bin/rtpi/realtimebusinformation?stopid=" + textBoxAdd.Text + "&format=json").Result;
			var result = response.Content.ReadAsStringAsync().Result;

			var obj = JsonConvert.DeserializeObject<RootObject>(result);

			//Debug.WriteLine(obj.Errorcode);

			if (obj.Errorcode.Contains("0"))
			{
				try
				{
					var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
					fileToSave = await storageFolder.GetFileAsync(fileName);

					string text = await Windows.Storage.FileIO.ReadTextAsync(fileToSave);

					if (!text.Contains(textBoxAdd.Text))
					{
						await Windows.Storage.FileIO.AppendTextAsync(fileToSave, textBoxAdd.Text + Environment.NewLine);
					}
					else
					{
						errorLabel.Text = "Code \"" + textBoxAdd.Text + "\"is already saved";
					}
				}
				catch (FileNotFoundException)
				{
					fileToSave = await storageFolder.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.ReplaceExisting);

					await Windows.Storage.FileIO.AppendTextAsync(fileToSave, textBoxAdd.Text + Environment.NewLine);

					Debug.WriteLine("-====::Success::====- \nError Code: " + obj.Errorcode + "\nError Message: " + obj.Errormessage + "\nCode " + textBoxAdd.Text + " is valid");
					Debug.WriteLine("Path: " + fileToSave.Path);
				}
			}
			else
			{
				Debug.WriteLine("-====::FAILURE::====- \nError Code: " + obj.Errorcode + "\nError Message: " + obj.Errormessage + "\nCode " + textBoxAdd.Text + " is not valid");
			}

			// Clears textbox after button code runs
			textBoxAdd.Text = String.Empty;
		}
	}
}
