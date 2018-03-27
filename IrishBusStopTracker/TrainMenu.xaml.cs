using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        public TrainMenu()
        {
            this.InitializeComponent();

			String url = "https://scontent-dub4-1.xx.fbcdn.net/v/t1.0-9/29025830_1727100117346019_1771119452012675072_n.jpg?oh=fd2dad187d5fdfe393123ae90fde4f21&oe=5B30FAAD";
		}
    }
}
