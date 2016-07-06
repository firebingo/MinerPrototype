using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ore : Entity
{
    // Use this for initialization
    protected override void Start()
    {
        queueOrder(GameController._instance.mapStock.transform.position);
    }

    // Update is called once per frame
    protected override void Update()
    {

    }

    public override void queueOrder(Vector3 destination)
    {
        Order carryOre = new Order();
        List<Vector3> dests = new List<Vector3>();
        dests.Add(transform.position);
        dests.Add(destination);
        carryOre.initOrder(1, dests, this);

		GameController._instance.oQueue.oreQueue.Enqueue(carryOre);
    }
    public override void queueOrder() { }

    public void OnTriggerEnter(Collider other)
    {
        if(other.name == "StockPile")
        {
            ++GameController._instance.oreCount;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.name == "StockPile")
        {
            --GameController._instance.oreCount;
        }
    }
}
