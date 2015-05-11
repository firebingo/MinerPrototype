using UnityEngine;
using System.Collections;

public class gameCanvas : MonoBehaviour
{
    GameController gameMaster;
    // Use this for initialization
    void Start()
    {
        gameMaster = FindObjectOfType<GameController>();
        gameMaster.gameUI = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMaster.selectedEntity)
        {
            if (gameMaster.selectedEntity.GetComponent<WallInfo>() != null)
            {
                WallInfo selectedWall = gameMaster.selectedEntity.GetComponent<WallInfo>();
                if (selectedWall.terrainType >= 3 && selectedWall.terrainType <= 6)
                {
                    transform.FindChild("Drill").gameObject.SetActive(true);
                }
                else
                    transform.FindChild("Drill").gameObject.SetActive(false);
            }
            else
                transform.FindChild("Drill").gameObject.SetActive(false);
        }
    }
}
