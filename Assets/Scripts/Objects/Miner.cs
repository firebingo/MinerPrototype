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

    protected void selection()
    {
        if (selected)
        {
            selected = false;
            gameMaster.selectedEntity = null;
        }
        if (!selected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50, 1 << 9))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    selected = true;
                    gameMaster.selectedEntity = this;
                }
            }
        }
    }

    protected void moveTo()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50, 1 << 8))
        {
            if(hit.collider.name.ToString() == "floor(Clone)")
            {
                this.GetComponent<NavMeshAgent>().SetDestination(hit.point);
                
            }
        }
    }
}
