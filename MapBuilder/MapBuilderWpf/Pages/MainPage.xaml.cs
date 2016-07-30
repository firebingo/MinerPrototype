﻿using MapEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MapBuilderWpf.Pages
{
	/// <summary>
	/// Interaction logic for MainPage.xaml
	/// </summary>
	public partial class MainPage : Page
	{
		MapBuilderApp appMap;
		int mapWidth = 0;
		int mapHeight = 0;
		const int baseGridWidth = 700;
		int maxGridWidth = baseGridWidth;
		const int baseGridHeight = 700;
		int maxGridHeight = baseGridHeight;
		const int baseTileWidth = 32;
		int maxTileWidth = baseTileWidth;
		const int baseTileHeight = 32;
		int maxTileHeight = baseTileHeight;
		List<ColumnDefinition> gridColumns;
		List<RowDefinition> gridRows;
		bottomControlData bottomData;
		rightControlData rightData;
		leftControlData leftData;
		errorMessageData errorData;
		Grid mapGrid;
		DateTime LastSizeUpdate = DateTime.Now;

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
			rightControls.DataContext = rightData;

			leftData = new leftControlData();
			leftData.oxygenCount = "300";
			leftData.oxygenTick = "1.0";
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

			checkSizes((int)this.Width, (int)this.Height);
		}

		private void PageSizeChanged(object sender, SizeChangedEventArgs e)
		{
			checkSizes((int)e.NewSize.Width, (int)e.NewSize.Height);
		}

		/// <summary>
		/// Checks the passed window height and width and calculates new row and column max sizes
		/// so the grid fits the window.
		/// </summary>
		/// <param name="windowWidth"></param>
		/// <param name="windowHeight"></param>
		private void checkSizes(int windowWidth, int windowHeight)
		{
			//Throttle the resize a bit because very large maps can cause the resize to run very slow.
			var delta = DateTime.Now - LastSizeUpdate;
			if (delta.TotalMilliseconds > 500)
			{
				//this system could use some work but it mostly works for the majority of resizing
				//the denominators here are also a bit of magic numbers that were just found by trial and error.
				if (windowWidth < 1200)
				{
					double ratio = 1.0;
					if (windowWidth < 800)
						ratio = (double)windowWidth / 1800.0;
					else
						ratio = (double)windowWidth / 1450.0;
					maxGridWidth = (int)(baseGridWidth * ratio);
					maxTileWidth = (int)(baseTileWidth * ratio);
					maxGridHeight = maxGridWidth;
					maxTileHeight = maxTileWidth;
					updateGridSizes();
				}
				else if (windowHeight < 900)
				{
					double ratio = 1.0;
					ratio = (double)windowHeight / 1200.0;
					maxGridHeight = (int)(baseGridHeight * ratio);
					maxTileHeight = (int)(baseTileHeight * ratio);
					maxGridWidth = maxGridHeight;
					maxTileWidth = maxTileHeight;
					updateGridSizes();
				}
				else if (windowWidth > 1920)
				{
					double ratio = 1.0;
					ratio = (double)windowWidth / 1700.0;
					maxGridWidth = (int)(baseGridWidth * ratio);
					maxTileWidth = (int)(baseTileWidth * ratio);
					maxGridHeight = maxGridWidth;
					maxTileHeight = maxTileWidth;
					updateGridSizes();
				}
				else if (windowHeight > 1080)
				{
					double ratio = 1.0;
					ratio = (double)windowHeight / 1080.0;
					maxGridHeight = (int)(baseGridHeight * ratio);
					maxTileHeight = (int)(baseTileHeight * ratio);
					maxGridWidth = maxGridHeight;
					maxTileWidth = maxTileHeight;
					updateGridSizes();
				}
				else
				{
					maxGridWidth = baseGridWidth;
					maxTileWidth = baseTileWidth;
					maxGridHeight = baseGridHeight;
					maxTileHeight = baseTileHeight;
					updateGridSizes();
				}
			}
		}

		/// <summary>
		/// recalculates grid sizes and updates the current grid.
		/// </summary>
		private void updateGridSizes()
		{
			if (mapGrid != null)
			{
				calculateGridSizes();
				double colWidth = mapGrid.Width / mapWidth;
				double rowHeight = mapGrid.Height / mapHeight;
				foreach (var col in gridColumns)
				{
					col.Width = new GridLength(colWidth);
				}
				foreach (var row in gridRows)
				{
					row.Height = new GridLength(rowHeight);
				}
				foreach (var child in mapGrid.Children)
				{
					var bu = child as Control;
					if (bu != null)
					{
						gridTileData gbd = bu.DataContext as gridTileData;
						if (gbd != null)
						{
							gbd.Width = colWidth;
							gbd.Height = rowHeight;
						}
					}
				}
			}
		}

		/// <summary>
		/// Does the caluclations for grid and column widths so that they fit the grid sizes
		/// </summary>
		private void calculateGridSizes()
		{
			mapGrid.Width = 0;
			mapGrid.Height = 0;
			if (mapWidth > mapHeight)
			{
				mapGrid.Width = mapWidth * maxTileWidth > maxGridWidth ? maxGridWidth : mapWidth * maxTileWidth;
				mapGrid.Height = (mapGrid.Width) * ((float)mapHeight / (float)mapWidth > 1 ? 1 : (float)mapHeight / (float)mapWidth);
			}
			else if (mapWidth < mapHeight)
			{
				mapGrid.Height = mapHeight * maxTileHeight > maxGridHeight ? maxGridHeight : mapHeight * maxTileHeight;
				mapGrid.Width = (mapGrid.Height) * ((float)mapWidth / (float)mapHeight > 1 ? 1 : (float)mapWidth / (float)mapHeight);
			}
			else
			{
				mapGrid.Width = mapWidth * maxTileWidth > maxGridWidth ? maxGridWidth : mapWidth * maxTileWidth;
				mapGrid.Height = mapHeight * maxTileHeight > maxGridHeight ? maxGridHeight : mapHeight * maxTileHeight;
			}
		}

		/// <summary>
		/// Generate a new map if the width and height from the controls is valid.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void newMapButtonClick(object sender, RoutedEventArgs e)
		{
			//parse width and height
			bool success = int.TryParse(bottomData.widthTextBox, out mapWidth);
			if (success)
				success = int.TryParse(bottomData.heightTextBox, out mapHeight);
			else
			{
				errorData.errorMessage = "Invalid Map Width";
				return;
			}

			if (success)
			{
				appMap = new MapBuilderApp();
				appMap.initializeMap(mapWidth, mapHeight);
				BuildMapGrid();
				errorData.errorMessage = "";
			}
			else
			{
				errorData.errorMessage = "Invalid Map Height";
				return;
			}
		}

		/// <summary>
		/// Save the map
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void saveMapButtonClick(object sender, RoutedEventArgs e)
		{
			if (mapGrid != null)
			{
				var result = appMap.saveMap();
				if (result)
				{
					errorData.errorMessage = "";
					return;
				}
				else
					errorData.errorMessage = "Error Saving Map";
			}
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
									if (e.LeftButton == MouseButtonState.Pressed)
									{
										changeGridTile(gtd);
									}
								}
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
					if (appMap.buildMap.modifyTile(data.x, data.y, (terrainType)rightData.terrainType, oreCount, crystalCount, rightData.mobSpawn))
					{
						var tileDef = appMap.tileColors.Where(tile => tile.type == (terrainType)rightData.terrainType).ToArray();
						var colorToUse = tileDef.Length > 0 ? tileDef[0].tileColor : Color.FromRgb(0, 0, 0);
						data.Background = new SolidColorBrush(colorToUse);
						data.oreCount = oreCount;
						data.crystalCount = crystalCount;
						data.mobSpawn = rightData.mobSpawn;
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
			calculateGridSizes();
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
						var tileDef = appMap.tileColors.Where(tile => tile.type == tileType).ToArray();
						var colorToUse = tileDef.Length > 0 ? tileDef[0].tileColor : Color.FromRgb(0, 0, 0);
						tileData.Background = new SolidColorBrush(colorToUse);
						tileData.Height = rowHeight;
						tileData.Width = colWidth;
						tileData.x = x;
						tileData.y = y;
						tileData.oreCount = 3;
						tileData.crystalCount = 0;
						tileData.mobSpawn = false;
						tileData.terrain = tileType;
						gridTile.DataContext = tileData;
						Grid.SetRow(gridTile, y);
						Grid.SetColumn(gridTile, x);
						mapGrid.Children.Add(gridTile);
					}
				}
			}

			mapParentGrid.Children.Add(mapGrid);
		}
	}

	public class bottomControlData
	{
		public string widthTextBox { get; set; }
		public string heightTextBox { get; set; }
	}

	public class rightControlData
	{
		public int terrainType { get; set; }
		public string oreCount { get; set; }
		public string crystalCount { get; set; }
		public bool mobSpawn { get; set; }
	}

	public class leftControlData
	{
		public string oxygenCount { get; set; }
		public string oxygenTick { get; set; }
	}

	public class errorMessageData : INotifyPropertyChanged
	{
		string _errorMessage;

		public string errorMessage
		{
			get
			{
				return _errorMessage;
			}
			set
			{
				_errorMessage = value;
				NotifyPropertyChanged("errorMessage");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String info)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}
	}

	public class gridTileData : Control
	{
		public int x;
		public int y;
		public terrainType terrain;
		public int oreCount;
		public int crystalCount;
		public bool mobSpawn;
	}
}