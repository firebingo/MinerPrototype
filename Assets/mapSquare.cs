using UnityEngine;
using System.Collections;

public class mapSquare : MonoBehaviour 
{

	// List of terrain types
	// 0 = void
	// 1 = floor
	// 2 = roof
	// 3 = softrock

	public int terrainType = 0;
	GameObject objectType = null;
	GameObject objectStore = null;
	public int xPos, zPos = 0;

	public mapScript parentMap = null;
	//List to contain neighbors of the map
	// 0 = x+ 1 = z+ 2 = x- 3 = z-
	public mapSquare[] neighbors = new mapSquare[4];

	bool hidden = false;

	void updateObjects()
	{
		if(objectStore != null)
		{
			Destroy(objectStore);
		}

		//neighbors[0]
		if(hidden)
		{
			objectType = (GameObject)Resources.Load("Walls/roof");
			objectStore = (GameObject)Instantiate(objectType,new Vector3(xPos,1,zPos),Quaternion.Euler(-90,0,0));
		}
		else if (terrainType == 1)
		{
			objectType = (GameObject)Resources.Load("Walls/roof");
			objectStore = (GameObject)Instantiate(objectType,new Vector3(xPos,0,zPos),Quaternion.Euler(-90,0,0));
		}
		else if (terrainType == 2)
		{
			objectType = (GameObject)Resources.Load("Walls/roof");
			objectStore = (GameObject)Instantiate(objectType,new Vector3(xPos,1,zPos),Quaternion.Euler(-90,0,0));
		}
		else if (terrainType == 3)
		{
			//this contains all possible rules for creating walls and their corners
			//if all neighbors are floors, make a single standing pyramid wall.
			if(neighbors[0].terrainType == 1 && neighbors[1].terrainType == 1 && neighbors[2].terrainType == 1 && neighbors[3].terrainType == 1)
			{
				objectType = (GameObject)Resources.Load("Walls/softRockPyramid");
				objectStore = (GameObject)Instantiate(objectType,new Vector3(xPos,0,zPos),Quaternion.Euler(-90,0,0));
			}
			
			//if x+ and x- are walls make a straight wall parallel to them
			else if(neighbors[0].terrainType == 3 && neighbors[2].terrainType == 3)
			{
				//if z+ is a floor make the wall rotated towards the floor
				if(neighbors[1].terrainType == 1)
				{
					objectType = (GameObject)Resources.Load("Walls/softRock");
					objectStore = (GameObject)Instantiate(objectType,new Vector3(xPos,0,zPos),Quaternion.Euler(-90,0,0));
				}
				//if z- is a floor make the wall rotated towards the floor
				if(neighbors[3].terrainType == 1)
				{
					objectType = (GameObject)Resources.Load("Walls/softRock");
					objectStore = (GameObject)Instantiate(objectType,new Vector3(xPos,0,zPos),Quaternion.Euler(-90,180,0));
				}
			}
			//if z+ and z- are walls make a straight wall parallel to them
			else if(neighbors[1].terrainType == 3 && neighbors[3].terrainType == 3)
			{
				//if x+ is a floor make the wall rotated towards the floor
				if(neighbors[0].terrainType == 1)
				{
					objectType = (GameObject)Resources.Load("Walls/softRock");
					objectStore = (GameObject)Instantiate(objectType,new Vector3(xPos,0,zPos),Quaternion.Euler(-90,90,0));
				}
				//if x- is a floor make the wall rotated towards the floor
				if(neighbors[2].terrainType == 1)
				{
					objectType = (GameObject)Resources.Load("Walls/softRock");
					objectStore = (GameObject)Instantiate(objectType,new Vector3(xPos,0,zPos),Quaternion.Euler(-90,-90,0));
				}
			}
			//if x+ is a wall and z+ is a wall and x- and z- is a roof make a inside corner.
			else if(neighbors[0].terrainType == 3 && neighbors[1].terrainType == 3 && neighbors[2].terrainType == 2 && neighbors[3].terrainType == 2)
			{
				objectType = (GameObject)Resources.Load("Walls/softRockICorner");
				objectStore = (GameObject)Instantiate(objectType,new Vector3(xPos,0,zPos),Quaternion.Euler(-90,90,0));
			}
			//if x- is a wall and z- is a wall and x+ and z+ is a roof make a inside corner.
			else if(neighbors[2].terrainType == 3 && neighbors[3].terrainType == 3 && neighbors[0].terrainType == 2 && neighbors[1].terrainType == 2)
			{
				objectType = (GameObject)Resources.Load("Walls/softRockICorner");
				objectStore = (GameObject)Instantiate(objectType,new Vector3(xPos,0,zPos),Quaternion.Euler(-90,-90,0));
			}
			//if x+ is a wall and z- is a wall and x- and z+ is a roof make a inside corner.
			else if(neighbors[0].terrainType == 3 && neighbors[3].terrainType == 3 && neighbors[2].terrainType == 2 && neighbors[1].terrainType == 2)
			{
				objectType = (GameObject)Resources.Load("Walls/softRockICorner");
				objectStore = (GameObject)Instantiate(objectType,new Vector3(xPos,0,zPos),Quaternion.Euler(-90,180,0));
			}
			//if x- is a wall and z+ is a wall and x+ and z- is a roof make a inside corner.
			else if(neighbors[2].terrainType == 3 && neighbors[1].terrainType == 3 && neighbors[0].terrainType == 2 && neighbors[3].terrainType == 2)
			{
				objectType = (GameObject)Resources.Load("Walls/softRockICorner");
				objectStore = (GameObject)Instantiate(objectType,new Vector3(xPos,0,zPos),Quaternion.Euler(-90,0,0));
			}
		}
	}

	// Use this for initialization
	void Start () 
	{
		//sets all the neighbors of the squares
		if(xPos != mapScript.width-1)
		{
			neighbors[0] = parentMap.map[xPos+1,zPos];
		}
		if(zPos != mapScript.height-1)
		{
			neighbors[1] = parentMap.map[xPos,zPos+1];
		}
		if(xPos != 0)
		{
			neighbors[2] = parentMap.map[xPos-1,zPos];
		}
		if(zPos != 0)
		{
			neighbors[3] = parentMap.map[xPos,zPos-1];
		}

		updateObjects();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
}
