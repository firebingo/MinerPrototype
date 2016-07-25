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
		public List<colorDef> tileColors { get; private set; }
		public Map buildMap { get; private set; }
		int width;
		int height;

		public MapBuilderApp()
		{
			tileColors = new List<colorDef>();
			foreach (var type in Enum.GetValues(typeof(terrainType)))
			{
				terrainType typeToSet = terrainType.empty;
				Color colorToSet = Color.FromRgb(0, 0, 0);
				switch ((int)type)
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
					case 7:
						typeToSet = terrainType.water;
						colorToSet = Color.FromRgb(0,90,255);
						break;
					case 8:
						typeToSet = terrainType.lava;
						colorToSet = Color.FromRgb(255, 90, 0);
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
