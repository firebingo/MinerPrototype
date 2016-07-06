using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Order
{
    GameController gameMaster;
    orderQueue oQueue;

    public Entity parentObject;

    //order types
    // 0 = null
    // 1 = carry ore
    // 2 = carry crystal
    // 3 = drill 

    public int orderType;

    public List<Vector3> destination;

    public void initOrder(int iType, List<Vector3> iDest, Entity iPar)
    {
        orderType = iType;
        destination = iDest;
        parentObject = iPar;
    }
}
