using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MapBuilderWpf.Pages;
using MapBuilderWpf.Windows;

namespace MapBuilderWpf
{
	public enum viewsEnum
	{
		terrain,
		ore,
		crystal,
		special,
		building
	}

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public viewsEnum currentView;
		public List<MenuItem> viewItems;
		private MainPage _mainAppPage;
		public MainPage mainAppPage {
			get
			{
				if(_mainAppPage == null)
				{
					_mainAppPage = mainPage.Content as MainPage;
				}
				return _mainAppPage;
			}
			private set
			{
				_mainAppPage = value;
			}
		}
		private bool ignoreUnchecked;

		public MainWindow()
		{
			currentView = viewsEnum.terrain;
			InitializeComponent();

			viewItems = new List<MenuItem>();
			viewItems.Add(terrainView);
			viewItems.Add(oreView);
			viewItems.Add(crystalView);
			viewItems.Add(specialView);
			viewItems[(int)currentView].IsChecked = true;
			mainAppPage = mainPage.Content as MainPage;
			ignoreUnchecked = false;
			this.Closed += delegate
			{
				App.Current.Shutdown();
			};
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
				mainAppPage = mainPage.Content as MainPage;

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
				else if(check == viewItems[(int)viewsEnum.special])
				{
					currentView = viewsEnum.special;
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

		private void newMap(object sender, RoutedEventArgs e)
		{
			newMapWindow mapWindow = new newMapWindow(this);
			mapWindow.Show();
		}

		private void saveMap(object sender, RoutedEventArgs e)
		{
			if (mainAppPage == null)
				mainAppPage = mainPage.Content as MainPage;

			if (mainAppPage?.mapGrid != null)
			{
				var result = mainAppPage.appMap.saveMap();
				if (result)
				{
					mainAppPage.updateErrorMessage("");
					return;
				}
				else
					mainAppPage.updateErrorMessage("Error Saving Map");
			}
			else
			{
				mainAppPage.updateErrorMessage("Error Saving Map: No Map To Save");
			}
		}

		private void openMap(object sender, RoutedEventArgs e)
		{

		}

		private void importMap(object sender, RoutedEventArgs e)
		{

		}
	}
}
