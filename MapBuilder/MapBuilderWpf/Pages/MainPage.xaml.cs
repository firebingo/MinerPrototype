//system
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
//map builder wpf
using MapBuilderWpf.Models;
using MapBuilderWpf.Helpers;
//map builder lib
using MapBuilderLibCore;
using MapEnums;

namespace MapBuilderWpf.Pages
{
	/// <summary>
	/// Interaction logic for MainPage.xaml
	/// </summary>
	public partial class MainPage : Page
	{
		public MapBuilderApp appMap { get; private set; }
		private int mapWidth = 0;
		private int mapHeight = 0;
		private List<ColumnDefinition> gridColumns;
		private List<RowDefinition> gridRows;
		private bottomControlData bottomData;
		private rightControlData rightData;
		private leftControlData leftData;
		private errorMessageData errorData;
		public Grid mapGrid { get; private set; }
		public viewsEnum currentView;
		public orientation currentOrientation;
		const int oreColorUpperBound = 5;
		const int crystalColorUpperBound = 5;
		Color resourceColorUpper = Colors.Red;
		Color resourceColorLower = Colors.Green;
		Color mobSpawnColor = Colors.DarkRed;
		Color cRechargeColor = Colors.Purple;

		public MainPage()
		{
			InitializeComponent();

			bottomData = new bottomControlData();
			bottomControls.DataContext = bottomData;

			rightData = new rightControlData();
			rightData.oreCount = "3";
			rightData.crystalCount = "0";
			rightData.mobSpawn = false;
			rightData.terrainType = (int)terrainType.empty;
			rightData.showMapControls = Visibility.Hidden;
			rightData.showBuildingControls = Visibility.Hidden;
			rightControls.DataContext = rightData;

			leftData = new leftControlData();
			leftData.oxygenCount = "300";
			leftData.oxygenTick = "1.0";
			leftData.mapName = "Map1";
			leftData.showLeftControls = Visibility.Hidden;
			leftControls.DataContext = leftData;

			errorData = new errorMessageData();
			errorData.errorMessage = "";
			errorGrid.DataContext = errorData;

			var terrainNames = Enum.GetNames(typeof(terrainType));
			foreach (var name in terrainNames)
			{
				var nameItem = new ComboBoxItem();
				nameItem.Content = name;
				terrainComboBox.Items.Add(nameItem);
			}

			buildingComboBox.Items.Add("None");
			foreach (var name in PredefBuildings.preBuildings)
			{
				var nameItem = new ComboBoxItem();
				nameItem.Content = name.Key;
				buildingComboBox.Items.Add(nameItem);
			}

			GridHelper.checkSizes((int)this.Width, (int)this.Height, mapGrid, mapWidth, mapHeight, gridColumns, gridRows);
			currentView = viewsEnum.terrain;
			currentOrientation = orientation.north;
		}

		private void PageSizeChanged(object sender, SizeChangedEventArgs e)
		{
			GridHelper.checkSizes((int)e.NewSize.Width, (int)e.NewSize.Height, mapGrid, mapWidth, mapHeight, gridColumns, gridRows);
		}

		/// <summary>
		/// Updates the error messages on the page.
		/// </summary>
		/// <param name="message"></param>
		public void updateErrorMessage(string message)
		{
			if (message != null && message != "")
				errorData.errorMessage = message;
		}

		/// <summary>
		/// Runs the process of creating a new map
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void createNewMap(int width, int height)
		{
			mapWidth = width;
			mapHeight = height;
			EventHelper.dynamicMessage(this, new { width = width, height = height }, "mapDimensions");
			appMap = new MapBuilderApp();
			appMap.initializeMap(mapWidth, mapHeight);
			appMap.buildMap.mapHeader.mapName = leftData.mapName;
			appMap.buildMap.mapHeader.oxygenCount = float.Parse(leftData.oxygenCount);
			appMap.buildMap.mapHeader.oxygenRate = float.Parse(leftData.oxygenTick);
			BuildMapGrid();
			leftData.showLeftControls = Visibility.Visible;
			errorData.errorMessage = "";
		}

