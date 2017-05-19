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

		public BuildingGhostLayer()
		{
			InitializeComponent();

			EventHelper.dynamicMessageEvent += handleEvent;

			buildingGhostData = new buildingGhostData();
			buildingGhostData.showBuildingGhost = Visibility.Hidden;
			buildingGhost.DataContext = buildingGhostData;
		}

		public void handleEvent(object sender, dynamicMessagesArgs e)
		{
			if (e.target == "changeBuilding" && HelperFunctions.hasProperty(e.args, "building"))
			{
				if (e.args.building != null)
					changeBuilding(e.args.building);
				else
					return;
			}
			else
				return;
		}

		public void changeBuilding(string building)
		{

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
