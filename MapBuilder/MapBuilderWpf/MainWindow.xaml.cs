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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MapBuilderLibWindows;
using MapEnums;
using System.Diagnostics;
using System.ComponentModel;
using MapBuilderWpf.Pages;

namespace MapBuilderWpf
{
	public enum viewsEnum
	{
		terrain,
		ore,
		crystal
	}

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public viewsEnum currentView;
		public List<MenuItem> viewItems;
		private MainPage mainAppPage;
		private bool ignoreUnchecked;

		public MainWindow()
		{
			currentView = viewsEnum.terrain;
			InitializeComponent();

			viewItems = new List<MenuItem>();
			viewItems.Add(terrainView);
			viewItems.Add(oreView);
			viewItems.Add(crystalView);
			viewItems[(int)currentView].IsChecked = true;
			mainAppPage = mainPage.Content as MainPage;
			ignoreUnchecked = false;
		}

		private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
		{
			//checkSizes((int)e.NewSize.Width, (int)e.NewSize.Height);
			//if (e.NewSize.Width != e.PreviousSize.Width)
			//{
			//	topMenuBar.Width = e.NewSize.Width - 8;
			//}
		}

		private void viewChecked(object sender, RoutedEventArgs e)
		{
			if(mainAppPage == null)
			{
				mainAppPage = mainPage.Content as MainPage;
			}
			var check = sender as MenuItem;
			if (check != null)
			{
				clearChecks(check);
				if (check == viewItems[(int)viewsEnum.terrain])
				{
					currentView = viewsEnum.terrain;
				}
				else if (check == viewItems[(int)viewsEnum.ore])
				{
					currentView = viewsEnum.ore;
				}
				else if (check == viewItems[(int)viewsEnum.crystal])
				{
					currentView = viewsEnum.crystal;
				}
				if (mainAppPage != null)
					mainAppPage.changeCurrentView(currentView);
			}
		}

		private void viewUnchecked(object sender, RoutedEventArgs e)
		{
			if (!ignoreUnchecked)
			{
				var check = sender as MenuItem;
				if (check != null)
				{
					foreach (var item in viewItems)
					{
						if (item == check && !item.IsChecked)
							item.IsChecked = true;
					}
				}
			}
		}

		private void clearChecks(MenuItem ignoreCheck)
		{
			ignoreUnchecked = true;
			foreach (var item in viewItems)
			{
				if (item != ignoreCheck)
					item.IsChecked = false;
			}
			ignoreUnchecked = false;
		}
	}
}
