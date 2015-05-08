using UnityEngine;
using System.Collections;

public class Miner : Entity
{
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50, 1 << 8))
        {
            if (hit.collider.name.ToString() == "floor(Clone)")
            {
                this.GetComponent<NavMeshAgent>().SetDestination(hit.point);

            }
        }
    }
}
