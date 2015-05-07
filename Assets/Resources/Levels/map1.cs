using UnityEngine;
using System.Collections;

public class map1 : mapScript
{

	void createMap()
	{
		//creates the map
		for (int i = 0; i < width; ++i)
		{
			for (int j = 0; j < height; ++j)
			{
				map [i, j] = gameObject.AddComponent<mapSquare>();
			}
		}
		
		//sets the parentMap of all the squares to this map
		for (int i  = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				map [i, j].parentMap = this;
			}
		}
		
		//Sets all map squares to floors
		for (int i = 0; i < width; ++i)
		{
			for (int j = 0; j < height; ++j)
			{
				map [i, j].terrainType = 1;
				map [i, j].xPos = i;
				map [i, j].zPos = j;
			}
		}
		
		//changes the edges of the map to walls
		for (int i = 0; i < height; ++i)
		{
			map [0, i].terrainType = 3;
			map [0, i].xPos = 0;
			map [0, i].zPos = i;
			map [width - 1, i].terrainType = 3;
			map [width - 1, i].xPos = width - 1;
			map [width - 1, i].zPos = i;
		}
		for (int i = 0; i < width; ++i)
		{
			map [i, 0].terrainType = 3;
			map [i, 0].xPos = i;
			map [i, 0].zPos = 0;
			map [i, height - 1].terrainType = 3;
			map [i, height - 1].xPos = i;
			map [i, height - 1].zPos = height - 1;
		}

        ////changes the inner edges to walls
        //for (int i = 1; i < height - 1; ++i)
        //{
        //    map[1, i].terrainType = 3;
        //    map[1, i].xPos = 1;
        //    map[1, i].zPos = i;
        //    map[width - 2, i].terrainType = 3;
        //    map[width - 2, i].xPos = width - 2;
        //    map[width - 2, i].zPos = i;
        //}
        //for (int i = 1; i < width - 1; ++i)
        //{
        //    map[i, 1].terrainType = 3;
        //    map[i, 1].xPos = i;
        //    map[i, 1].zPos = 1;
        //    map[i, height - 2].terrainType = 3;
        //    map[i, height - 2].xPos = i;
        //    map[i, height - 2].zPos = height - 2;
        //}
		
		// manual changing of map squares
		map [3, 3].terrainType = 3;
		map [3, 3].xPos = 3;
		map [3, 3].zPos = 3;

		map [3, 5].terrainType = 3;
		map [3, 5].xPos = 3;
		map [3, 5].zPos = 5;

		map [3, 6].terrainType = 3;
		map [3, 6].xPos = 3;
		map [3, 6].zPos = 6;

		map [5, 6].terrainType = 3;
		map [5, 6].xPos = 5;
		map [5, 6].zPos = 6;
		
		map [6, 6].terrainType = 3;
		map [6, 6].xPos = 6;
		map [6, 6].zPos = 6;

		map [5, 3].terrainType = 3;
		map [5, 3].xPos = 5;
		map [5, 3].zPos = 3;

		map [6, 3].terrainType = 3;
		map [6, 3].xPos = 6;
		map [6, 3].zPos = 3;

		map [5, 4].terrainType = 3;
		map [5, 4].xPos = 5;
		map [5, 4].zPos = 4;

		map [6, 4].terrainType = 3;
		map [6, 4].xPos = 6;
		map [6, 4].zPos = 4;

		map [8, 2].terrainType = 3;
		map [8, 2].xPos = 8;
		map [8, 2].zPos = 2;
		map [9, 2].terrainType = 3;
		map [9, 2].xPos = 9;
		map [9, 2].zPos = 2;
		map [2, 9].terrainType = 3;
		map [2, 9].xPos = 2;
		map [2, 9].zPos = 9;
		map [16, 17].terrainType = 3;
		map [16, 17].xPos = 16;
		map [16, 17].zPos = 17;
		map [17, 16].terrainType = 3;
		map [17, 16].xPos = 17;
		map [17, 16].zPos = 16;


