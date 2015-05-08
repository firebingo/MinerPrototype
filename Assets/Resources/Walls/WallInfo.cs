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

    public int xPos;
    public int zPos;
    public int terrainType;
    public int tileValue;
    public mapSquare parentSquare;

    

    Color origColor;

    protected override void Start()
    {
        selected = false;

        gameMaster = parentSquare.gameMaster;

        Material objMat = GetComponentInChildren<Renderer>().material;
        origColor = objMat.GetColor("_Color");
        selectionChange = false;
    }

    protected override void Update()
    {
        if (terrainType != 6)
        {
            if (selected && selectionChange)
            {
                Material objMat = GetComponentInChildren<Renderer>().material;
                objMat.SetColor("_Color", new Color(origColor.r - 0.2f, origColor.g - 0.2f, origColor.b - 0.2f, origColor.a));
                selectionChange = false;
            }
            else if (selectionChange)
            {
                Material objMat = GetComponentInChildren<Renderer>().material;
                objMat.SetColor("_Color", origColor);
                selectionChange = false;
            }
        }
    }

    public void selection()
    {
        selected = true;
        gameMaster.selectedEntity = this;
    }
}
