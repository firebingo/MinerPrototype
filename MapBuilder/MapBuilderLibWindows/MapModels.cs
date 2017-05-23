using MapEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

		public MapTile(terrainType type, Vector2<int> pos, int oreCount = 3, int crystalCount = 0, bool mobSpawn = false, bool crystalRecharge = false)
		{
			this.pos = new Vector2<int>();
			this.pos.x = pos.x;
			this.pos.y = pos.y;
			tileType = type;
			this.oreCount = oreCount;
			this.crystalCount = crystalCount;
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
				//Deep copy array
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
						//The serializtion is to deep copy otherwise the relative pos vectors get passed by reference and get screwed up.
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
					flippedBuilding[i, j].relativePos.x = flippedBuilding[i, j].relativePos.x;
				}
			}
			return flippedBuilding;
		}

		//private BuildingTile[,] rotateBuildingNeg90(BuildingTile[,] building)
		//{
		//	var buildingWidth = building.GetLength(1);
		//	var buildingHeight = building.GetLength(0);
		//	//reverse rows
		//	BuildingTile[,] flippedBuilding = new BuildingTile[buildingWidth, buildingHeight];
		//	for (int i = 0; i < buildingHeight; ++i)
		//	{
		//		for (int j = buildingWidth - 1; j >= 0; --j)
		//		{
		//			flippedBuilding[j, i] = building[i, buildingWidth - j - 1];
		//			flippedBuilding[j, i].relativePos.x = -flippedBuilding[j, i].relativePos.x;
		//		}
		//	}
		//	//transpose building
		//	BuildingTile[,] rotBuilding = new BuildingTile[buildingWidth, buildingHeight];
		//	for (int i = 0; i < buildingHeight; ++i)
		//	{
		//		for (int j = 0; j < buildingWidth; ++j)
		//		{
		//			rotBuilding[j, i] = building[i, j];
		//			var tempX = rotBuilding[j, i].relativePos.x;
		//			rotBuilding[j, i].relativePos.x = rotBuilding[j, i].relativePos.y;
		//			rotBuilding[j, i].relativePos.y = tempX;
		//		}
		//	}
		//	return rotBuilding;
		//}

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
		public Vector2<int> relativePos; //the position of the building tile when the orientation is north.
		public bool core;
		public bool onLava = false;
		public bool onWater = false;
		public bool inWall = false;
	}
}
