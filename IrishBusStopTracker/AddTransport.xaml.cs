using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

			try
			{
				var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);

				fileToSave = await storageFolder.GetFileAsync(fileName);

				await Windows.Storage.FileIO.AppendTextAsync(fileToSave, textBoxAdd.Text + Environment.NewLine);
			}
			catch (FileNotFoundException)
			{
				fileToSave = await storageFolder.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.ReplaceExisting);

				await Windows.Storage.FileIO.AppendTextAsync(fileToSave, textBoxAdd.Text + Environment.NewLine);
			}
		}
	}
}
