using MapEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapBuilderLibWindows
{
	public class Vector2<T>
	{
		private T _x;
		private T _y;
		public T x { get { return _x; } set { _x = value; } }
		public T y { get { return _y; } set { _y = value; } }

		public Vector2()
		{
			x = default(T);
			y = default(T);
		}

		public Vector2(T x, T y)
		{
			this.x = x;
			this.y = y;
		}
	}

	public class MapHeader
	{
		public float oxygenCount;
		public float oxygenRate;

		public MapHeader()
		{
		}
	}

	public class MapTile
	{
		public terrainType tileType;
		public Vector2<int> pos { get; private set; }
		public int oreCount;
		public int crystalCount;
		public bool mobSpawn;
		public bool crystalRecharge;
		public Guid building;

		public MapTile(terrainType type, Vector2<int> pos, int oreCount = 3, int crystalCount = 0, bool mobSpawn = false, bool crystalRecharge = false)
		{
			this.pos = new Vector2<int>();
			this.pos.x = pos.x;
			this.pos.y = pos.y;
			tileType = type;
			this.oreCount = oreCount;
			this.crystalCount = crystalCount;
			building = default(Guid);
			if (tileType > terrainType.roof && tileType < terrainType.water)
			{
				this.mobSpawn = mobSpawn;
				this.crystalRecharge = crystalRecharge;
			}
			else
			{
				this.mobSpawn = false;
				this.crystalRecharge = false;
			}
		}
	}

	public class MapBuildings
	{
		public Dictionary<Guid, BuildingModel> mapBuildings;
	}

	public class BuildingModel
	{
		public Guid buildingGuid;
		public string buildingType;
		public orientation buildingDirection;
		public BuildingTile[,] buildingLayout;
		public BuildingTile[,] orientedLayout
		{
			get
			{
				switch (buildingDirection)
				{
					default:
					case orientation.north:
						return buildingLayout;
					case orientation.east:
						return rotateBuildingPos90(buildingLayout);
					case orientation.west:
						return rotateBuildingPos90(buildingLayout);
					case orientation.south:
						var rot1 = rotateBuildingPos90(buildingLayout);
						return rotateBuildingPos90(rot1);
				}
			}
		}
		public Vector2<int> pos;

		public BuildingModel(string type, orientation direction, Vector2<int> pos, BuildingTile[,] layout = null)
		{
			buildingGuid = new Guid();
			buildingType = type;
			buildingDirection = direction;
			this.pos = new Vector2<int>();
			this.pos.x = pos.x;
			this.pos.y = pos.y;
			if (layout != null)
			{
				buildingLayout = layout;
			}
			else
			{
				if (PredefBuildings.preBuildings.ContainsKey(buildingType) && (buildingType != "Unknown" || buildingType != "None"))
				{
					buildingLayout = PredefBuildings.preBuildings[buildingType];
				}
			}
		}

		private BuildingTile[,] rotateBuildingPos90(BuildingTile[,] building)
		{
			var buildingWidth = building.GetLength(1);
			var buildingHeight = building.GetLength(0);
			//transpose building
			BuildingTile[,] rotBuilding = new BuildingTile[buildingWidth, buildingHeight];
			for (int i = 0; i < buildingWidth; ++i)
			{
				for (int j = 0; j < buildingHeight; ++j)
				{
					rotBuilding[i, j] = building[j, i];
					var tempX = rotBuilding[i, j].relativePos.x;
					rotBuilding[i, j].relativePos.x = rotBuilding[i, j].relativePos.y;
					rotBuilding[i, j].relativePos.y = tempX;
				}
			}
			//reverse rows
			BuildingTile[,] flippedBuilding = new BuildingTile[buildingWidth, buildingHeight];
			for (int i = 0; i < buildingWidth; ++i)
			{
				for (int j = buildingHeight - 1; j >= 0; --j)
				{
					flippedBuilding[i, j] = rotBuilding[i, buildingHeight - j - 1];
					flippedBuilding[i, j].relativePos.x = -flippedBuilding[i, j].relativePos.x;
				}
			}
			return flippedBuilding;
		}

		private BuildingTile[,] rotateBuildingNeg90(BuildingTile[,] building)
		{
			var buildingWidth = building.GetLength(1);
			var buildingHeight = building.GetLength(0);
			//reverse rows
			BuildingTile[,] flippedBuilding = new BuildingTile[buildingWidth, buildingHeight];
			for (int i = 0; i < buildingWidth; ++i)
			{
				for (int j = buildingHeight - 1; j >= 0; --j)
				{
					flippedBuilding[i, j] = building[i, buildingHeight - j - 1];
					flippedBuilding[i, j].relativePos.x = -flippedBuilding[i, j].relativePos.x;
				}
			}
			//transpose building
			BuildingTile[,] rotBuilding = new BuildingTile[buildingWidth, buildingHeight];
			for (int i = 0; i < buildingWidth; ++i)
			{
				for (int j = 0; j < buildingHeight; ++j)
				{
					rotBuilding[i, j] = flippedBuilding[j, i];
					var tempX = rotBuilding[i, j].relativePos.x;
					rotBuilding[i, j].relativePos.x = rotBuilding[i, j].relativePos.y;
					rotBuilding[i, j].relativePos.y = tempX;
				}
			}
			return rotBuilding;
		}
	}

	public class BuildingTile
	{
		public buildingSection section;
		public Vector2<int> relativePos; //the position of the building tile when the orientation is north.
		public bool core;
		public bool onLava = false;
		public bool onWater = false;
		public bool inWall = false;
	}
}
