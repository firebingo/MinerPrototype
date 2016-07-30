using MapEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapBuilderLibWindows
{
	public class MapTile
	{
		public terrainType tileType;
		public int x { get; private set; }
		public int y { get; private set; }
		public int oreCount;
		public int crystalCount;
		public bool mobSpawn;

		public MapTile(terrainType type, int x, int y, int oreCount = 3, int crystalCount = 0, bool mobSpawn = false)
		{
			this.x = x;
			this.y = y;
			tileType = type;
			this.oreCount = oreCount;
			this.crystalCount = crystalCount;
			this.mobSpawn = mobSpawn;
		}
	}
}
