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
		private Visibility _tileExists;

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
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}
	}
}
