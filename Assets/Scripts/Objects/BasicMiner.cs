using UnityEngine;
using System.Collections;

public class BasicMiner : Miner
{
    Color origColor;

    public bool drilling;
    Collider drillingZone;

    int orderXPos;

    protected override void Start()
    {
        gameMaster = FindObjectOfType<GameController>();

        Material objMat = GetComponentInChildren<Renderer>().material;
        origColor = objMat.GetColor("_Color");

        doingOrder = false;
    }

    protected override void Update()
    {
        if (selected)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (!doingOrder)
                    moveTo();
            }
        }
        if (selected && selectionChange)
        {
            Material objMat = GetComponentInChildren<Renderer>().material;
            objMat.SetColor("_Color", new Color(0.15f, 1.0f, 0.0f, origColor.a));

            selectionChange = false;
        }
        else if (selectionChange)
        {
            Material objMat = GetComponentInChildren<Renderer>().material;
            objMat.SetColor("_Color", origColor);
            selectionChange = false;
        }
        if (currentOrder != null)
        {
            if (doingOrder == false)
                doOrder();
        }
        else
        {
            findOrder();
        }

    }
    public override void queueOrder()
    {
        throw new System.NotImplementedException();
    }

    public void findOrder()
    {
        if (gameMaster.oQueue.drillQueue.Count > 0)
        {
            currentOrder = gameMaster.oQueue.drillQueue.Dequeue();
            WallInfo tempInfo = currentOrder.parentObject as WallInfo;
            orderXPos = tempInfo.xPos;
        }
    }

    public void doOrder()
    {
        doingOrder = true;
        
        if (currentOrder.orderType == 3)
        {
            if (navAgent == null)
                navAgent = GetComponent<NavMeshAgent>();

            Vector3 shortestZone;
            shortestZone = currentOrder.destination[0];

            for (int i = 0; i < currentOrder.destination.Count; ++i)
            {
                if (Vector3.Distance(transform.position, currentOrder.destination[i]) < Vector3.Distance(transform.position, shortestZone))
                    shortestZone = currentOrder.destination[i];
            }

            navAgent.SetDestination(shortestZone);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "DrillZone" && currentOrder != null && !drilling)
        {
            if (other.gameObject.transform.parent.gameObject.GetComponent<WallInfo>().xPos == orderXPos)
            {
                drillingZone = other;
                drilling = true;
                StartCoroutine(drillTime());
            }
        }
        else if(drilling && other.name == "DrillZone")
        {
            drillingZone = other;
        }
    }

    IEnumerator drillTime()
    {
        yield return new WaitForSeconds(4.0f);
        if (drillingZone.gameObject.transform.parent.gameObject.GetComponent<WallInfo>())
            drillingZone.gameObject.transform.parent.gameObject.GetComponent<WallInfo>().destroyWall();
        currentOrder = null;
        doingOrder = false;
        drilling = false;
    }

}
