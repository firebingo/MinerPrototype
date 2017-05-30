using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MapEnums;

namespace MapBuilderLibCore
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
			buildings.mapBuildings = new Dictionary<Guid, BuildingModel>();
		}

		/// <summary>
		/// Creates a new map of the dimensions from the contructor with solid rock on the edges and floor covering the rest.
		/// </summary>
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

		/// <summary>
		/// Modifies a tile
		/// </summary>
		/// <param name="x">The X coordinate of the tile</param>
		/// <param name="y">The Y coordniate of the tile</param>
		/// <param name="tileType">The terrainType of the tile</param>
		/// <param name="oreCount">The count of ore in the tile if it is a wall</param>
		/// <param name="crystalCount">The count of crystals in the tile if it is a wall</param>
		/// <param name="mobSpawn">Whether the tile can spawn mobs</param>
		/// <param name="crystalRecharge">Whether the tile can be used to recharge crystals</param>
		/// <returns>Returns true if modifying the tile succedded</returns>
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

		/// <summary>
		/// Places a building on the map
		/// </summary>
		/// <param name="building">The model of the building to place</param>
		/// <returns>Returns true if placing the building succeded</returns>
		public bool placeBuilding(BuildingModel building)
		{
			try
			{
				var orientedLayout = building.orientedLayout;
				var bHeight = orientedLayout.GetLength(0);
				var bWidth = orientedLayout.GetLength(1);
				for(var i = 0; i < bHeight; ++i)
				{
					for(var j = 0; j < bWidth; ++j)
					{
						var tile = orientedLayout[i, j];
						if (tile.section != buildingSection.empty)
						{
							var tileX = building.pos.x + i;
							var tileY = building.pos.y + j;
							if (tileX < 0 || tileX > width)
								return false;
							if (tileY < 0 || tileY > height)
								return false;
							if (mapTiles[tileX, tileY].building != Guid.Empty)
								return false;
							mapTiles[tileX, tileY].tileType = terrainType.floor;
							mapTiles[tileX, tileY].buildingSection = tile.section;
							mapTiles[tileX, tileY].mobSpawn = false;
							mapTiles[tileX, tileY].building = building.buildingGuid;
						}
					}
				}
				buildings.mapBuildings.Add(building.buildingGuid, building);
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		/// <summary>
		/// Removes a building from the map.
		/// </summary>
		/// <param name="buildingGuid">The guid of the building to remove</param>
		/// <returns></returns>
		public bool removeBuilding(Guid buildingGuid)
		{
			try
			{
				foreach (var tile in mapTiles)
				{
					if (tile.building == buildingGuid)
						tile.building = default(Guid);
				}
				buildings.mapBuildings.Remove(buildingGuid);
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		/// <summary>
		/// Saves the map to the given directory.
		/// Will overwrite the file if it already exists.
		/// </summary>
		/// <param name="directory">The directory to save in, does not include filename.</param>
		/// <param name="fileName">The filename of the file to save.</param>
		/// <returns>Returns true if saving the map to the directory succeeds.</returns>
		public async Task<bool> saveMap(string directory, string fileName)
		{
			return await mapWriter.serializeMap(directory, fileName, mapTiles, mapHeader, buildings);
		}
	}
}
