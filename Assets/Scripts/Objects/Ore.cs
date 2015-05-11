using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ore : Entity
{
    // Use this for initialization
    protected override void Start()
    {
        gameMaster = FindObjectOfType<GameController>();
        queueOrder(gameMaster.mapStock.transform.position);
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

        gameMaster.oQueue.oreQueue.Enqueue(carryOre);
    }
    public override void queueOrder() { }

    public void OnTriggerEnter(Collider other)
    {
        if(other.name == "StockPile")
        {
            ++gameMaster.oreCount;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.name == "StockPile")
        {
            --gameMaster.oreCount;
        }
    }
}
