using MapBuilderLibWindows;
using MapEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MapBuilderWpf
{
	public class MapBuilderApp
	{
		public Map buildMap { get; private set; }
		int width;
		int height;

		public MapBuilderApp()
		{
		}

		public void initializeMap(int width, int height)
		{
			this.width = width;
			this.height = height;
			buildMap = new Map(width, height);
			buildMap.intializeBlankMap();
		}

		public bool saveMap()
		{
			return buildMap.saveMap();
		}
	}
}
