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

		private async void Submit(object sender, RoutedEventArgs e)
		{

			Windows.Storage.StorageFolder storageFolder;
			Windows.Storage.StorageFile fileToSave = null;

			String fileName = "BusIDs.txt";

			Debug.WriteLine("Test: " + fileName);

			try
			{
				await FileIO.AppendTextAsync(fileToSave, "text content");
			}
			catch (NullReferenceException)
			{
				storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
				fileToSave = await storageFolder.CreateFileAsync("sample.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);

				await FileIO.AppendTextAsync(fileToSave, "text content");
			}
			
			await FileIO.AppendTextAsync(fileToSave, "text content");

			string text = await Windows.Storage.FileIO.ReadTextAsync(fileToSave);

			Debug.WriteLine("Test: " + fileToSave.Path);
		}
	}
}
