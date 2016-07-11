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

namespace MapBuilderWpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		MapBuilderApp appMap;
		int width = 0;
		int height = 0;
		List<ColumnDefinition> gridColumns;
		List<RowDefinition> gridRows;
		bottomControlData bottomData;
		rightControlData rightData;

		public MainWindow()
		{
			InitializeComponent();

			bottomData = new bottomControlData();
			bottomControls.DataContext = bottomData;

			rightData = new rightControlData();
			rightControls.DataContext = rightData;

			var terrainNames = Enum.GetNames(typeof(terrainType));
			foreach(var name in terrainNames)
			{
				var nameItem = new ComboBoxItem();
				nameItem.Content = name;
				terrainComboBox.Items.Add(nameItem);
			}
		}

		private void newMapButtonClick(object sender, RoutedEventArgs e)
		{
			//parse width and height
			bool success = int.TryParse(bottomData.widthTextBox, out width);
			if (success)
				success = int.TryParse(bottomData.heightTextBox, out height);
			else
				return;

			if (success)
			{
				Stopwatch test = new Stopwatch();
				test.Start();
				appMap = new MapBuilderApp();
				appMap.initializeMap(width, height);
				BuildMapGrid();
				test.Stop();
			}
			else
			{
				return;
			}
		}

		private void gridButtonClick(object sender, RoutedEventArgs e)
		{
			
		}

		private void BuildMapGrid()
		{
			if (mapParentGrid.Children.Count > 0)
				mapParentGrid.Children.Clear();

			var mapGrid = new Grid();
			
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
			mapGrid.Width = 0;
			mapGrid.Height = 0;
			if (width > height)
			{
				mapGrid.Width = width * 32 > 750 ? 750 : width * 32;
				mapGrid.Height = (mapGrid.Width) * ((float)height / (float)width > 1 ? 1 : (float)height / (float)width);
			}
			else if(width < height)
			{
				mapGrid.Height = height * 32 > 750 ? 750 : height * 32 ;
				mapGrid.Width = (mapGrid.Height) * ((float)width / (float)height > 1 ? 1 : (float)width / (float)height);
			}
			else
			{
				mapGrid.Width = width * 32 > 750 ? 750 : width * 32;
				mapGrid.Height = height * 32 > 750 ? 750 : height * 32;
			}
			mapGrid.HorizontalAlignment = HorizontalAlignment.Center;
			mapGrid.VerticalAlignment = VerticalAlignment.Center;
			double colWidth = mapGrid.Width / width;
			double rowHeight = mapGrid.Width / width;
			for (int x = 0; x < width; ++x)
			{
				ColumnDefinition column = new ColumnDefinition();
				gridColumns.Add(column);
			}
			foreach (var col in gridColumns)
			{
				col.Width = new GridLength(colWidth);
				mapGrid.ColumnDefinitions.Add(col);
			}
			for (int y = 0; y < height; ++y)
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
			//create buttons on grid
			for (int x = 0; x < width; ++x)
			{
				for (int y = 0; y < height; ++y)
				{
					if (buttonTemplate != null)
					{
						Button gridButton = new Button();
						gridButton.Template = buttonTemplate;
						gridButton.Click += gridButtonClick;
						var tileType = appMap.buildMap.mapTiles[x, y].tileType;
						var tileDef = appMap.tileColors.Where(tile => tile.type == tileType).ToArray();
						var colorToUse = tileDef.Length > 0 ? tileDef[0].tileColor : Color.FromRgb(0, 0, 0);
						gridButton.Background = new SolidColorBrush(colorToUse);
						gridButton.Height = rowHeight;
						gridButton.Width = colWidth;
						Grid.SetRow(gridButton, y);
						Grid.SetColumn(gridButton, x);
						mapGrid.Children.Add(gridButton);
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
	}

	public class MapBuilderApp
	{
		public List<colorDef> tileColors { get; private set; }
		public Map buildMap { get; private set; }
		int width;
		int height;

		public MapBuilderApp()
		{
			tileColors = new List<colorDef>();
			foreach(var type in Enum.GetValues(typeof(terrainType)))
			{
				terrainType typeToSet = terrainType.empty;
				Color colorToSet = Color.FromRgb(0, 0, 0);
				switch((int)type)
				{
					case 0:
						typeToSet = terrainType.empty;
						colorToSet = Color.FromRgb(0, 0, 0);
						break;
					case 1:
						typeToSet = terrainType.floor;
						colorToSet = Color.FromRgb(205, 160, 113);
						break;
					case 2:
						typeToSet = terrainType.roof;
						colorToSet = Color.FromRgb(89, 59, 53);
						break;
					case 3:
						typeToSet = terrainType.softrock;
						colorToSet = Color.FromRgb(168, 122, 74);
						break;
					case 4:
						typeToSet = terrainType.looserock;
						colorToSet = Color.FromRgb(142, 100, 55);
						break;
					case 5:
						typeToSet = terrainType.hardrock;
						colorToSet = Color.FromRgb(92, 74, 54);
						break;
					case 6:
						typeToSet = terrainType.solidrock;
						colorToSet = Color.FromRgb(65, 58, 50);
						break;
				}
				var color = new colorDef(typeToSet, colorToSet);
				tileColors.Add(color);
			}
		}

		public void initializeMap(int width, int height)
		{
			this.width = width;
			this.height = height;
			buildMap = new Map(width, height);
			buildMap.intializeBlankMap();
		}
	}

	public class colorDef
	{
		public terrainType type { get; private set; }
		public Color tileColor { get; private set; }

		public colorDef(terrainType type, Color tileColor)
		{
			this.type = type;
			this.tileColor = tileColor;
		}
	}
}
