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
	public enum viewsEnum {
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

		public MainWindow()
		{
			currentView = viewsEnum.terrain;
			viewItems.Add(terrainView);
			viewItems.Add(oreView);
			viewItems.Add(crystalView);

			InitializeComponent();
		}

		private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
		{
			//checkSizes((int)e.NewSize.Width, (int)e.NewSize.Height);
			//if (e.NewSize.Width != e.PreviousSize.Width)
			//{
			//	topMenuBar.Width = e.NewSize.Width - 8;
			//}
		}

		private void fileMenuButtonClick(object sender, RoutedEventArgs e)
		{
			var bu = sender as Button;
			if (bu != null)
			{
				bu.ContextMenu.IsEnabled = true;
				bu.ContextMenu.PlacementTarget = bu;
				bu.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
				bu.ContextMenu.IsOpen = true;
			}
		}
	}
}
