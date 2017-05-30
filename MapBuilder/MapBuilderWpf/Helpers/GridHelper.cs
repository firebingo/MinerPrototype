//system
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
//map builder lib
using MapBuilderWpf.Models;
using MapEnums;

namespace MapBuilderWpf.Helpers
{
	public static class GridHelper
	{
		public static DateTime LastSizeUpdate { get; private set; } = DateTime.Now;
		private const int baseGridWidth = 725;
		private static int maxGridWidth = baseGridWidth;
		private const int baseGridHeight = 725;
		private static int maxGridHeight = baseGridHeight;
		private const int baseTileWidth = 32;
		private static int maxTileWidth = baseTileWidth;
		private const int baseTileHeight = 32;
		private static int maxTileHeight = baseTileHeight;
		private const double baseFontSize = 16;
		public static double currentFontSize = baseFontSize;
		public static Dictionary<terrainType, Color> terrainTileColors { get; } = new Dictionary<terrainType, Color>()
		{
			{ terrainType.empty, Color.FromRgb(0, 0, 0) },
			{ terrainType.floor, Color.FromRgb(205, 160, 113) },
			{ terrainType.roof, Color.FromRgb(89, 59, 53) },
			{ terrainType.softrock, Color.FromRgb(168, 122, 74) },
			{ terrainType.looserock, Color.FromRgb(142, 100, 55) },
			{ terrainType.hardrock, Color.FromRgb(92, 74, 54) },
			{ terrainType.solidrock, Color.FromRgb(65, 58, 50) },
			{ terrainType.water, Color.FromRgb(0, 90, 255) },
			{ terrainType.lava, Color.FromRgb(255, 90, 0) },
		};
		public static Dictionary<buildingSection, Color> buildingTileColors { get; } = new Dictionary<buildingSection, Color>()
		{
			{ buildingSection.building, Color.FromRgb(128, 128, 128) },
			{ buildingSection.path, Color.FromRgb(160, 160, 170) },
			{ buildingSection.empty, Color.FromArgb(0, 128, 128, 128) },
			{ buildingSection.personTeleport, Color.FromRgb(180, 180, 100) },
			{ buildingSection.resourceCollect, Color.FromRgb(220, 180, 100) },
			{ buildingSection.landTeleport, Color.FromRgb(180, 20, 180) },
			{ buildingSection.waterTeleport, Color.FromRgb(180, 20, 220) },
			{ buildingSection.lavaTeleport, Color.FromRgb(220, 20, 180) },
			{ buildingSection.energyCollect, Color.FromRgb(20, 180, 20) },
			{ buildingSection.energyRecharge, Color.FromRgb(30, 220, 30) },
		};
		//public static Color buildingColor { get; } = Color.FromArgb(255, 128, 128, 128);

		/// <summary>
		/// Checks the passed window height and width and calculates new row and column max sizes
		/// so the grid fits the window.
		/// </summary>
		/// <param name="windowWidth"></param>
		/// <param name="windowHeight"></param>
		public static void checkSizes(int windowWidth, int windowHeight, Grid iGrid, int gridWidth, int gridHeight, List<ColumnDefinition> columns, List<RowDefinition> rows)
		{
			LastSizeUpdate = DateTime.Now;
			double ratio = 1.0;
			long baseSize = 1920 * 1080;
			ratio = (double)(windowWidth * windowHeight) / baseSize;
			maxGridWidth = (int)(baseGridWidth * ratio);
			maxTileWidth = (int)(baseTileWidth * ratio);
			maxGridHeight = maxGridWidth;
			maxTileHeight = maxTileWidth;
			updateGridSizes(iGrid, gridWidth, gridHeight, columns, rows);
		}

		/// <summary>
		/// recalculates grid sizes and updates the current grid.
		/// </summary>
		public static void updateGridSizes(Grid iGrid, int gridWidth, int gridHeight, List<ColumnDefinition> columns, List<RowDefinition> rows)
		{
			if (iGrid != null)
			{
				calculateGridSizes(iGrid, gridWidth, gridHeight);
				double colWidth = iGrid.Width / gridWidth;
				double rowHeight = iGrid.Height / gridHeight;
				currentFontSize = (double)colWidth / (double)2;
				foreach (var col in columns)
				{
					col.Width = new GridLength(colWidth);
				}
				foreach (var row in rows)
				{
					row.Height = new GridLength(rowHeight);
				}
				foreach (var child in iGrid.Children)
				{
					var bu = child as Control;
					if (bu != null)
					{
						gridTileData gbd = bu.DataContext as gridTileData;
						if (gbd != null)
						{
							gbd.fontSize = currentFontSize > 8 ? currentFontSize : 0.01;
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
		public static void calculateGridSizes(Grid iGrid, int gridWidth, int gridHeight)
		{
			iGrid.Width = 0;
			iGrid.Height = 0;
			if (gridWidth > gridHeight)
			{
				iGrid.Width = gridWidth * maxTileWidth > maxGridWidth ? maxGridWidth : gridWidth * maxTileWidth;
				iGrid.Height = (iGrid.Width) * ((float)gridHeight / (float)gridWidth > 1 ? 1 : (float)gridHeight / (float)gridWidth);
			}
			else if (gridWidth < gridHeight)
			{
				iGrid.Height = gridHeight * maxTileHeight > maxGridHeight ? maxGridHeight : gridHeight * maxTileHeight;
				iGrid.Width = (iGrid.Height) * ((float)gridWidth / (float)gridHeight > 1 ? 1 : (float)gridWidth / (float)gridHeight);
			}
			else
			{
				iGrid.Width = gridWidth * maxTileWidth > maxGridWidth ? maxGridWidth : gridWidth * maxTileWidth;
				iGrid.Height = gridHeight * maxTileHeight > maxGridHeight ? maxGridHeight : gridHeight * maxTileHeight;
			}
		}
	}
}
