using MapBuilderWpf.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MapBuilderWpf.Windows
{
	/// <summary>
	/// Interaction logic for newMapWindow.xaml
	/// </summary>
	public partial class newMapWindow : Window
	{
		private int mapWidth = 0;
		private int mapHeight = 0;
		private newMapControlData newMapData;
		private errorMessageData errorData;
		private MainWindow mainWindow;

		public newMapWindow(MainWindow parentWindow)
		{
			InitializeComponent();

			newMapData = new newMapControlData();
			newMapControls.DataContext = newMapData;
			mainWindow = parentWindow;

			errorData = new errorMessageData();
			errorData.errorMessage = "";
			errorGrid.DataContext = errorData;
		}

		/// <summary>
		/// Generate a new map if the width and height from the controls is valid.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void newMapButtonClick(object sender, RoutedEventArgs e)
		{
			//parse width and height
			bool success = int.TryParse(newMapData.widthTextBox, out mapWidth);
			if (success)
				success = int.TryParse(newMapData.heightTextBox, out mapHeight);
			else
			{
				errorData.errorMessage = "Invalid Map Width";
				return;
			}

			if (success)
			{
				mainWindow.mainAppPage.createNewMap(mapWidth, mapHeight);
				errorData.errorMessage = "";
				this.Close();
			}
			else
			{
				errorData.errorMessage = "Invalid Map Height";
				return;
			}
		}
	}

	public class newMapControlData
	{
		public string widthTextBox { get; set; }
		public string heightTextBox { get; set; }
	}
}
