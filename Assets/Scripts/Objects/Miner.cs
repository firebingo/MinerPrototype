using UnityEngine;
using System.Collections;

public class Miner : Entity
{
    public NavMeshAgent navAgent;
    public Order currentOrder;
    public bool doingOrder;

    protected override void Start()
    {
        gameMaster = FindObjectOfType<GameController>();
        selected = false;
    }

    protected override void Update()
    {

    }

    public void selection()
    {
        selected = true;
        gameMaster.selectedEntity = this;
    }

    protected void moveTo()
    {
        if(navAgent == null)
            navAgent = GetComponent<NavMeshAgent>();


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50, 1 << 8))
        {
            if (hit.collider.name.ToString() == "floor(Clone)")
            {
                navAgent.SetDestination(hit.point);
            }
        }
    }

    public override void queueOrder()
    {
        throw new System.NotImplementedException();
    }
}
