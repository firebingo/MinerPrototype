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
        if (Input.GetMouseButtonDown(0))
        {
            selection();
        }
        if (selected)
        {
            Material objMat = GetComponentInChildren<Renderer>().material;
            objMat.SetColor("_Color", new Color(0.15f, 1.0f, 0.0f, origColor.a));

            if (Input.GetMouseButtonDown(1))
            {
                moveTo();
            }
        }
        else
        {
            Material objMat = GetComponentInChildren<Renderer>().material;
            objMat.SetColor("_Color", origColor);
        }
        
    }
}
