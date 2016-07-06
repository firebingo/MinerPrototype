using UnityEngine;
using System.Collections;

public class mapScript : MonoBehaviour
{
    public static int width;
    public static int height;
    public mapSquare[,] map;

    public GameController gameMaster;

    //updates the whole map, very slow process
    public void updateMap()
    {
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                if (map[i, j].terrainType != terrainType.floor)
                    map[i, j].updateObjects();
            }
        }
    }

    //only updates squares in the area around a changed wall, faster than changing the whole map.
    public void updateSquare(int x, int z)
    {
        //map[x, z].updateObjects();
        for (int i = 0; i < 4; ++i)
        {
            if (map[x, z].neighbors[i])
                map[x, z].neighbors[i].updateObjects();

            for (int j = 0; j < 4; ++j)
            {
                if (map[x, z].neighbors[i])
                    if (map[x, z].neighbors[i].neighbors[j])
                        map[x, z].neighbors[i].neighbors[j].updateObjects();
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
