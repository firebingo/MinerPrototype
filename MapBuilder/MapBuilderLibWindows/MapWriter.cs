using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapBuilderLibWindows
{
	public class MapWriter
	{
		string mapString;
		string mapHeader;

		public bool serializeMap(MapTile[,] mapArray, MapHeader header)
		{
			mapString = JsonConvert.SerializeObject(mapArray);
			mapHeader = JsonConvert.SerializeObject(header);
			return true;
		}
	}
}