		/// <summary>
		/// When the mouse moves on the grid.
		/// If the mouse is pressed update the tile that the mouse is over with the data
		///  in the UI controls.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GridOnMouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				if (mapGrid?.Children != null && mapGrid.Children.Count > 0)
				{
					foreach (var child in mapGrid.Children)
					{
						var co = child as Control;
						if (co != null)
						{
							if (co.IsMouseOver)
							{
								gridTileData gtd = co.DataContext as gridTileData;
								if (gtd != null)
								{
									changeGridTile(gtd);
								}
							}
						}
					}
				}
			}
		}

		private void MainOnMouseMove(object sender, MouseEventArgs e)
		{
			EventHelper.dynamicMessage(this, new { position = e.GetPosition(null) }, "mainMouseMove");
		}

		/// <summary>
		/// When a grid tile is clicked on update its data.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GridOnMouseLeftUp(object sender, MouseButtonEventArgs e)
		{
			if (mapGrid?.Children != null && mapGrid.Children.Count > 0)
			{
				foreach (var child in mapGrid.Children)
				{
					var co = child as Control;
					if (co != null)
					{
						if (co.IsMouseOver)
						{
							gridTileData gtd = co.DataContext as gridTileData;
							if (gtd != null)
							{
								changeGridTile(gtd);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Updates the tile on the map and the grid.
		/// </summary>
		/// <param name="data"></param>
		private void changeGridTile(gridTileData data)
		{
			if (currentView != viewsEnum.building)
			{
				if (Enum.IsDefined(typeof(terrainType), rightData.terrainType))
				{
					var oreCount = 0;
					var crystalCount = 0;
					//parse the ore and crystal counts
					var parsed = int.TryParse(rightData.oreCount, out oreCount);
					if (parsed)
						parsed = int.TryParse(rightData.crystalCount, out crystalCount);
					else
					{
						errorData.errorMessage = "Invalid Ore Count";
						return;
					}
					if (parsed)
					{
						//try to modify the tile on the map. if it succeeds update the tile on the grid.
						if (appMap.buildMap.modifyTile(data.x, data.y, (terrainType)rightData.terrainType, oreCount, crystalCount, rightData.mobSpawn, rightData.crystalRecharge))
						{
							var tile = appMap.buildMap.mapTiles[data.x, data.y];
							if (currentView == viewsEnum.terrain)
							{
								data.Background = new SolidColorBrush(GridHelper.terrainTileColors[(terrainType)rightData.terrainType]);
							}
							if (currentView == viewsEnum.special)
							{
								if (tile != null)
								{
									if (tile.tileType > terrainType.roof && tile.tileType < terrainType.water)
									{
										if (tile.mobSpawn)
											data.Background = new SolidColorBrush(mobSpawnColor);
										else if (tile.crystalRecharge)
											data.Background = new SolidColorBrush(cRechargeColor);
										else
											data.Background = new SolidColorBrush(Colors.White);
									}
									else
										data.Background = new SolidColorBrush(Colors.White);
								}
							}
							if (currentView == viewsEnum.ore)
							{
								if (tile != null)
								{
									if (tile.tileType > terrainType.roof && tile.tileType < terrainType.water)
										data.Background = new SolidColorBrush(HelperFunctions.lerpColors(resourceColorUpper, resourceColorLower, ((float)tile.oreCount / (float)oreColorUpperBound)));
									else
										data.Background = new SolidColorBrush(Colors.White);
								}
							}
							if (currentView == viewsEnum.crystal)
							{
								if (tile != null)
								{
									if (tile.tileType > terrainType.roof && tile.tileType < terrainType.water)
										data.Background = new SolidColorBrush(HelperFunctions.lerpColors(resourceColorUpper, resourceColorLower, ((float)tile.crystalCount / (float)crystalColorUpperBound)));
									else
										data.Background = new SolidColorBrush(Colors.White);
								}
							}
							data.oreCount = oreCount;
							data.crystalCount = crystalCount;
							data.mobSpawn = rightData.mobSpawn;
							data.crystalRecharge = rightData.crystalRecharge;
							data.terrain = (terrainType)rightData.terrainType;
							errorData.errorMessage = "";
						}
						else
						{
							errorData.errorMessage = "Modifying Tile Failed";
						}
					}
					else
					{
						errorData.errorMessage = "Invalid Crystal Count";
						return;
					}
				}
			}
			else
			{
				if (buildingComboBox.Text != "None")
				{
					var newBuilding = new BuildingModel(buildingComboBox.Text, currentOrientation, new Vector2<int>(data.x, data.y));
					appMap.buildMap.placeBuilding(newBuilding);
				}
				else
				{
					appMap.buildMap.removeBuilding(data.buildingGuid);
				}
				//resync the map since there can be multiple tiles changed in various forms
				BuildMapGrid();
			}
		}

		/// <summary>
		/// Builds the grid for the created map.
		/// </summary>
		private void BuildMapGrid()
		{
			//remove the old grid if there is one
			if (mapParentGrid.Children.Count > 0)
				mapParentGrid.Children.Clear();

			mapGrid = new Grid();

			//clear the grid rows and columns
			if (gridColumns != null)
				gridColumns.Clear();
			else
				gridColumns = new List<ColumnDefinition>();
			if (gridRows != null)
				gridRows.Clear();
			else
				gridRows = new List<RowDefinition>();

			//create the ui grid for the map
			mapGrid.Background = new SolidColorBrush(Colors.LightSteelBlue);
			GridHelper.calculateGridSizes(mapGrid, mapWidth, mapHeight);
			mapGrid.HorizontalAlignment = HorizontalAlignment.Center;
			mapGrid.VerticalAlignment = VerticalAlignment.Center;
			double colWidth = mapGrid.Width / mapWidth;
			double rowHeight = mapGrid.Height / mapHeight;
			for (int x = 0; x < mapWidth; ++x)
			{
				ColumnDefinition column = new ColumnDefinition();
				gridColumns.Add(column);
			}
			foreach (var col in gridColumns)
			{
				col.Width = new GridLength(colWidth);
				mapGrid.ColumnDefinitions.Add(col);
			}
			for (int y = 0; y < mapHeight; ++y)
			{
				RowDefinition row = new RowDefinition();
				gridRows.Add(row);
			}
			foreach (var row in gridRows)
			{
				row.Height = new GridLength(rowHeight);
				mapGrid.RowDefinitions.Add(row);
			}

			var tileTemplate = FindResource("GridTileTemplate") as ControlTemplate;
			//create tiles on grid
			for (int x = 0; x < mapWidth; ++x)
			{
				for (int y = 0; y < mapHeight; ++y)
				{
					if (tileTemplate != null)
					{
						gridTileData tileData = new gridTileData();
						Control gridTile = new Control();
						gridTile.Template = tileTemplate;
						var tileType = appMap.buildMap.mapTiles[x, y].tileType;
						tileData.Background = new SolidColorBrush(Colors.White);
						tileData.Height = rowHeight;
						tileData.Width = colWidth;
						tileData.x = x;
						tileData.y = y;
						tileData.oreCount = appMap.buildMap.mapTiles[x, y].oreCount;
						tileData.crystalCount = appMap.buildMap.mapTiles[x, y].crystalCount;
						tileData.mobSpawn = appMap.buildMap.mapTiles[x, y].mobSpawn;
						tileData.crystalRecharge = appMap.buildMap.mapTiles[x, y].crystalRecharge;
						tileData.terrain = tileType;
						tileData.hasBuilding = appMap.buildMap.mapTiles[x, y].building != default(Guid);
						tileData.buildingSection = appMap.buildMap.mapTiles[x, y].buildingSection;
						tileData.buildingGuid = appMap.buildMap.mapTiles[x, y].building;
						tileData.showCrystal = Visibility.Hidden;
						tileData.showOre = Visibility.Hidden;
						tileData.showSpecial = Visibility.Hidden;
						if (currentView == viewsEnum.ore)
							tileData.showOre = Visibility.Visible;
						else if (currentView == viewsEnum.crystal)
							tileData.showCrystal = Visibility.Visible;
						GridHelper.currentFontSize = (double)colWidth / (double)2;
						tileData.fontSize = GridHelper.currentFontSize > 8 ? GridHelper.currentFontSize : 0.01;
						gridTile.DataContext = tileData;
						Grid.SetRow(gridTile, y);
						Grid.SetColumn(gridTile, x);
						mapGrid.Children.Add(gridTile);
					}
				}
			}

			mapParentGrid.Children.Add(mapGrid);
			changeCurrentView(currentView, true);
		}

		/// <summary>
		/// Rotates the selected building clockwise.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void rotateBuilding(object sender, RoutedEventArgs e)
		{
			switch(currentOrientation)
			{
				default:
				case orientation.north:
					currentOrientation = orientation.east;
					break;
				case orientation.east:
					currentOrientation = orientation.south;
					break;
				case orientation.south:
					currentOrientation = orientation.west;
					break;
				case orientation.west:
					currentOrientation = orientation.north;
					break;
			}

			EventHelper.dynamicMessage(this, new { orientation = currentOrientation }, "changeOrientation");
		}

		/// <summary>
		/// Changes the current view of the grid.
		/// </summary>
		/// <param name="view"></param>
		public void changeCurrentView(viewsEnum view, bool hasMap)
		{
			currentView = view;
			if (mapGrid != null && mapGrid.Children.Count > 0)
			{
				switch (currentView)
				{
					default:
					case viewsEnum.terrain:
						rightData.showMapControls = Visibility.Visible;
						rightData.showBuildingControls = Visibility.Hidden;
						foreach (var child in mapGrid.Children)
						{
							var ct = child as Control;
							if (ct != null)
							{
								gridTileData gbd = ct.DataContext as gridTileData;
								if (gbd != null)
								{
									if (gbd.hasBuilding)
									{
										var bColor = GridHelper.buildingTileColors[gbd.buildingSection];
										gbd.Background = new SolidColorBrush(HelperFunctions.normalBlendColor(GridHelper.terrainTileColors[gbd.terrain], Color.FromScRgb(0.4f, bColor.ScR, bColor.ScG, bColor.ScB)));
									}
									else
										gbd.Background = new SolidColorBrush(GridHelper.terrainTileColors[gbd.terrain]);
									gbd.showCrystal = Visibility.Hidden;
									gbd.showOre = Visibility.Hidden;
									gbd.showSpecial = Visibility.Hidden;
								}
							}
						}
						break;
					case viewsEnum.ore:
						rightData.showMapControls = Visibility.Visible;
						rightData.showBuildingControls = Visibility.Hidden;
						foreach (var child in mapGrid.Children)
						{
							var ct = child as Control;
							if (ct != null)
							{
								gridTileData gbd = ct.DataContext as gridTileData;
								if (gbd != null)
								{
									gbd.showCrystal = Visibility.Hidden;
									gbd.showOre = Visibility.Visible;
									gbd.showSpecial = Visibility.Hidden;
									if (gbd.terrain > terrainType.roof && gbd.terrain < terrainType.water)
										gbd.Background = new SolidColorBrush(HelperFunctions.lerpColors(resourceColorUpper, resourceColorLower, ((float)gbd.oreCount / (float)oreColorUpperBound)));
									else
										gbd.Background = new SolidColorBrush(Colors.White);
								}
							}
						}
						break;
					case viewsEnum.crystal:
						rightData.showMapControls = Visibility.Visible;
						rightData.showBuildingControls = Visibility.Hidden;
						foreach (var child in mapGrid.Children)
						{
							var ct = child as Control;
							if (ct != null)
							{
								gridTileData gbd = ct.DataContext as gridTileData;
								if (gbd != null)
								{
									gbd.showCrystal = Visibility.Visible;
									gbd.showOre = Visibility.Hidden;
									gbd.showSpecial = Visibility.Hidden;
									if (gbd.terrain > terrainType.roof && gbd.terrain < terrainType.water)
										gbd.Background = new SolidColorBrush(HelperFunctions.lerpColors(resourceColorUpper, resourceColorLower, ((float)gbd.crystalCount / (float)crystalColorUpperBound)));
									else
										gbd.Background = new SolidColorBrush(Colors.White);
								}
							}
						}
						break;
					case viewsEnum.special:
						rightData.showMapControls = Visibility.Visible;
						rightData.showBuildingControls = Visibility.Hidden;
						foreach (var child in mapGrid.Children)
						{
							var ct = child as Control;
							if (ct != null)
							{
								gridTileData gbd = ct.DataContext as gridTileData;
								if (gbd != null)
								{
									gbd.showCrystal = Visibility.Hidden;
									gbd.showOre = Visibility.Hidden;
									gbd.showSpecial = Visibility.Visible;
									if (gbd.terrain > terrainType.roof && gbd.terrain < terrainType.water)
									{
										if (gbd.mobSpawn)
											gbd.Background = new SolidColorBrush(mobSpawnColor);
										else if (gbd.crystalRecharge)
											gbd.Background = new SolidColorBrush(cRechargeColor);
										else
											gbd.Background = new SolidColorBrush(Colors.White);
									}
									else
										gbd.Background = new SolidColorBrush(Colors.White);
								}
							}
						}
						break;
					case viewsEnum.building:
						rightData.showMapControls = Visibility.Hidden;
						rightData.showBuildingControls = Visibility.Visible;
						foreach (var child in mapGrid.Children)
						{
							var ct = child as Control;
							if (ct != null)
							{
								gridTileData gbd = ct.DataContext as gridTileData;
								if (gbd != null)
								{
									if (gbd.hasBuilding)
									{
										gbd.Background = new SolidColorBrush(GridHelper.buildingTileColors[gbd.buildingSection]);
										gbd.showCrystal = Visibility.Hidden;
										gbd.showOre = Visibility.Hidden;
										gbd.showSpecial = Visibility.Hidden;
									}
									else
									{
										gbd.Background = new SolidColorBrush(GridHelper.terrainTileColors[gbd.terrain]);
										gbd.showCrystal = Visibility.Hidden;
										gbd.showOre = Visibility.Hidden;
										gbd.showSpecial = Visibility.Hidden;
									}
								}
							}
						}
						break;
				}
			}
		}

		/// <summary>
		/// Used to confirm text in the ore input box
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OreCountKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				var textBox = sender as TextBox;
				if (textBox != null)
				{
					var cont = rightControls.DataContext as rightControlData;
					if (cont != null)
						cont.oreCount = textBox.Text;
					Keyboard.ClearFocus();
				}
			}
		}

		/// <summary>
		/// Used to confirm text in the crystal input box
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CrystalCountKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				var textBox = sender as TextBox;
				if (textBox != null)
				{
					var cont = rightControls.DataContext as rightControlData;
					if (cont != null)
						cont.crystalCount = textBox.Text;
					Keyboard.ClearFocus();
				}
			}
		}

		/// <summary>
		/// When the building selected in the combo box is changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buildingChanged(object sender, SelectionChangedEventArgs e)
		{
			var item = e.AddedItems[0] as ComboBoxItem;
			if (item?.Content != null)
				EventHelper.dynamicMessage(this, new { building = item?.Content != null ? item.Content : "None" }, "changeBuilding");
			else
				EventHelper.dynamicMessage(this, new { building = "None" }, "changeBuilding");
		}

		/// <summary>
		/// Update oxygen count when field changes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void oxygenCountChanged(object sender, TextChangedEventArgs e)
		{
			if (appMap != null)
			{
				errorData.errorMessage = "";
				var value = 0.0f;
				if (float.TryParse((sender as TextBox).Text, out value))
					appMap.buildMap.mapHeader.oxygenCount = value;
				else
					errorData.errorMessage = "Invalid Oxygen";
			}
		}

		/// <summary>
		/// Update oxygen per second when field changes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void oxygenSecondChanged(object sender, TextChangedEventArgs e)
		{
			if (appMap != null)
			{
				errorData.errorMessage = "";
				var value = 0.0f;
				if (float.TryParse((sender as TextBox).Text, out value))
					appMap.buildMap.mapHeader.oxygenRate = value;
				else
					errorData.errorMessage = "Invalid Oxygen Per Second";
			}
		}

		/// <summary>
		/// Update the map name when the field changes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mapNameChanged(object sender, TextChangedEventArgs e)
		{
			if (appMap != null)
			{
				errorData.errorMessage = "";
				var name = (sender as TextBox).Text;
				if (name != null && name.Length > 0)
					appMap.buildMap.mapHeader.mapName = name;
				else
					errorData.errorMessage = "Invalid Map Name";
			}
		}
	}
}
