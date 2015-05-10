using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crystal : Entity
{
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
        Order carryCry = new Order();
        List<Vector3> dests = new List<Vector3>();
        dests.Add(transform.position);
        dests.Add(destination);
        carryCry.initOrder(2, dests, this);

        gameMaster.oQueue.crystalQueue.Enqueue(carryCry);
    }
    public override void queueOrder() { }
}
