using MapBuilderWpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
