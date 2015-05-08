using UnityEngine;
using System.Collections;

public class mapScript : MonoBehaviour 
{

	public static int width;
	public static int height;
	public mapSquare[,] map;

    //updates the whole map, very slow process
    public void updateMap()
    {
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                if(map[i,j].terrainType != 2)
                    map[i, j].updateObjects();
            }
        }
    }

	/*

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	*/
}
