using UnityEngine;
using System.Collections;

public class map1 : mapScript
{

	void createMap()
	{
		//creates the map
		for(int i = 0; i < width; ++i)
		{
			for(int j = 0;j < height; ++j)
			{
				map[i,j] = gameObject.AddComponent<mapSquare>();
			}
		}
		
		//sets the parentMap of all the squares to this map
		for(int i  = 0; i < width; i++)
		{
			for(int j = 0;j < height;j++)
			{
				map[i,j].parentMap = this;
			}
		}
		
		//Sets all map squares to floors
		for(int i = 0; i < width; ++i)
		{
			for(int j = 0;j < height; ++j)
			{
				map[i,j].terrainType = 1;
				map[i,j].xPos = i;
				map[i,j].zPos = j;
			}
		}
		
		//changes the edges of the map to roofs
		for (int i = 0; i < height; ++i)
		{
			map[0,i].terrainType = 2;
			map[0,i].xPos = 0;
			map[0,i].zPos = i;
			map[width-1,i].terrainType = 2;
			map[width-1,i].xPos = width-1;
			map[width-1,i].zPos = i;
		}
		for (int i = 0; i < width; ++i)
		{
			map[i,0].terrainType = 2;
			map[i,0].xPos = i;
			map[i,0].zPos = 0;
			map[i,height-1].terrainType = 2;
			map[i,height-1].xPos = i;
			map[i,height-1].zPos = height-1;
		}
		
		//changes the inner edges to walls
		for (int i = 1; i < height-1; ++i)
		{
			map[1,i].terrainType = 3;
			map[1,i].xPos = 1;
			map[1,i].zPos = i;
			map[width-2,i].terrainType = 3;
			map[width-2,i].xPos = width-2;
			map[width-2,i].zPos = i;
		}
		for (int i = 1; i < width-1; ++i)
		{
			map[i,1].terrainType = 3;
			map[i,1].xPos = i;
			map[i,1].zPos = 1;
			map[i,height-2].terrainType = 3;
			map[i,height-2].xPos = i;
			map[i,height-2].zPos = height-2;
		}
		
		// manual changing of map squares
		map[3,5].terrainType = 3;
		map[3,5].xPos = 3;
		map[3,5].zPos = 5;
		
		map[6,6].terrainType = 3;
		map[6,6].xPos = 6;
		map[6,6].zPos = 6;
		
		map[5,3].terrainType = 3;
		map[5,3].xPos = 5;
		map[5,3].zPos = 3;
		
		map[5,4].terrainType = 3;
		map[5,4].xPos = 5;
		map[5,4].zPos = 4;
	}

	void deleteMap()
	{
		for(int i =0; i< width; ++i)
		{
			for(int j = 0; j< height;++j)
			{
				map[i,j].terrainType = 0;
			}
		}
	}

	void updateMap()
	{
		for(int i = 0; i< width; ++i)
		{
			for(int j = 0; j< height;++j)
			{
				map[i,j].terrainType = 0;
			}
		}
	}

	// Use this for initialization
	void Start () 
	{
		width = 25;
		height = 25;
		map = new mapSquare[width,height];

		createMap ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey(KeyCode.G))
		{
			Debug.Log ("test");
			deleteMap ();
		}
	}
}
