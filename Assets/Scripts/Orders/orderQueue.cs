using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class orderQueue : MonoBehaviour
{
    GameController gameMaster;

    public Queue<Order> oreQueue;
    public Queue<Order> crystalQueue;
    public Queue<Order> drillQueue;

    // Use this for initialization
    void Start()
    {
        gameMaster = FindObjectOfType<GameController>();
        gameMaster.oQueue = this;

        oreQueue = new Queue<Order>();
        crystalQueue = new Queue<Order>();
        drillQueue = new Queue<Order>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(drillQueue.Count);
        }
    }

    public void addOrder(Order iOrder)
    {
        if(iOrder.orderType == 1)
        {
            oreQueue.Enqueue(iOrder);  
        }
        if (iOrder.orderType == 2)
        {
            crystalQueue.Enqueue(iOrder);
        }
        if (iOrder.orderType == 3)
        {
            drillQueue.Enqueue(iOrder);
        }
    }
}
