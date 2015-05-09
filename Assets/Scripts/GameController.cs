using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public SandboxController sandCont;
    public Selector gSelector;

    public bool objectSelected;
    public Entity selectedEntity;

    public orderQueue oQueue;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        objectSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedEntity)
            objectSelected = true;
        else
            objectSelected = false;
    }
}
