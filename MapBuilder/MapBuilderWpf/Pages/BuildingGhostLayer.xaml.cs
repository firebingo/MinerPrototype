//system
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
//map builder wpf
using MapBuilderWpf.Models;
using MapBuilderWpf.Helpers;
//map builder lib
using MapBuilderLibWindows;
using MapEnums;

namespace MapBuilderWpf.Pages
{
	/// <summary>
	/// Interaction logic for BuildingGhostLayer.xaml
	/// </summary>
	public partial class BuildingGhostLayer : Page
	{
		private buildingGhostData buildingGhostData;
		public viewsEnum currentView;
		private string buildingType = "None";
		private orientation buildingOrientation;
		private Grid ghostGrid = null;
		private List<ColumnDefinition> gridColumns;
		private List<RowDefinition> gridRows;
		private int mapWidth = 0;
		private int mapHeight = 0;

		public BuildingGhostLayer()
		{
			InitializeComponent();

			EventHelper.dynamicMessageEvent += handleEvent;

			buildingGhostData = new buildingGhostData();
			buildingGhostData.showBuildingGhost = Visibility.Hidden;
			buildingGhost.DataContext = buildingGhostData;
		}

		/// <summary>
		/// Catch events from the message system
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void handleEvent(object sender, dynamicMessagesArgs e)
		{
			if (e.target == "changeBuilding" && HelperFunctions.hasProperty(e.args, "building"))
			{
				if (e.args.building != null)
					changeBuilding(e.args.building);
				else
					return;
			} 
			else if (e.target == "changeOrientation" && HelperFunctions.hasProperty(e.args, "orientation"))
			{
				changeOrientation(e.args.orientation);
			}
			else if (e.target == "mapDimensions" && HelperFunctions.hasProperty(e.args, "width") && HelperFunctions.hasProperty(e.args, "height"))
			{
				setMapDimensions(e.args.width, e.args.height);
			}
			else if (e.target == "mainMouseMove" && HelperFunctions.hasProperty(e.args, "position"))
			{
				setGhostPosition(e.args.position);
			}
			else
				return;
		}

		private void setMapDimensions(int width, int height)
		{
			mapWidth = width;
			mapHeight = height;
		}

		private void changeBuilding(string building)
		{
			buildingType = building;
			buildGhostGrid();
		}

		private void changeOrientation (orientation orientation)
		{
			buildingOrientation = orientation;
			buildGhostGrid();
		}

		private void setGhostPosition(Point position)
		{
			Canvas.SetLeft(ghostGrid, position.X - 10);
			Canvas.SetTop(ghostGrid, position.Y - 10);
		}

		/// <summary>
		/// Create the grid for the building ghost and put it on the page.
		/// </summary>
		private void buildGhostGrid()
		{
			var layout = PredefBuildings.preBuildings.ContainsKey(buildingType) ? PredefBuildings.preBuildings[buildingType] : null;

			//remove the old grid if there is one
			if (buildingGhost.Children.Count > 0)
				buildingGhost.Children.Clear();

			double? left = null;
			double? top = null;
			if(ghostGrid != null)
			{
				left = Canvas.GetLeft(ghostGrid);
				top = Canvas.GetTop(ghostGrid);
			}
			ghostGrid = new Grid();

			//clear the grid rows and columns
			if (gridColumns != null)
				gridColumns.Clear();
			else
				gridColumns = new List<ColumnDefinition>();
			if (gridRows != null)
				gridRows.Clear();
			else
				gridRows = new List<RowDefinition>();

			if(layout != null)
			{
				var model = new BuildingModel(buildingType, buildingOrientation, new Vector2<int>(-1, -1), layout);
				var oriented = model.orientedLayout;
				var width = oriented.GetLength(0);
				var height = oriented.GetLength(1);
				ghostGrid.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
				GridHelper.calculateGridSizes(ghostGrid, mapWidth, mapHeight);
				ghostGrid.HorizontalAlignment = HorizontalAlignment.Center;
				ghostGrid.VerticalAlignment = VerticalAlignment.Center;
				double colWidth = ghostGrid.Width / mapWidth;
				double rowHeight = ghostGrid.Height / mapHeight;
				for (int x = 0; x < width; ++x)
				{
					ColumnDefinition column = new ColumnDefinition();
					gridColumns.Add(column);
				}
				foreach (var col in gridColumns)
				{
					col.Width = new GridLength(colWidth);
					ghostGrid.ColumnDefinitions.Add(col);
				}
				for (int y = 0; y < height; ++y)
				{
					RowDefinition row = new RowDefinition();
					gridRows.Add(row);
				}
				foreach (var row in gridRows)
				{
					row.Height = new GridLength(rowHeight);
					ghostGrid.RowDefinitions.Add(row);
				}

				var tileTemplate = FindResource("GhostGridTileTemplate") as ControlTemplate;

				for (int x = 0; x < width; ++x)
				{
					for (int y = 0; y < height; ++y)
					{
						if (tileTemplate != null)
						{
							gridGhostTileData tileData = new gridGhostTileData();
							Control gridTile = new Control();
							gridTile.Template = tileTemplate;
							tileData.Height = rowHeight;
							tileData.Width = colWidth;
							tileData.x = x;
							tileData.y = y;
							tileData.buildingSection = oriented[x, y].section;
							tileData.tileExists = tileData.buildingSection != buildingSection.empty ? Visibility.Visible : Visibility.Hidden;
							tileData.Background = new SolidColorBrush(GridHelper.buildingTileColors[tileData.buildingSection]);
							gridTile.DataContext = tileData;
							Grid.SetRow(gridTile, y);
							Grid.SetColumn(gridTile, x);
							ghostGrid.Children.Add(gridTile);
						}
					}
				}

				buildingGhost.Children.Add(ghostGrid);
				if(left.HasValue && top.HasValue)
				setGhostPosition(new Point(left.Value, top.Value));
			}
		}

		/// <summary>
		/// Changes the current view of the grid.
		/// </summary>
		/// <param name="view"></param>
		public void changeCurrentView(viewsEnum view, bool hasMap)
		{
			currentView = view;
			if (hasMap)
			{
				switch (currentView)
				{
					default:
						buildingGhostData.showBuildingGhost = Visibility.Hidden;
						break;
					case viewsEnum.building:
						buildingGhostData.showBuildingGhost = Visibility.Visible;
						break;
				}
			}
			else
				buildingGhostData.showBuildingGhost = Visibility.Hidden;
		}
	}
}
