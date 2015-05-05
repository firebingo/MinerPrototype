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
    int tileValue;
    int tempTileValue;

    bool hidden = false;

    //this method should only be called when absolutly needed as it is very very slow.
    public void updateObjects()
    {
        if (objectStore != null)
        {
            Destroy(objectStore);
        }

        //neighbors[0]
        if (hidden)
        {
            objectType = (GameObject)Resources.Load("Walls/roof");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 1, zPos), Quaternion.Euler(-90, 0, 0));
        }
        else if (terrainType == 1)
        {
            objectType = (GameObject)Resources.Load("Walls/roof");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
        }
        else if (terrainType == 2)
        {
            objectType = (GameObject)Resources.Load("Walls/roof");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 1, zPos), Quaternion.Euler(-90, 0, 0));
        }
        else if (terrainType == 3)
        {
            //the tilemapping works in a 255 number system counting from clockwise starting at the x+ neighbor.
            // information on this can be found here: http://www.saltgames.com/2010/a-bitwise-method-for-applying-tilemaps/ (the 255 system was from the comments)
            if (neighbors[0].terrainType == 3)
            {
                tileValue += 1;
            }
            if(parentMap.map[xPos+1, zPos-1].terrainType == 3)
            {
                tileValue += 2;
            }
            if (neighbors[3].terrainType == 3)
            {
                tileValue += 4;
            }
            if (parentMap.map[xPos - 1, zPos - 1].terrainType == 3)
            {
                tileValue += 8;
            }
            if (neighbors[2].terrainType == 3)
            {
                tileValue += 16;
            }
            if (parentMap.map[xPos - 1, zPos + 1].terrainType == 3)
            {
                tileValue += 32;
            }
            if (neighbors[1].terrainType == 3)
            {
                tileValue += 64;
            }
            if (parentMap.map[xPos + 1, zPos + 1].terrainType == 3)
            {
                tileValue += 128;
            }

            //after the tiles value is determined it then goes through and defines what wall it should use for each necessary number.
            if(tileValue == 0)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramid");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
            }
            else if (tileValue == 1)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidAngle");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, -90, 0));
            }
            //tileValue 2 unimportant
            else if (tileValue == 3)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidInsideCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, -90, 0));
            }
            else if (tileValue == 4)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidAngle");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
            }
            else if(tileValue == 16)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidAngle");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 90, 0));
            }
            else if (tileValue == 64)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidAngle");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 180, 0));
            }
            else if(tileValue == 255)
            {
                objectType = (GameObject)Resources.Load("Walls/roof");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 1, zPos), Quaternion.Euler(-90, 0, 0));
            }
            else
            {
                objectType = (GameObject)Resources.Load("Walls/roof");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 1, zPos), Quaternion.Euler(0, 0, 0));
            }
            tempTileValue = tileValue;
            tileValue = 0;
        }

        if (objectStore)
        {
            WallInfo objectWallInfo = objectStore.GetComponent<WallInfo>();
            objectWallInfo.xPos = xPos;
            objectWallInfo.zPos = zPos;
            objectWallInfo.terrainType = terrainType;
            objectWallInfo.tileValue = tempTileValue;
        }
    }

    // Use this for initialization
    void Start()
    {
        //sets all the neighbors of the squares
        if (xPos != mapScript.width - 1)
        {
            neighbors[0] = parentMap.map[xPos + 1, zPos];
        }
        if (zPos != mapScript.height - 1)
        {
            neighbors[1] = parentMap.map[xPos, zPos + 1];
        }
        if (xPos != 0)
        {
            neighbors[2] = parentMap.map[xPos - 1, zPos];
        }
        if (zPos != 0)
        {
            neighbors[3] = parentMap.map[xPos, zPos - 1];
        }

        updateObjects();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
