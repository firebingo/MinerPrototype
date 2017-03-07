﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MapEnums;

namespace MapBuilderLibWindows
{
	public class Map
	{
		public int width { get; private set; }
		public int height { get; private set; }

		public MapWriter mapWriter { get; private set; }
		public MapHeader mapHeader { get; private set; }
		public MapTile[,] mapTiles { get; private set; }
		public MapBuildings buildings { get; private set; }

		public Map(int width, int height)
		{
			this.width = width;
			this.height = height;
			mapWriter = new MapWriter();
			mapHeader = new MapHeader();
			mapTiles = new MapTile[width, height];
			buildings = new MapBuildings();
		}

		public void intializeBlankMap()
		{
			//initialize all tiles to floor.
			for (int x = 0; x < width; ++x)
			{
				for (int y = 0; y < height; ++y)
				{
					mapTiles[x, y] = new MapTile(terrainType.floor, new Vector2<int>(x, y), 0, 0);
				}
			}

			//initialize walls to solid rock.
			for (int x = 0; x < width; ++x)
			{
				mapTiles[x, 0].tileType = terrainType.solidrock;
				mapTiles[x, height - 1].tileType = terrainType.solidrock;
			}
			for (int y = 0; y < height; ++y)
			{
				mapTiles[0, y].tileType = terrainType.solidrock;
				mapTiles[width - 1, y].tileType = terrainType.solidrock;
			}
		}

		public bool modifyTile(int x, int y, terrainType tileType, int oreCount, int crystalCount, bool mobSpawn, bool crystalRecharge)
		{
			try
			{
				if ((x < width && y < height) && (x > -1 && y > -1))
				{
					if (mapTiles[x, y].building != Guid.Empty)
					{
						return false;
					}
					mapTiles[x, y].tileType = tileType;
					mapTiles[x, y].oreCount = oreCount;
					mapTiles[x, y].crystalCount = crystalCount;
					if (mapTiles[x, y].tileType > terrainType.roof && mapTiles[x, y].tileType < terrainType.water)
					{
						mapTiles[x, y].mobSpawn = mobSpawn;
						mapTiles[x, y].crystalRecharge = crystalRecharge;
					}
					else
					{
						mapTiles[x, y].mobSpawn = false;
						mapTiles[x, y].crystalRecharge = false;
					}
					return true;
				}
				return false;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		public bool placeBuilding(BuildingModel building)
		{
			try
			{
				var orientedLayout = building.orientedLayout;
				foreach (var tile in orientedLayout)
				{
					var tileX = building.pos.x + tile.relativePos.x;
					var tileY = building.pos.y + tile.relativePos.y;
					if (tileX < 0 || tileX > width)
						return false;
					if (tileY < 0 || tileY > height)
						return false;
					if(mapTiles[tileX, tileY].building != Guid.Empty)
						return false;
					mapTiles[tileX, tileY].tileType = terrainType.floor;
					mapTiles[tileX, tileY].mobSpawn = false;
					mapTiles[tileX, tileY].building = building.buildingGuid;
				}
				buildings.mapBuildings.Add(building.buildingGuid, building);
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		public bool removeBuilding(Guid buildingGuid)
		{
			var success = false;
			foreach(var tile in mapTiles)
			{
				if (tile.building == buildingGuid)
				{
					tile.building = default(Guid);
					success = true;
				}
			}
			buildings.mapBuildings.Remove(buildingGuid);
			return success;
		}

		public bool saveMap()
		{
			return mapWriter.serializeMap(mapTiles, mapHeader, buildings);
		}
	}
}
