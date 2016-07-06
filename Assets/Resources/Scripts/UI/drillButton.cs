using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class drillButton : MonoBehaviour
{
    GameController gameMaster;
    // Use this for initialization
    void Start()
    {
        gameMaster = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onButtonPush()
    {
        if (gameMaster.selectedEntity)
        {
            if (gameMaster.selectedEntity.GetComponent<WallInfo>() != null)
            {
                WallInfo selectedWall = gameMaster.selectedEntity.GetComponent<WallInfo>();
                if (selectedWall.terrainType >= terrainType.softrock && selectedWall.terrainType <= terrainType.solidrock)
                {
                    selectedWall.queueOrder();
                }
            }
        }
    }
}
