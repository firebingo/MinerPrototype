using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour
{

    GameController gameMaster;

    public bool canSelect;

    // Use this for initialization
    void Start()
    {
        gameMaster = FindObjectOfType<GameController>();
        gameMaster.gSelector = this;
        canSelect = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMaster.sandCont)
            if (gameMaster.sandCont.isActive)
                canSelect = false;
            else
                canSelect = true;
        if (canSelect)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 50, 1 << 9))
                {
                    Miner selection = hit.collider.GetComponent<Miner>();
                    if (gameMaster.selectedEntity)
                    {
                        gameMaster.selectedEntity.selected = false;
                        gameMaster.selectedEntity.selectionChange = true;
                        gameMaster.selectedEntity = null;
                    }
                    gameMaster.selectedEntity = selection;
                    gameMaster.selectedEntity.selectionChange = true;
                    selection.selection();
                }
                else if (Physics.Raycast(ray, out hit, 50, 1 << 8))
                {
                    WallInfo selection = hit.collider.GetComponent<WallInfo>();
                    if (selection.terrainType != terrainType.solidrock)
                    {
                        if (gameMaster.selectedEntity)
                        {
                            gameMaster.selectedEntity.selected = false;
                            gameMaster.selectedEntity.selectionChange = true;
                            gameMaster.selectedEntity = null;
                        }
                        gameMaster.selectedEntity = selection;
                        gameMaster.selectedEntity.selectionChange = true;
                        selection.selection();
                    }
                }
            }
        }
    }
}
