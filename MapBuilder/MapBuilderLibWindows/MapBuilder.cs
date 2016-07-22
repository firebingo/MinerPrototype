using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MapEnums;

namespace MapBuilderLibWindows
{
	public class MapHeader
	{
		public float oxygenCount;
		public float oxygenRate;

		public MapHeader()
		{
		}
	}

	public class Map
	{
		public int width;
		public int height;

		public MapWriter mapWriter { get; private set; }
		public MapHeader mapHeader { get; private set; }
		public MapTile[,] mapTiles { get; private set; }

		public Map(int width, int height)
		{
			this.width = width;
			this.height = height;
			mapWriter = new MapWriter();
			mapHeader = new MapHeader();
			mapTiles = new MapTile[width, height];
		}

		public void intializeBlankMap()
		{
			//initialize all tiles to floor.
			for (int x = 0; x < width; ++x)
			{
				for (int y = 0; y < height; ++y)
				{
					mapTiles[x, y] = new MapTile(terrainType.floor, x, y);
				}
			}

			//initialize walls to solid rock.
			for (int x = 0; x < width; ++x)
			{
				mapTiles[x, 0].tileType = terrainType.solidrock;
				mapTiles[x, height-1].tileType = terrainType.solidrock;
			}
			for (int y = 0; y < height; ++y)
			{
				mapTiles[0, y].tileType = terrainType.solidrock;
				mapTiles[width - 1, y].tileType = terrainType.solidrock;
			}
		}

		public void modifyTile(int x, int y, terrainType tileType, bool mobSpawn)
		{
			if(x < width && y < height)
			{
				mapTiles[x, y].tileType = tileType;
				mapTiles[x, y].mobSpawn = mobSpawn;
			}
		}
	}

	public class MapTile
	{
		public terrainType tileType;
		public int x { get; private set; }
		public int y { get; private set; }
		public bool mobSpawn;

		public MapTile(terrainType type, int x, int y, bool mobSpawn = false)
		{
			this.x = x;
			this.y = y;
			tileType = type;
			this.mobSpawn = mobSpawn;
		}
	}

	public class MapWriter
	{

	}
}
