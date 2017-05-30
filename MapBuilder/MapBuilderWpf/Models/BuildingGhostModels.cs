//system
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
//map builder lib
using MapEnums;

namespace MapBuilderWpf.Models
{
	public class gridGhostTileData : Control, INotifyPropertyChanged
	{
		public int x;
		public int y;
		private buildingSection _buildingSection;
		private Visibility _tileExists;

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

		public Visibility tileExists
		{
			get
			{
				return _tileExists;
			}
			set
			{
				_tileExists = value;
				NotifyPropertyChanged("tileExists");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String info)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
		}
	}
}
