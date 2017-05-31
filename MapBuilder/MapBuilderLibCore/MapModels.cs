using MapEnums;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MapBuilderLibCore
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

		//These will throw expections if type T does not have the operator itself.
		public static Vector2<T> operator +(Vector2<T> v, Vector2<T> v2)
		{
			return new Vector2<T>((dynamic)v.x + (dynamic)v2.x, (dynamic)v2.y + (dynamic)v2.y);
		}

		public static Vector2<T> operator -(Vector2<T> v, Vector2<T> v2)
		{
			return new Vector2<T>((dynamic)v.x - (dynamic)v2.x, (dynamic)v2.y - (dynamic)v2.y);
		}

		public static Vector2<T> operator -(Vector2<T> v)
		{
			return new Vector2<T>(-(dynamic)v.x, -(dynamic)v.y);
		}

		public static bool operator ==(Vector2<T> lhs, Vector2<T> rhs)
		{
			return (lhs.x.Equals(rhs.x) && lhs.y.Equals(rhs.y));
		}

		public static bool operator !=(Vector2<T> lhs, Vector2<T> rhs)
		{
			return (!lhs.x.Equals(rhs.x) || !lhs.y.Equals(rhs.y));
		}

		public override string ToString()
		{
			return ($"{_x.ToString()}, {_y.ToString()}");
		}

		public string ToString(string format)
		{
			var first = format.Replace("%X", _x.ToString());
			return first.Replace("%Y", _y.ToString());
		}
	}

	public class MapHeader
	{
		public string mapName;
		public float oxygenCount;
		public float oxygenRate;
		public mapGoal goal;
		public int goalAmount;

		public MapHeader()
		{
		}
	}

	public class MapTile
	{
		public terrainType tileType;
		public buildingSection buildingSection;
		public Vector2<int> pos { get; private set; }
		public int oreCount;
		public int crystalCount;
		public bool mobSpawn;
		public bool crystalRecharge;
		public Guid building;
		public bool hidden;

		public MapTile(terrainType type, Vector2<int> pos, int oreCount = 3, int crystalCount = 0, bool mobSpawn = false, bool crystalRecharge = false, bool hidden = false)
		{
			this.pos = new Vector2<int>();
			this.pos.x = pos.x;
			this.pos.y = pos.y;
			tileType = type;
			this.oreCount = oreCount;
			this.crystalCount = crystalCount;
			this.hidden = hidden;
			building = default(Guid);
			buildingSection = buildingSection.empty;
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
				//The serializtion in here is to deep copy the arrays content because c# doesn't have a way to do in natively.
				var height = buildingLayout.GetLength(1);
				var width = buildingLayout.GetLength(0); 
				var serialized = JsonConvert.SerializeObject(buildingLayout);
				var modifiable = JsonConvert.DeserializeObject<BuildingTile[,]>(serialized);
				switch (buildingDirection)
				{
					default:
					case orientation.north:
						return modifiable;
					case orientation.east:
						return rotateBuildingPos90(modifiable);
					case orientation.west:
						var rot1 = rotateBuildingPos90(modifiable);
						serialized = JsonConvert.SerializeObject(rot1);
						var rot2 = rotateBuildingPos90(JsonConvert.DeserializeObject<BuildingTile[,]>(serialized));
						serialized = JsonConvert.SerializeObject(rot2);
						return rotateBuildingPos90(JsonConvert.DeserializeObject<BuildingTile[,]>(serialized));
					case orientation.south:
						rot1 = rotateBuildingPos90(modifiable);
						serialized = JsonConvert.SerializeObject(rot1);
						return rotateBuildingPos90(JsonConvert.DeserializeObject<BuildingTile[,]>(serialized));
				}
			}
		}
		public Vector2<int> pos;

		/// <summary>
		/// A model of a building intended to be placed on the map.
		/// </summary>
		/// <param name="type">A string denoting the type/name of the building</param>
		/// <param name="direction">The orinentation of the building</param>
		/// <param name="pos">The position the 0x0 (top left) tile of the building will be placed at</param>
		/// <param name="layout">The array of tiles that compose the building</param>
		public BuildingModel(string type, orientation direction, Vector2<int> pos, BuildingTile[,] layout = null)
		{
			buildingGuid = Guid.NewGuid();
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
				}
			}
			//reverse rows
			BuildingTile[,] flippedBuilding = new BuildingTile[buildingWidth, buildingHeight];
			for (int i = 0; i < buildingWidth; ++i)
			{
				for (int j = buildingHeight - 1; j >= 0; --j)
				{
					flippedBuilding[i, j] = rotBuilding[i, buildingHeight - j - 1];
				}
			}
			return flippedBuilding;
		}

		//From https://stackoverflow.com/questions/18034805/rotate-mn-matrix-90-degrees in case 
		//I need future refernce for changing the rotation functions.
		//static int[,] RotateMatrixCounterClockwise(int[,] oldMatrix)
		//{
		//	int[,] newMatrix = new int[oldMatrix.GetLength(1), oldMatrix.GetLength(0)];
		//	int newColumn, newRow = 0;
		//	for (int oldColumn = oldMatrix.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
		//	{
		//		newColumn = 0;
		//		for (int oldRow = 0; oldRow < oldMatrix.GetLength(0); oldRow++)
		//		{
		//			newMatrix[newRow, newColumn] = oldMatrix[oldRow, oldColumn];
		//			newColumn++;
		//		}
		//		newRow++;
		//	}
		//	return newMatrix;
		//}
	}

	public class BuildingTile
	{
		public buildingSection section;
		public bool core;
		public bool onLava = false;
		public bool onWater = false;
		public bool inWall = false;
	}
}
