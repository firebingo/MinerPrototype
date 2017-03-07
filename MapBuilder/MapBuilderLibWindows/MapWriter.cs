using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapBuilderLibWindows
{
	public class WriteModel
	{
		public MapTile[,] mapArray;
		public MapHeader header;
		public MapBuildings buildings;

		public WriteModel(MapTile[,] ma, MapHeader mh, MapBuildings mb)
		{
			mapArray = ma;
			header = mh;
			buildings = mb;
		}
	}

	public class MapWriter
	{
		string mapInfo;

		public bool serializeMap(MapTile[,] mapArray, MapHeader header, MapBuildings buildings)
		{
			var toWrite = new WriteModel(mapArray, header, buildings);
			mapInfo = JsonConvert.SerializeObject(toWrite);
			return true;
		}
	}
}
