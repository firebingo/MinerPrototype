using UnityEngine;
using System.Collections;

public class StockPile : MonoBehaviour
{
    GameController gameMaster;

    // Use this for initialization
    void Start()
    {
        gameMaster = FindObjectOfType<GameController>();
        gameMaster.mapStock = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
