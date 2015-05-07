using UnityEngine;
using System.Collections;

public class WallInfo : Entity
{
    // List of terrain types
    // 0 = void
    // 1 = floor
    // 2 = roof
    // 3 = softrock
    // 4 = looserock
    // 5 = hardrock
    // 6 = solidrock

    protected override void Start()
    {
       
    }

    protected override void Update()
    {

    } 

    public int xPos;
    public int zPos;
    public int terrainType;
    public int tileValue;
    public mapSquare parentSquare;
}
