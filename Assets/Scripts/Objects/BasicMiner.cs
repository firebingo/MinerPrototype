using UnityEngine;
using System.Collections;

public class BasicMiner : Miner
{
    Color origColor;

    protected override void Start()
    {
        gameMaster = FindObjectOfType<GameController>();

        Material objMat = GetComponentInChildren<Renderer>().material;
        origColor = objMat.GetColor("_Color");
    }

    protected override void Update()
    {
        if(selected)
        {
            if (Input.GetMouseButtonDown(1))
            {
                moveTo();
            }
        }
        if (selected && selectionChange)
        {
            Material objMat = GetComponentInChildren<Renderer>().material;
            objMat.SetColor("_Color", new Color(0.15f, 1.0f, 0.0f, origColor.a));

            selectionChange = false;
        }
        else if(selectionChange)
        {
            Material objMat = GetComponentInChildren<Renderer>().material;
            objMat.SetColor("_Color", origColor);
            selectionChange = false;
        }
        
    }
}
