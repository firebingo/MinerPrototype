using UnityEngine;
using System.Collections;

public class BasicMiner : Miner
{
    Color origColor;

    public bool drilling;
    Collider drillingZone;
    public bool carrying;
    Entity carriedObject;

    int orderXPos;
    float orderTime;

    protected override void Start()
    {
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

        if (currentOrder != null && navAgent != null)
        {
            if (navAgent.remainingDistance == 0 && orderTime > 0.3f && currentOrder.orderType == 3 && drilling == false)
            {
                currentOrder = null;
                doingOrder = false;
            }
        }

        if(doingOrder)
            orderTime += Time.deltaTime;
        else
            orderTime = 0;

    }
    public override void queueOrder()
    {
        throw new System.NotImplementedException();
    }

    public void findOrder()
    {
        if (GameController._instance.oQueue.drillQueue.Count > 0)
        {
            currentOrder = GameController._instance.oQueue.drillQueue.Dequeue();
            WallInfo tempInfo = currentOrder.parentObject as WallInfo;
            orderXPos = tempInfo.xPos;
        }
        else if (GameController._instance.oQueue.crystalQueue.Count > 0)
        {
            currentOrder = GameController._instance.oQueue.crystalQueue.Dequeue();
        }
        else if(GameController._instance.oQueue.oreQueue.Count > 0)
        {
            currentOrder = GameController._instance.oQueue.oreQueue.Dequeue();
        }
    }

    public void doOrder()
    {
        doingOrder = true;
        
        if(currentOrder.orderType == 1 && !carrying)
        {
            if (navAgent == null)
            {
                navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
                navSpeed = navAgent.speed;
            }

            navAgent.SetDestination(currentOrder.destination[0]);
        }
        if (currentOrder.orderType == 2 && !carrying)
        {
            if (navAgent == null)
            {
                navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
                navSpeed = navAgent.speed;
            }

            navAgent.SetDestination(currentOrder.destination[0]);
        }

        if (currentOrder.orderType == 3) 
        {
            if (navAgent == null)
            {
                navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
                navSpeed = navAgent.speed;
            }

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
        else if (other.name == "Ore(Clone)" && currentOrder != null)
        {
            if(other.GetComponent<Ore>() == currentOrder.parentObject)
            {
                other.transform.position = new Vector3(transform.FindChild("CarrySpot").transform.position.x, transform.FindChild("CarrySpot").transform.position.y, transform.FindChild("CarrySpot").transform.position.z);
                other.transform.parent = this.gameObject.transform;
                carrying = true;
                carriedObject = other.GetComponent<Ore>();
                navAgent.speed = navSpeed / 2;
                navAgent.SetDestination(GameController._instance.mapStock.transform.position);
            }
        }
        else if (other.name == "Crystal(Clone)" && currentOrder != null)
        {
            if (other.GetComponent<Crystal>() == currentOrder.parentObject)
            {
                other.transform.position = new Vector3(transform.FindChild("CarrySpot").transform.position.x, transform.FindChild("CarrySpot").transform.position.y, transform.FindChild("CarrySpot").transform.position.z);
                other.transform.parent = this.gameObject.transform;
                carrying = true;
                carriedObject = other.GetComponent<Crystal>();
                navAgent.speed = navSpeed / 2;
                navAgent.SetDestination(GameController._instance.mapStock.transform.position);
            }
        }
        else if (other.name == "StockPile" && currentOrder != null && carrying)
        {
            carriedObject.transform.parent = null;
            if (carriedObject.name == "Ore(Clone)")
                carriedObject.transform.position = new Vector3(GameController._instance.mapStock.transform.position.x + Random.Range(-0.3f, 0.3f), 0.06f, GameController._instance.mapStock.transform.position.z + Random.Range(-0.3f, 0.3f));
            if (carriedObject.name == "Crystal(Clone)")
                carriedObject.transform.position = new Vector3(GameController._instance.mapStock.transform.position.x + Random.Range(-0.3f, 0.3f), 0.032f, GameController._instance.mapStock.transform.position.z + Random.Range(-0.3f, 0.3f));
            carrying = false;
            carriedObject = null;
            currentOrder = null;
            doingOrder = false;
            orderTime = 0;
            navAgent.speed = navSpeed;  
            navAgent.SetDestination(transform.position);
        }
    }

    IEnumerator drillTime()
    {
        yield return new WaitForSeconds(4.0f);
        if (drillingZone)
        {
            if (drillingZone.gameObject.transform.parent.gameObject.GetComponent<WallInfo>())
                drillingZone.gameObject.transform.parent.gameObject.GetComponent<WallInfo>().destroyWall();
        }
        currentOrder = null;
        doingOrder = false;
        drilling = false;
        orderTime = 0.0f;
        orderXPos = -1;
    }

}