		map [4, 8].terrainType = 3;
		map [4, 8].xPos = 4;
		map [4, 8].zPos = 8;
		map [4, 10].terrainType = 3;
		map [4, 10].xPos = 4;
		map [4, 10].zPos = 10;
		map [5, 8].terrainType = 3;
		map [5, 8].xPos = 5;
		map [5, 8].zPos = 8;
		map [5, 9].terrainType = 3;
		map [5, 9].xPos = 5;
		map [5, 9].zPos = 9;
		map [5, 10].terrainType = 3;
		map [5, 10].xPos = 5;
		map [5, 10].zPos = 10;
		map [6, 8].terrainType = 3;
		map [6, 8].xPos = 6;
		map [6, 8].zPos = 8;

		map [9, 5].terrainType = 3;
		map [9, 5].xPos = 9;
		map [9, 5].zPos = 5;
		map [10, 5].terrainType = 3;
		map [10, 5].xPos = 10;
		map [10, 5].zPos = 5;
		map [11, 4].terrainType = 3;
		map [11, 4].xPos = 11;
		map [11, 4].zPos = 4;
		map [11, 5].terrainType = 3;
		map [11, 5].xPos = 11;
		map [11, 5].zPos = 5;
		map [10, 7].terrainType = 3;
		map [10, 7].xPos = 10;
		map [10, 7].zPos = 7;
		map [9, 7].terrainType = 3;
		map [9, 7].xPos = 9;
		map [9, 7].zPos = 7;
		map [9, 8].terrainType = 3;
		map [9, 8].xPos = 9;
		map [9, 8].zPos = 8;
		map [8, 5].terrainType = 3;
		map [8, 5].xPos = 8;
		map [8, 5].zPos = 5;
		map [11, 7].terrainType = 3;
		map [11, 7].xPos = 11;
		map [11, 7].zPos = 7;
		map [9, 6].terrainType = 3;
		map [9, 6].xPos = 9;
		map [9, 6].zPos = 6;
		map [11, 6].terrainType = 3;
		map [11, 6].xPos = 11;
		map [11, 6].zPos = 6;
		map [10, 6].terrainType = 3;
		map [10, 6].xPos = 10;
		map [10, 6].zPos = 6;
		map [12, 7].terrainType = 3;
		map [12, 7].xPos = 12;
		map [12, 7].zPos = 7;
		map [10, 4].terrainType = 3;
		map [10, 4].xPos = 10;
		map [10, 4].zPos = 4;
		map [9, 4].terrainType = 3;
		map [9, 4].xPos = 9;
		map [9, 4].zPos = 4;

	}

	void deleteMap()
	{
		for (int i =0; i< width; ++i)
		{
			for (int j = 0; j< height; ++j)
			{
				map [i, j].terrainType = 0;
			}
		}
	}

    ////updates the whole map, very slow process
    //public void updateMap()
    //{
    //    for (int i = 0; i< width; ++i)
    //    {
    //        for (int j = 0; j< height; ++j)
    //        {
    //            map [i, j].updateObjects();
    //        }
    //    }
    //}

	//only updates squares in the area around a changed wall, faster than changing the whole map.
	void updateSquare(int x, int z)
	{
		//map[x, z].updateObjects();
		for(int i = 0; i < 4; ++i)
		{
			map[x,z].neighbors[i].updateObjects();
			for(int j = 0; j < 4; ++j)
			{
				map[x,z].neighbors[i].neighbors[j].updateObjects();
			}
		}
	}

	// Use this for initialization
	void Start()
	{
		width = 20;
		height = 20;
		map = new mapSquare[width, height];

		createMap();
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKey(KeyCode.G))
		{
			Debug.Log("Map Deleted");
			deleteMap();
			updateMap();
		}
		if (Input.GetKey(KeyCode.H))
		{
			Debug.Log("Map Created");
			deleteMap();
			updateMap();
			createMap();
			updateMap();
		}
		if (Input.GetKey(KeyCode.J))
		{
			Debug.Log("Map Updated");
			updateMap();
		}
		if (Input.GetKey(KeyCode.K))
		{
			Debug.Log("Square 10,6 changed to floor");
			map[10,6].terrainType = 1;
			updateSquare(10, 6);
		}
	}
}
