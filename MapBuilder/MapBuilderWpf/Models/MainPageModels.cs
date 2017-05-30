//system
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
//map builder lib
using MapEnums;

namespace MapBuilderWpf.Models
{
	public class bottomControlData
	{
		public string widthTextBox { get; set; }
		public string heightTextBox { get; set; }
	}

	public class rightControlData : INotifyPropertyChanged
	{
		public int terrainType { get; set; }
		public string oreCount { get; set; }
		public string crystalCount { get; set; }
		public bool mobSpawn { get; set; }
		public bool crystalRecharge { get; set; }
		public int buildingType { get; set; }
		private Visibility _showMapControls;
		public Visibility showMapControls
		{
			get
			{
				return _showMapControls;
			}
			set
			{
				_showMapControls = value;
				NotifyPropertyChanged("showMapControls");
			}
		}
		private Visibility _showBuildingControls;
		public Visibility showBuildingControls
		{
			get
			{
				return _showBuildingControls;
			}
			set
			{
				_showBuildingControls = value;
				NotifyPropertyChanged("showBuildingControls");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String info)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
		}
	}

	public class leftControlData : INotifyPropertyChanged
	{
		public string oxygenCount { get; set; }
		public string oxygenTick { get; set; }
		public string mapName { get; set; }
		private Visibility _showLeftControls;
		public Visibility showLeftControls
		{
			get
			{
				return _showLeftControls;
			}
			set
			{
				_showLeftControls = value;
				NotifyPropertyChanged("showLeftControls");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String info)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
		}
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

	public class buildingGhostData : INotifyPropertyChanged
	{
		private Visibility _showBuildingGhost;
		public Visibility showBuildingGhost
		{
			get
			{
				return _showBuildingGhost;
			}
			set
			{
				_showBuildingGhost = value;
				NotifyPropertyChanged("showBuildingGhost");
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

	public class gridTileData : Control, INotifyPropertyChanged
	{
		public int x;
		public int y;
		terrainType _terrain;
		buildingSection _buildingSection;
		Guid _buildingGuid;
		int _oreCount;
		int _crystalCount;
		bool _mobSpawn;
		bool _crystalRecharge;
		bool _hasBuilding;
		Visibility _showOre;
		Visibility _showCrystal;
		Visibility _showSpecial;
		Visibility _showBuilding;
		double _fontSize;

		public terrainType terrain
		{
			get
			{
				return _terrain;
			}
			set
			{
				_terrain = value;
				NotifyPropertyChanged("terrain");
			}
		}

		public buildingSection buildingSection
		{
			get
			{
				return _buildingSection;
			}
			set
			{
				_buildingSection = value;
				NotifyPropertyChanged("buildingSection");
			}
		}

		public Guid buildingGuid { get { return _buildingGuid; } set { _buildingGuid = value; } }

		public int oreCount
		{
			get
			{
				return _oreCount;
			}
			set
			{
				_oreCount = value;
				NotifyPropertyChanged("oreCount");
			}
		}

		public int crystalCount
		{
			get
			{
				return _crystalCount;
			}
			set
			{
				_crystalCount = value;
				NotifyPropertyChanged("crystalCount");
			}
		}

		public bool mobSpawn
		{
			get
			{
				return _mobSpawn;
			}
			set
			{
				_mobSpawn = value;
				NotifyPropertyChanged("mobSpawn");
			}
		}

		public bool crystalRecharge
		{
			get
			{
				return _crystalRecharge;
			}
			set
			{
				_crystalRecharge = value;
				NotifyPropertyChanged("crystalRecharge");
			}
		}

		public bool hasBuilding
		{
			get
			{
				return _hasBuilding;
			}
			set
			{
				_hasBuilding = value;
				NotifyPropertyChanged("hasBuilding");
			}
		}

		public Visibility showOre
		{
			get
			{
				return _showOre;
			}
			set
			{
				_showOre = value;
				NotifyPropertyChanged("showOre");
			}
		}

		public Visibility showCrystal
		{
			get
			{
				return _showCrystal;
			}
			set
			{
				_showCrystal = value;
				NotifyPropertyChanged("showCrystal");
			}
		}

		public Visibility showSpecial
		{
			get
			{
				return _showSpecial;
			}
			set
			{
				_showSpecial = value;
				NotifyPropertyChanged("showSpecial");
			}
		}

		public Visibility showBuilding
		{
			get
			{
				return _showBuilding;
			}
			set
			{
				_showBuilding = value;
				NotifyPropertyChanged("showBuilding");
			}
		}

		public double fontSize
		{
			get
			{
				return _fontSize;
			}
			set
			{
				_fontSize = value;
				NotifyPropertyChanged("fontSize");
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
}
