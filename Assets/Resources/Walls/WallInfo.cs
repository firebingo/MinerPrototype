using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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
    public int crystalCount;
    public mapSquare parentSquare;

    public orderQueue oQueue;
    public bool inQueue;
    Miner hasOrder;

    public List<Vector3> drillZones;

    Color origColor;

    protected override void Start()
    {
        selected = false;

        gameMaster = parentSquare.gameMaster;
        oQueue = gameMaster.oQueue;

        Material objMat = GetComponentInChildren<Renderer>().material;
        origColor = objMat.GetColor("_Color");
        selectionChange = false;

        drillZones = new List<Vector3>();
        inQueue = false;
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

        if (selected && Input.GetKeyDown(KeyCode.O))
        {
            queueOrder();
        }
    }

    public void selection()
    {
        selected = true;
        gameMaster.selectedEntity = this;
        //gameMaster.gameUI.transform.FindChild("Drill").gameObject.SetActive(true);
    }

    public override void queueOrder()
    {
        if (!inQueue && terrainType != 1)
        {
            Order drillWall = new Order();
            for (int i = 0; i < transform.childCount; ++i)
            {
                drillZones.Add(transform.GetChild(i).transform.position);
            }
            drillWall.initOrder(3, drillZones, this);

            oQueue.addOrder(drillWall);
            inQueue = true;
        }
    }

    public override void queueOrder(Vector3 destination) { }

    public void destroyWall()
    {
        if(crystalCount > 0)
        {
            for(int i = 0; i < crystalCount; ++i)
            {
                Instantiate(Resources.Load("Objects/Crystal"), new Vector3(Random.Range(transform.position.x - 0.15f, transform.position.x + 0.15f), 0.032f, Random.Range(transform.position.z - 0.15f, transform.position.z + 0.15f)), Quaternion.Euler(0, 0, 0));
            }
        }
        for (int i = 0; i < 3; ++i)
        {
            Instantiate(Resources.Load("Objects/Ore"), new Vector3(Random.Range(transform.position.x - 0.15f, transform.position.x + 0.15f), 0.06f, Random.Range(transform.position.z - 0.15f, transform.position.z + 0.15f)), Quaternion.Euler(0, 0, 0));
        }
        inQueue = false;
        parentSquare.parentMap.map[parentSquare.xPos, parentSquare.zPos].terrainType = 1;
        parentSquare.parentMap.updateSquare(parentSquare.xPos, parentSquare.zPos);
    }

}
