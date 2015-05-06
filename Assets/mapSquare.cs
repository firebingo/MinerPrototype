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
            if (parentMap.map[xPos + 1, zPos - 1].terrainType == 3)
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
            if (tileValue == 0 || tileValue == 2 || tileValue == 8 || tileValue == 10 || tileValue == 32 || tileValue == 34 || tileValue == 40 || tileValue == 128 || tileValue == 130 || tileValue == 138 || tileValue == 160 || tileValue == 162 || tileValue == 170)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramid");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
            }
            else if (tileValue == 1 || tileValue == 9 || tileValue == 35 || tileValue == 43 || tileValue == 129 || tileValue == 139 || tileValue == 163 || tileValue == 171)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidAngle");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, -90, 0));
            }
            else if (tileValue == 3 || tileValue == 11 || tileValue == 131)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidAngle");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, -90, 0));
            }
            else if (tileValue == 4 || tileValue == 6 || tileValue == 14 || tileValue == 44 || tileValue == 140 || tileValue == 166)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidAngle");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
            }
            else if (tileValue == 5)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidInsideCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 180, 0));
            }
            else if (tileValue == 7 || tileValue == 15 || tileValue == 37 || tileValue == 39 || tileValue == 135)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
            }
            else if (tileValue == 12)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidAngle");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
            }
            else if (tileValue == 13)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidInsideCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 180, 0));
            }
            else if (tileValue == 16 || tileValue == 18 || tileValue == 24 || tileValue == 26 || tileValue == 48 || tileValue == 56 || tileValue == 58 || tileValue == 144 || tileValue == 152 || tileValue == 154 || tileValue == 178 || tileValue == 184 || tileValue == 186)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidAngle");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 90, 0));
            }
            else if (tileValue == 17 || tileValue == 19 || tileValue == 25 || tileValue == 27 || tileValue == 49 || tileValue == 57 || tileValue == 59 || tileValue == 145 || tileValue == 185)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockWedge");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
            }
            else if (tileValue == 20)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidInsideCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, -90, 0));
            }
            else if (tileValue == 21)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockTCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 180, 0));
            }
            else if (tileValue == 22)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidInsideCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, -90, 0));
            }
            else if (tileValue == 23)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockTICorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 180, 0));
                objectStore.transform.localScale = new Vector3(-objectStore.transform.localScale.x, objectStore.transform.localScale.y, objectStore.transform.localScale.z);
            }
            else if (tileValue == 28 || tileValue == 30 || tileValue == 60 || tileValue == 62 || tileValue == 190)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 90, 0));
            }
            else if (tileValue == 29)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockTICorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 180, 0));
            }
            else if(tileValue == 31 || tileValue == 63)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRock");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
            }
            else if(tileValue == 55)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockTICorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 180, 0));
                objectStore.transform.localScale = new Vector3(-objectStore.transform.localScale.x, objectStore.transform.localScale.y, objectStore.transform.localScale.z);
            }
            else if (tileValue == 64 || tileValue == 66 || tileValue == 96 || tileValue == 104 || tileValue == 106 || tileValue == 192 || tileValue == 194 || tileValue == 224 || tileValue == 226)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidAngle");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 180, 0));
            }
            else if (tileValue == 65)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidInsideCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 90, 0));
            }
            else if(tileValue == 67)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidInsideCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 90, 0));
            }
            else if (tileValue == 68 || tileValue == 70 || tileValue == 76 || tileValue == 100 || tileValue == 108 || tileValue == 196 || tileValue == 206 || tileValue == 236)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockWedge");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 90, 0));
            }
            else if (tileValue == 69)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockTCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 90, 0));
            }
            else if(tileValue == 80)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidInsideCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
            }
            else if (tileValue == 71 || tileValue == 79)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockTICorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 90, 0));
            }
            else if(tileValue == 77)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockTCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 90, 0));
            }
            else if(tileValue == 81)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockTCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
            }
            else if (tileValue == 84)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockTCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, -90, 0));
            }
            else if (tileValue == 87)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockTriangleTop");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, -90, 0));
            }
            else if (tileValue == 92)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockTICorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 90, 0));
                objectStore.transform.localScale = new Vector3(objectStore.transform.localScale.x, -objectStore.transform.localScale.y, objectStore.transform.localScale.z);
            }
            else if(tileValue == 97)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidInsideCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 90, 0));
            }
            else if (tileValue == 102 ||tileValue == 110 || tileValue == 198)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockWedge");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 90, 0));
            }
            else if(tileValue == 103)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockTICorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 90, 0));
            }
            else if (tileValue == 107 || tileValue == 75)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidInsideCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 90, 0));
            }
            else if(tileValue == 112 || tileValue == 114 || tileValue == 120 || tileValue == 122 || tileValue == 248)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 180, 0));
            }
            else if (tileValue == 113)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockTICorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
                objectStore.transform.localScale = new Vector3(-objectStore.transform.localScale.x, objectStore.transform.localScale.y, objectStore.transform.localScale.z);
            }
            else if(tileValue == 116)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockTICorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, -90, 0));
            }
            else if(tileValue == 117)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockTriangleTop");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 90, 0));
            }
            else if( tileValue == 118)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockTICorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, -90, 0));
            }
            else if (tileValue == 119)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockValleyCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 90, 0));
            }
            else if(tileValue == 124 || tileValue == 126 || tileValue == 252)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRock");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 90, 0));
            }
            else if(tileValue == 127)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockICorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 90, 0));
            }
            else if (tileValue == 182)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidInsideCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, -90, 0));
            }
            else if(tileValue == 193 || tileValue == 195 || tileValue == 225)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, -90, 0));
            }
            else if(tileValue == 191)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRock");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
            }
            else if(tileValue == 197)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockTICorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 90, 0));
                objectStore.transform.localScale = new Vector3(-objectStore.transform.localScale.x, objectStore.transform.localScale.y, objectStore.transform.localScale.z);
            }
            else if(tileValue == 199 || tileValue == 207 || tileValue == 231)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRock");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, -90, 0));
            }
            else if (tileValue == 208)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidInsideCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
            }
            else if (tileValue == 209)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockTICorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
            }
            else if (tileValue == 210)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockPyramidInsideCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
            }
            else if (tileValue == 211)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockTICorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
            }
            else if(tileValue == 215)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockDCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, -90, 0));
            }
            else if(tileValue == 223)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockICorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
            }
            else if(tileValue == 240)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockCorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 180, 0));
            }
            else if(tileValue == 241 || tileValue == 249 || tileValue == 251)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRock");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 180, 0));
            }
            else if(tileValue == 247)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockICorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, -90, 0));
            }
            else if (tileValue == 253)
            {
                objectType = (GameObject)Resources.Load("Walls/softRock/softRockICorner");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 180, 0));
            }
            else if (tileValue == 255)
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
