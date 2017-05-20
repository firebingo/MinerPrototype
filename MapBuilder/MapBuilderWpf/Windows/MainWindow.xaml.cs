//system
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
//map builder wpf
using MapBuilderWpf.Pages;
using MapBuilderWpf.Windows;
using System.Windows.Input;
using MapBuilderWpf.Helpers;

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
		private BuildingGhostLayer _buildingGhostLayer;
		public BuildingGhostLayer buildingGhostLayer
		{
			get
			{
				if (_buildingGhostLayer == null)
				{
					_buildingGhostLayer = buildingGhost.Content as BuildingGhostLayer;
				}
				return _buildingGhostLayer;
			}
			private set
			{
				_buildingGhostLayer = value;
			}
		}
		private bool ignoreUnchecked;
		public bool hasMap;

		public MainWindow()
		{
			currentView = viewsEnum.terrain;
			InitializeComponent();

			viewItems = new List<MenuItem>();
			viewItems.Add(terrainView);
			viewItems.Add(oreView);
			viewItems.Add(crystalView);
			viewItems.Add(specialView);
			viewItems.Add(buildingView);
			viewItems[(int)currentView].IsChecked = true;
			mainAppPage = mainPage.Content as MainPage;
			buildingGhostLayer = buildingGhost.Content as BuildingGhostLayer;
			ignoreUnchecked = false;
			this.Closed += delegate
			{
				App.Current.Shutdown();
			};
		}

		private void MainOnMouseMove(object sender, MouseEventArgs e)
		{
			EventHelper.dynamicMessage(this, new { position = e.GetPosition(null) }, "mainMouseMove");
		}

		private void viewChecked(object sender, RoutedEventArgs e)
		{
			if(mainAppPage == null)
				mainAppPage = mainPage.Content as MainPage;
			if(buildingGhostLayer == null)
				buildingGhostLayer = buildingGhost.Content as BuildingGhostLayer;

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
				else if (check == viewItems[(int)viewsEnum.building])
				{
					currentView = viewsEnum.building;
				}
				if (mainAppPage != null)
					mainAppPage.changeCurrentView(currentView, hasMap);
				if(buildingGhostLayer != null)
					buildingGhostLayer.changeCurrentView(currentView, hasMap);
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
