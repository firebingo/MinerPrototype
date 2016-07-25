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

namespace MapBuilderWpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
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

		public MainWindow()
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
			errorData.errorMessage = "TSET";
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

		private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
		{
			checkSizes((int)e.NewSize.Width, (int)e.NewSize.Height);
		}

		private void checkSizes(int windowWidth, int windowHeight)
		{
			var delta = DateTime.Now - LastSizeUpdate;
			if (delta.TotalMilliseconds > 500)
			{
				if (windowWidth < 1200)
				{
					double ratio = 1.0;
					if (windowWidth < 800)
						ratio = (double)windowWidth / 1800.0;
					else
						ratio = (double)windowWidth / 1450.0;
					maxGridWidth = (int)(700 * ratio);
					maxTileWidth = (int)(32 * ratio);
					maxGridHeight = maxGridWidth;
					maxTileHeight = maxTileWidth;
					updateGridSizes();
				}
				else if (windowHeight < 900)
				{
					double ratio = 1.0;
					ratio = (double)windowHeight / 1200.0;
					maxGridHeight = (int)(700 * ratio);
					maxTileHeight = (int)(32 * ratio);
					maxGridWidth = maxGridHeight;
					maxTileWidth = maxTileHeight;
					updateGridSizes();
				}
				else if (windowWidth > 1920)
				{
					double ratio = 1.0;
					ratio = (double)windowWidth / 1700.0;
					maxGridWidth = (int)(700 * ratio);
					maxTileWidth = (int)(32 * ratio);
					maxGridHeight = maxGridWidth;
					maxTileHeight = maxTileWidth;
					updateGridSizes();
				}
				else if (windowHeight > 1080)
				{
					double ratio = 1.0;
					ratio = (double)windowHeight / 1080.0;
					maxGridHeight = (int)(700 * ratio);
					maxTileHeight = (int)(32 * ratio);
					maxGridWidth = maxGridHeight;
					maxTileWidth = maxTileHeight;
					updateGridSizes();
				}
				else
				{
					maxGridWidth = 700;
					maxTileWidth = 32;
					maxGridHeight = 700;
					maxTileHeight = 32;
					updateGridSizes();
				}
			}
		}

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

		private void GridOnMouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				if (mapGrid?.Children != null && mapGrid.Children.Count > 0)
				{
					foreach (var child in mapGrid.Children)
					{
						var bu = child as Control;
						if (bu != null)
						{
							if (bu.IsMouseOver)
							{
								gridTileData gbd = bu.DataContext as gridTileData;
								if (gbd != null)
								{
									if (e.LeftButton == MouseButtonState.Pressed)
									{
										changeTilebutton(gbd);
									}
								}
							}
						}
					}
				}
			}
		}

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
				Stopwatch test = new Stopwatch();
				test.Start();
				appMap = new MapBuilderApp();
				appMap.initializeMap(mapWidth, mapHeight);
				BuildMapGrid();
				test.Stop();
			}
			else
			{
				errorData.errorMessage = "Invalid Map Height";
				return;
			}
		}

		//private void GridButtonMouseMove(object sender, MouseEventArgs e)
		//{
		//	Button bu = sender as Button;
		//	if (bu != null)
		//	{
		//		bu.ReleaseMouseCapture();
		//	}
		//}

		private void GridButtonMouseDown(object sender, MouseButtonEventArgs e)
		{
			Control bu = sender as Control;
			if (bu != null)
			{
				bu.CaptureMouse();
				bu.ReleaseMouseCapture();
			}
		}

		private void gridButtonEnter(object sender, MouseEventArgs e)
		{
			Control bu = sender as Control;
			if (bu != null)
			{
				gridTileData gbd = bu.DataContext as gridTileData;
				if (gbd != null)
				{
					if (e.LeftButton == MouseButtonState.Pressed)
					{
						changeTilebutton(gbd);
					}
				}
			}
		}

		private void changeTilebutton(gridTileData data)
		{
			if (Enum.IsDefined(typeof(terrainType), rightData.terrainType))
			{
				var oreCount = 0;
				var crystalCount = 0;
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
					if (appMap.buildMap.modifyTile(data.x, data.y, (terrainType)rightData.terrainType, oreCount, crystalCount, rightData.mobSpawn))
					{
						var tileDef = appMap.tileColors.Where(tile => tile.type == (terrainType)rightData.terrainType).ToArray();
						var colorToUse = tileDef.Length > 0 ? tileDef[0].tileColor : Color.FromRgb(0, 0, 0);
						data.Background = new SolidColorBrush(colorToUse);
					}
				}
				else
				{
					errorData.errorMessage = "Invalid Crystal Count";
					return;
				}
			}
		}

		private void BuildMapGrid()
		{
			if (mapParentGrid.Children.Count > 0)
				mapParentGrid.Children.Clear();

			mapGrid = new Grid();

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

			var buttonTemplate = FindResource("GridButtonColorTemplate") as ControlTemplate;
			var tileTemplate = FindResource("GridTileTemplate") as ControlTemplate;
			//create buttons on grid
			for (int x = 0; x < mapWidth; ++x)
			{
				for (int y = 0; y < mapHeight; ++y)
				{
					if (buttonTemplate != null)
					{
						gridTileData tileData = new gridTileData();
						Control gridTile = new Control();
						gridTile.Template = tileTemplate;
						gridTile.PreviewMouseDown += GridButtonMouseDown;
						//gridButton.Click += gridButtonClick;
						//gridButton.MouseMove += GridButtonMouseMove;
						//gridButton.MouseEnter += gridButtonEnter;
						var tileType = appMap.buildMap.mapTiles[x, y].tileType;
						var tileDef = appMap.tileColors.Where(tile => tile.type == tileType).ToArray();
						var colorToUse = tileDef.Length > 0 ? tileDef[0].tileColor : Color.FromRgb(0, 0, 0);
						tileData.Background = new SolidColorBrush(colorToUse);
						tileData.Height = rowHeight;
						tileData.Width = colWidth;
						tileData.x = x;
						tileData.y = y;
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
	}

}
