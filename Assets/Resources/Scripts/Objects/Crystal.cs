using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crystal : Entity
{
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
        Order carryCry = new Order();
        List<Vector3> dests = new List<Vector3>();
        dests.Add(transform.position);
        dests.Add(destination);
        carryCry.initOrder(2, dests, this);

		GameController._instance.oQueue.crystalQueue.Enqueue(carryCry);
    }
    public override void queueOrder() { }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "StockPile")
        {
            ++GameController._instance.crystalCount;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.name == "StockPile")
        {
            --GameController._instance.crystalCount;
        }
    }
}
