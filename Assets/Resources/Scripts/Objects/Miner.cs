using UnityEngine;
using System.Collections;

public class Miner : Entity
{
    public NavMeshAgent navAgent;
    public Order currentOrder;
    public bool doingOrder;
    protected float navSpeed;

    protected override void Start()
    {
        selected = false;
    }

    protected override void Update()
    {

    }

    public void selection()
    {
        selected = true;
        GameController._instance.selectedEntity = this;
    }

    protected void moveTo()
    {
        if (navAgent == null)
        {
            navAgent = GetComponent<NavMeshAgent>();
            navSpeed = navAgent.speed;
        }


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

    public override void queueOrder() { }
    public override void queueOrder(Vector3 destination) { }
}
