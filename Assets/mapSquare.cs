using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mapSquare : MonoBehaviour
{

    // List of terrain types
    // 0 = void
    // 1 = floor
    // 2 = roof
    // 3 = softrock
    // 4 = looserock
    // 5 = hardrock
    // 6 = solidrock

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

    public int crystalCount;

    public GameController gameMaster;

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
            objectType = (GameObject)Resources.Load("Walls/floor");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
        }
        else if (terrainType == 2)
        {
            objectType = (GameObject)Resources.Load("Walls/roof");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 1, zPos), Quaternion.Euler(-90, 0, 0));
        }
        //SoftRock
        else if (terrainType == 3)
        {
            //the tilemapping works in a 255 number system counting from clockwise starting at the x+ neighbor.
            // information on this can be found here: http://www.saltgames.com/2010/a-bitwise-method-for-applying-tilemaps/ (the 255 system was from the comments)
            if (neighbors[0])
                if (neighbors[0].terrainType >= 3 && neighbors[0].terrainType <= 6)
                    tileValue += 1;

            if (xPos + 1 < mapScript.height && zPos - 1 >= 0)
                if (parentMap.map[xPos + 1, zPos - 1].terrainType >= 3 && parentMap.map[xPos + 1, zPos - 1].terrainType <= 6)
                    tileValue += 2;

            if (neighbors[3])
                if (neighbors[3].terrainType >= 3 && neighbors[3].terrainType <= 6)
                    tileValue += 4;

            if (xPos - 1 >= 0 && zPos - 1 >= 0)
                if (parentMap.map[xPos - 1, zPos - 1].terrainType >= 3 && parentMap.map[xPos - 1, zPos - 1].terrainType <= 6)
                    tileValue += 8;

            if (neighbors[2])
                if (neighbors[2].terrainType >= 3 && neighbors[2].terrainType <= 6)
                    tileValue += 16;

            if (xPos - 1 >= 0 && zPos + 1 < mapScript.width)
                if (parentMap.map[xPos - 1, zPos + 1].terrainType >= 3 && parentMap.map[xPos - 1, zPos + 1].terrainType <= 6)
                    tileValue += 32;

            if (neighbors[1])
                if (neighbors[1].terrainType >= 3 && neighbors[1].terrainType <= 6)
                    tileValue += 64;

            if (xPos + 1 < mapScript.height && zPos + 1 < mapScript.width)
                if (parentMap.map[xPos + 1, zPos + 1].terrainType >= 3 && parentMap.map[xPos + 1, zPos + 1].terrainType <= 6)
                    tileValue += 128;

            //after the tiles value is determined it then goes through and defines what wall it should use for each necessary number.
            if (tileValue == 0 || tileValue == 2 || tileValue == 8 || tileValue == 10 || tileValue == 32 || tileValue == 34 || tileValue == 40 || tileValue == 42 || tileValue == 128 || tileValue == 130
                || tileValue == 136 || tileValue == 138 || tileValue == 160 || tileValue == 162 || tileValue == 168 || tileValue == 170)
                createPyramid(3);
 
            else if (tileValue == 1 || tileValue == 3 || tileValue == 9 || tileValue == 11 || tileValue == 33 || tileValue == 35 || tileValue == 41 || tileValue == 43 
                || tileValue == 129 || tileValue == 131 || tileValue == 137 || tileValue == 139 || tileValue == 161 || tileValue == 163 || tileValue == 169 || tileValue == 171)
                createPyramidAngle(3, -90);
 
            else if (tileValue == 4 || tileValue == 6 || tileValue == 12 || tileValue == 14 || tileValue == 36 || tileValue == 38 || tileValue == 44 || tileValue == 46 || tileValue == 132 || tileValue == 134 
                || tileValue == 140 || tileValue == 142 || tileValue == 164 || tileValue == 166 || tileValue == 172 || tileValue == 174)
                createPyramidAngle(3, 0);

            else if (tileValue == 5 || tileValue == 13 || tileValue == 37 || tileValue == 45 || tileValue == 133 || tileValue == 141 || tileValue == 165 || tileValue == 173)
                createPyramidInsideCorner(3, 180);

            else if (tileValue == 7 || tileValue == 15 || tileValue == 39 || tileValue == 47 || tileValue == 135 || tileValue == 143 || tileValue == 167 || tileValue == 175)
                createCorner(3, 0);

            else if (tileValue == 16 || tileValue == 18 || tileValue == 24 || tileValue == 26 || tileValue == 48 || tileValue == 50 || tileValue == 56 || tileValue == 58 || tileValue == 144
                || tileValue == 146 || tileValue == 152 || tileValue == 154 || tileValue == 176 || tileValue == 178 || tileValue == 184 || tileValue == 186)
                createPyramidAngle(3, 90);

            else if (tileValue == 17 || tileValue == 19 || tileValue == 25 || tileValue == 27 || tileValue == 49 || tileValue == 51 || tileValue == 57 || tileValue == 59 || tileValue == 145
                || tileValue == 147 || tileValue == 153 || tileValue == 155 || tileValue == 177 || tileValue == 179 || tileValue == 185 || tileValue == 187)
                createWedge(3, 0);

            else if (tileValue == 20 || tileValue == 22 || tileValue == 52 || tileValue == 54 || tileValue == 148 || tileValue == 150 || tileValue == 180 || tileValue == 182)
                createPyramidInsideCorner(3, -90);

            else if (tileValue == 21 || tileValue == 53 || tileValue == 149 || tileValue == 181)
                createTCorner(3, 180);

            else if (tileValue == 23 || tileValue == 55 || tileValue == 151 || tileValue == 183)
                createTICorner(3, 180, true);
 
            else if (tileValue == 28 || tileValue == 30 || tileValue == 60 || tileValue == 62 || tileValue == 156 || tileValue == 158 || tileValue == 188 || tileValue == 190)
                createCorner(3, 90);

            else if (tileValue == 29 || tileValue == 61 || tileValue == 157 || tileValue == 189)
                createTICorner(3, 180, false);

            else if (tileValue == 31 || tileValue == 63 || tileValue == 159 || tileValue == 191)
                createBase(3, 0);

            else if (tileValue == 64 || tileValue == 66 || tileValue == 72 || tileValue == 74 || tileValue == 96 || tileValue == 98 || tileValue == 104 || tileValue == 106 || tileValue == 192
                || tileValue == 194 || tileValue == 200 || tileValue == 202 || tileValue == 224 || tileValue == 226 || tileValue == 232 || tileValue == 234)
                createPyramidAngle(3, 180);

            else if (tileValue == 65 || tileValue == 67 || tileValue == 73 || tileValue == 75 || tileValue == 97 || tileValue == 99 || tileValue == 105 || tileValue == 107)
                createPyramidInsideCorner(3, 90);

            else if (tileValue == 68 || tileValue == 70 || tileValue == 76 || tileValue == 78 || tileValue == 100 || tileValue == 102 || tileValue == 108 || tileValue == 110 || tileValue == 196 
                || tileValue == 198 || tileValue == 204 || tileValue == 206 || tileValue == 228 || tileValue == 230 || tileValue == 236 || tileValue == 238)
                createWedge(3, 90);

            else if (tileValue == 69 || tileValue == 77 || tileValue == 101 || tileValue == 109)
                createTCorner(3, 90);

            else if (tileValue == 84 || tileValue == 86 || tileValue == 212 || tileValue == 214)
                createTCorner(3, -90);
 
            else if (tileValue == 80 || tileValue == 82 || tileValue == 88 || tileValue == 90 || tileValue == 208 || tileValue == 210 || tileValue == 216 || tileValue == 218)
                createPyramidInsideCorner(3, 0);

            else if (tileValue == 71 || tileValue == 79 || tileValue == 103 || tileValue == 111)
                createTICorner(3, 90, false);

            else if (tileValue == 81 || tileValue == 83 || tileValue == 89 || tileValue == 91)
                createTCorner(3, 0);
 
            else if (tileValue == 85)
                createQuadTop(3);

            else if (tileValue == 87)
                createTriangleTop(3, -90);

            else if (tileValue == 92 || tileValue == 94 || tileValue == 220 || tileValue == 222)
                createTICorner(3, -90, true);
  
            else if (tileValue == 93)
                createTriangleTop(3, 0);
  
            else if (tileValue == 95)
                createDCorner(3, 0);

            else if (tileValue == 112 || tileValue == 114 || tileValue == 120 || tileValue == 122 || tileValue == 240 || tileValue == 242 || tileValue == 248 || tileValue == 250)
                createCorner(3, 180);

            else if (tileValue == 113 || tileValue == 115 || tileValue == 121 || tileValue == 123)
                createTICorner(3, 0, true);

            else if (tileValue == 116 || tileValue == 118 || tileValue == 244 || tileValue == 246)
                createTICorner(3, -90, false);

            else if (tileValue == 117)
                createTriangleTop(3, 90);

            else if (tileValue == 119)
                createValleyCorner(3, 90);

            else if (tileValue == 124 || tileValue == 126 || tileValue == 252 || tileValue == 254)
                createBase(3, 90);

            else if (tileValue == 125)
                createDCorner(3, 90);

            else if (tileValue == 127)
                createICorner(3, 90);

            else if (tileValue == 193 || tileValue == 195 || tileValue == 201 || tileValue == 203 || tileValue == 225 || tileValue == 227 || tileValue == 233 || tileValue == 235)
                createCorner(3, -90);

            else if (tileValue == 197 || tileValue == 205 || tileValue == 229 || tileValue == 237)
                createTICorner(3, 90, true);

            else if (tileValue == 199 || tileValue == 207 || tileValue == 231 || tileValue == 239)
                createBase(3, -90);

            else if (tileValue == 209 || tileValue == 211 || tileValue == 217 || tileValue == 219)
                createTICorner(3, 0, false);

            else if (tileValue == 213)
                createTriangleTop(3, 180);

            else if (tileValue == 215)
                createDCorner(3, -90);

            else if (tileValue == 221)
                createValleyCorner(3, 0);

            else if (tileValue == 223)
                createICorner(3, 0);
 
            else if (tileValue == 241 || tileValue == 243 || tileValue == 249 || tileValue == 251)
                createBase(3, 180);

            else if (tileValue == 245)
                createDCorner(3, 180);

            else if (tileValue == 247)
                createICorner(3, -90);

            else if (tileValue == 253)
                createICorner(3, 180);

            else if (tileValue == 255)
                createRoof(3);
            else
            {
                objectType = (GameObject)Resources.Load("Walls/roof");
                objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 1, zPos), Quaternion.Euler(0, 0, 0));
            }
            tempTileValue = tileValue;
            tileValue = 0;
        }

        //Solid Rock
        else if (terrainType == 6)
        {
            //the tilemapping works in a 255 number system counting from clockwise starting at the x+ neighbor.
            // information on this can be found here: http://www.saltgames.com/2010/a-bitwise-method-for-applying-tilemaps/ (the 255 system was from the comments)
            if (neighbors[0])
                if (neighbors[0].terrainType >= 3 && neighbors[0].terrainType <= 6)
                    tileValue += 1;

            if (xPos + 1 < mapScript.height && zPos - 1 >= 0)
                if (parentMap.map[xPos + 1, zPos - 1].terrainType >= 3 && parentMap.map[xPos + 1, zPos - 1].terrainType <= 6)
                    tileValue += 2;

            if (neighbors[3])
                if (neighbors[3].terrainType >= 3 && neighbors[3].terrainType <= 6)
                    tileValue += 4;

            if (xPos - 1 >= 0 && zPos - 1 >= 0)
                if (parentMap.map[xPos - 1, zPos - 1].terrainType >= 3 && parentMap.map[xPos - 1, zPos - 1].terrainType <= 6)
                    tileValue += 8;

            if (neighbors[2])
                if (neighbors[2].terrainType >= 3 && neighbors[2].terrainType <= 6)
                    tileValue += 16;

            if (xPos - 1 >= 0 && zPos + 1 < mapScript.width)
                if (parentMap.map[xPos - 1, zPos + 1].terrainType >= 3 && parentMap.map[xPos - 1, zPos + 1].terrainType <= 6)
                    tileValue += 32;

            if (neighbors[1])
                if (neighbors[1].terrainType >= 3 && neighbors[1].terrainType <= 6)
                    tileValue += 64;

            if (xPos + 1 < mapScript.height && zPos + 1 < mapScript.width)
                if (parentMap.map[xPos + 1, zPos + 1].terrainType >= 3 && parentMap.map[xPos + 1, zPos + 1].terrainType <= 6)
                    tileValue += 128;

            //after the tiles value is determined it then goes through and defines what wall it should use for each necessary number.
            if (tileValue == 0 || tileValue == 2 || tileValue == 8 || tileValue == 10 || tileValue == 32 || tileValue == 34 || tileValue == 40 || tileValue == 42 || tileValue == 128 || tileValue == 130
                || tileValue == 136 || tileValue == 138 || tileValue == 160 || tileValue == 162 || tileValue == 168 || tileValue == 170)
                createPyramid(6);

            else if (tileValue == 1 || tileValue == 3 || tileValue == 9 || tileValue == 11 || tileValue == 33 || tileValue == 35 || tileValue == 41 || tileValue == 43
                || tileValue == 129 || tileValue == 131 || tileValue == 137 || tileValue == 139 || tileValue == 161 || tileValue == 163 || tileValue == 169 || tileValue == 171)
                createPyramidAngle(6, -90);

            else if (tileValue == 4 || tileValue == 6 || tileValue == 12 || tileValue == 14 || tileValue == 36 || tileValue == 38 || tileValue == 44 || tileValue == 46 || tileValue == 132 || tileValue == 134
                || tileValue == 140 || tileValue == 142 || tileValue == 164 || tileValue == 166 || tileValue == 172 || tileValue == 174)
                createPyramidAngle(6, 0);

            else if (tileValue == 5 || tileValue == 13 || tileValue == 37 || tileValue == 45 || tileValue == 133 || tileValue == 141 || tileValue == 165 || tileValue == 173)
                createPyramidInsideCorner(6, 180);

            else if (tileValue == 7 || tileValue == 15 || tileValue == 39 || tileValue == 47 || tileValue == 135 || tileValue == 143 || tileValue == 167 || tileValue == 175)
                createCorner(6, 0);

            else if (tileValue == 16 || tileValue == 18 || tileValue == 24 || tileValue == 26 || tileValue == 48 || tileValue == 50 || tileValue == 56 || tileValue == 58 || tileValue == 144
                || tileValue == 146 || tileValue == 152 || tileValue == 154 || tileValue == 176 || tileValue == 178 || tileValue == 184 || tileValue == 186)
                createPyramidAngle(6, 90);

            else if (tileValue == 17 || tileValue == 19 || tileValue == 25 || tileValue == 27 || tileValue == 49 || tileValue == 51 || tileValue == 57 || tileValue == 59 || tileValue == 145
                || tileValue == 147 || tileValue == 153 || tileValue == 155 || tileValue == 177 || tileValue == 179 || tileValue == 185 || tileValue == 187)
                createWedge(6, 0);

            else if (tileValue == 20 || tileValue == 22 || tileValue == 52 || tileValue == 54 || tileValue == 148 || tileValue == 150 || tileValue == 180 || tileValue == 182)
                createPyramidInsideCorner(6, -90);

            else if (tileValue == 21 || tileValue == 53 || tileValue == 149 || tileValue == 181)
                createTCorner(6, 180);

            else if (tileValue == 23 || tileValue == 55 || tileValue == 151 || tileValue == 183)
                createTICorner(6, 180, true);

            else if (tileValue == 28 || tileValue == 30 || tileValue == 60 || tileValue == 62 || tileValue == 156 || tileValue == 158 || tileValue == 188 || tileValue == 190)
                createCorner(6, 90);

            else if (tileValue == 29 || tileValue == 61 || tileValue == 157 || tileValue == 189)
                createTICorner(6, 180, false);

            else if (tileValue == 31 || tileValue == 63 || tileValue == 159 || tileValue == 191)
                createBase(6, 0);

            else if (tileValue == 64 || tileValue == 66 || tileValue == 72 || tileValue == 74 || tileValue == 96 || tileValue == 98 || tileValue == 104 || tileValue == 106 || tileValue == 192
                || tileValue == 194 || tileValue == 200 || tileValue == 202 || tileValue == 224 || tileValue == 226 || tileValue == 232 || tileValue == 234)
                createPyramidAngle(6, 180);

            else if (tileValue == 65 || tileValue == 67 || tileValue == 73 || tileValue == 75 || tileValue == 97 || tileValue == 99 || tileValue == 105 || tileValue == 107)
                createPyramidInsideCorner(6, 90);

            else if (tileValue == 68 || tileValue == 70 || tileValue == 76 || tileValue == 78 || tileValue == 100 || tileValue == 102 || tileValue == 108 || tileValue == 110 || tileValue == 196
                || tileValue == 198 || tileValue == 204 || tileValue == 206 || tileValue == 228 || tileValue == 230 || tileValue == 236 || tileValue == 238)
                createWedge(6, 90);

            else if (tileValue == 69 || tileValue == 77 || tileValue == 101 || tileValue == 109)
                createTCorner(6, 90);

            else if (tileValue == 84 || tileValue == 86 || tileValue == 212 || tileValue == 214)
                createTCorner(6, -90);

            else if (tileValue == 80 || tileValue == 82 || tileValue == 88 || tileValue == 90 || tileValue == 208 || tileValue == 210 || tileValue == 216 || tileValue == 218)
                createPyramidInsideCorner(6, 0);

            else if (tileValue == 71 || tileValue == 79 || tileValue == 103 || tileValue == 111)
                createTICorner(6, 90, false);

            else if (tileValue == 81 || tileValue == 83 || tileValue == 89 || tileValue == 91)
                createTCorner(6, 0);

            else if (tileValue == 85)
                createQuadTop(6);

            else if (tileValue == 87)
                createTriangleTop(6, -90);

            else if (tileValue == 92 || tileValue == 94 || tileValue == 220 || tileValue == 222)
                createTICorner(6, -90, true);

            else if (tileValue == 93)
                createTriangleTop(6, 0);

            else if (tileValue == 95)
                createDCorner(6, 0);

            else if (tileValue == 112 || tileValue == 114 || tileValue == 120 || tileValue == 122 || tileValue == 240 || tileValue == 242 || tileValue == 248 || tileValue == 250)
                createCorner(6, 180);

            else if (tileValue == 113 || tileValue == 115 || tileValue == 121 || tileValue == 123)
                createTICorner(6, 0, true);

            else if (tileValue == 116 || tileValue == 118 || tileValue == 244 || tileValue == 246)
                createTICorner(6, -90, false);

            else if (tileValue == 117)
                createTriangleTop(6, 90);

            else if (tileValue == 119)
                createValleyCorner(6, 90);

            else if (tileValue == 124 || tileValue == 126 || tileValue == 252 || tileValue == 254)
                createBase(6, 90);

            else if (tileValue == 125)
                createDCorner(6, 90);

            else if (tileValue == 127)
                createICorner(6, 90);

            else if (tileValue == 193 || tileValue == 195 || tileValue == 201 || tileValue == 203 || tileValue == 225 || tileValue == 227 || tileValue == 233 || tileValue == 235)
                createCorner(6, -90);

            else if (tileValue == 197 || tileValue == 205 || tileValue == 229 || tileValue == 237)
                createTICorner(6, 90, true);

            else if (tileValue == 199 || tileValue == 207 || tileValue == 231 || tileValue == 239)
                createBase(6, -90);

            else if (tileValue == 209 || tileValue == 211 || tileValue == 217 || tileValue == 219)
                createTICorner(6, 0, false);

            else if (tileValue == 213)
                createTriangleTop(6, 180);

            else if (tileValue == 215)
                createDCorner(6, -90);

            else if (tileValue == 221)
                createValleyCorner(6, 0);

            else if (tileValue == 223)
                createICorner(6, 0);

            else if (tileValue == 241 || tileValue == 243 || tileValue == 249 || tileValue == 251)
                createBase(6, 180);

            else if (tileValue == 245)
                createDCorner(6, 180);

            else if (tileValue == 247)
                createICorner(6, -90);

            else if (tileValue == 253)
                createICorner(6, 180);

            else if (tileValue == 255)
                createRoof(6);
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
            objectWallInfo.parentSquare = this;
            objectWallInfo.crystalCount = crystalCount;
        }
    }

    void createBase(int terType, int rot)
    {
        if (terType == 3)
        {
            objectType = (GameObject)Resources.Load("Walls/SoftRock/softRock");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
        }
        else if (terType == 6)
        {
            objectType = (GameObject)Resources.Load("Walls/SolidRock/solidRock");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
        }
    }
    void createRoof(int terType)
    {
        if (terType == 3)
        {
            objectType = (GameObject)Resources.Load("Walls/roof");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 1, zPos), Quaternion.Euler(-90, 0, 0));
        }
        else if (terType == 6)
        {
            objectType = (GameObject)Resources.Load("Walls/roof");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 1, zPos), Quaternion.Euler(-90, 0, 0));
        }
    }
    void createPyramid(int terType)
    {
        if (terType == 3)
        {
            objectType = (GameObject)Resources.Load("Walls/SoftRock/softRockPyramid");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
        }
        else if (terType == 6)
        {
            objectType = (GameObject)Resources.Load("Walls/SolidRock/solidRockPyramid");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
        }
    }
    void createPyramidAngle(int terType, int rot)
    {
        if (terType == 3)
        {
            objectType = (GameObject)Resources.Load("Walls/SoftRock/softRockPyramidAngle");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
        }
        else if (terType == 6)
        {
            objectType = (GameObject)Resources.Load("Walls/SolidRock/solidRockPyramidAngle");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
        }
    }
    void createWedge(int terType, int rot)
    {
        if (terType == 3)
        {
            objectType = (GameObject)Resources.Load("Walls/SoftRock/softRockWedge");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
        }
        else if (terType == 6)
        {
            objectType = (GameObject)Resources.Load("Walls/SolidRock/solidRockWedge");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
        }
    }
    void createCorner(int terType, int rot)
    {
        if (terType == 3)
        {
            objectType = (GameObject)Resources.Load("Walls/SoftRock/softRockCorner");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
        }
        else if (terType == 6)
        {
            objectType = (GameObject)Resources.Load("Walls/SolidRock/solidRockCorner");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
        }
    }
    void createICorner(int terType, int rot)
    {
        if (terType == 3)
        {
            objectType = (GameObject)Resources.Load("Walls/SoftRock/softRockICorner");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
        }
        else if (terType == 6)
        {
            objectType = (GameObject)Resources.Load("Walls/SolidRock/solidRockICorner");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
        }
    }
    void createDCorner(int terType, int rot)
    {
        if (terType == 3)
        {
            objectType = (GameObject)Resources.Load("Walls/SoftRock/softRockDCorner");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
        }
        else if (terType == 6)
        {
            objectType = (GameObject)Resources.Load("Walls/SolidRock/solidRockDCorner");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
        }
    }
    void createTICorner(int terType, int rot, bool flipped)
    {
        if (terType == 3)
        {
            objectType = (GameObject)Resources.Load("Walls/SoftRock/softRockTICorner");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
            if (flipped)
                objectStore.transform.localScale = new Vector3(-objectStore.transform.localScale.x, objectStore.transform.localScale.y, objectStore.transform.localScale.z);
        }
        else if (terType == 6)
        {
            objectType = (GameObject)Resources.Load("Walls/SolidRock/solidRockTICorner");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
            if (flipped)
                objectStore.transform.localScale = new Vector3(-objectStore.transform.localScale.x, objectStore.transform.localScale.y, objectStore.transform.localScale.z);
        }
    }
    void createValleyCorner(int terType, int rot)
    {
        if (terType == 3)
        {
            objectType = (GameObject)Resources.Load("Walls/SoftRock/softRockValleyCorner");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
        }
        else if (terType == 6)
        {
            objectType = (GameObject)Resources.Load("Walls/SolidRock/solidRockValleyCorner");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
        }
    }
    void createTCorner(int terType, int rot)
    {
        if (terType == 3)
        {
            objectType = (GameObject)Resources.Load("Walls/SoftRock/softRockTCorner");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
        }
        else if (terType == 6)
        {
            objectType = (GameObject)Resources.Load("Walls/SolidRock/solidRockTCorner");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
        }
    }
    void createTriangleTop(int terType, int rot)
    {
        if (terType == 3)
        {
            objectType = (GameObject)Resources.Load("Walls/SoftRock/softRockTriangleTop");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
        }
        else if (terType == 6)
        {
            objectType = (GameObject)Resources.Load("Walls/SolidRock/solidRockTriangleTop");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
        }
    }
    void createQuadTop(int terType)
    {
        if (terType == 3)
        {
            objectType = (GameObject)Resources.Load("Walls/SoftRock/softRockQuadTop");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
        }
        else if (terType == 6)
        {
            objectType = (GameObject)Resources.Load("Walls/SolidRock/solidRockQuadTop");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, 0, 0));
        }
    }
    void createPyramidInsideCorner(int terType, int rot)
    {
        if (terType == 3)
        {
            objectType = (GameObject)Resources.Load("Walls/SoftRock/softRockPyramidInsideCorner");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
        }
        else if (terType == 6)
        {
            objectType = (GameObject)Resources.Load("Walls/SolidRock/solidRockPyramidInsideCorner");
            objectStore = (GameObject)Instantiate(objectType, new Vector3(xPos, 0, zPos), Quaternion.Euler(-90, rot, 0));
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

        gameMaster = parentMap.gameMaster;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
