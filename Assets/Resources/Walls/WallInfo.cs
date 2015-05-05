using UnityEngine;
using System.Collections;

public class WallInfo : MonoBehaviour
{
    // List of terrain types
    // 0 = void
    // 1 = floor
    // 2 = roof
    // 3 = softrock

    public int xPos;
    public int zPos;
    public int terrainType;
    public int tileValue;
}
