using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class resourceCount : MonoBehaviour
{
    Text resourceText;
    GameController gameMaster;
    // Use this for initialization
    void Start()
    {
        gameMaster = FindObjectOfType<GameController>();
        resourceText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        resourceText.text = "Ore: " + gameMaster.oreCount + "\nCrystals: " + gameMaster.crystalCount + "\nOxygen: " + gameMaster.oxyTime; 
    }
}
