using UnityEngine;
using System.Collections;

public class OxygenController : MonoBehaviour
{
    GameController gameMaster;
    float time;
    // Use this for initialization
    void Start()
    {
        gameMaster = FindObjectOfType<GameController>();
        time = 600;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        gameMaster.oxyTime = time;
    }
}
