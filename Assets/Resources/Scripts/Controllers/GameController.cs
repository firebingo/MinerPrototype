using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
	public static GameController _instance;

    public SandboxController sandCont;
    public Selector gSelector;

    public bool objectSelected;
    public Entity selectedEntity;

    public orderQueue oQueue;
    public StockPile mapStock;

    public GameObject gameUI;

    public int oreCount;
    public int crystalCount;
    public float oxyTime;

    void Awake()
    {
		if(_instance)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
		}
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

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
