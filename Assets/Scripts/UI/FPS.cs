using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPS : MonoBehaviour
{
    Text displayText;

    float framesPerSec;
    float elapsedTime;
    int frameCount;

    // Use this for initialization
    void Start()
    {
        displayText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        frameCount++;
        if(elapsedTime >= 0.15f)
        {
            framesPerSec = (float)frameCount / elapsedTime;
            elapsedTime = 0;
            frameCount = 0;
            displayText.text = "FPS: " + framesPerSec;
        }
    }
}
