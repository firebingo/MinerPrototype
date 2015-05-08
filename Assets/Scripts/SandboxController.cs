using UnityEngine;
using System.Collections;

public class SandboxController : MonoBehaviour {

    public mapScript mainMap = null;
    public bool isActive;
    int terrainType;

	// Use this for initialization
	void Start () 
    {
        terrainType = 3;
	}
	
	// Update is called once per frame
	void Update () 
    {
        mainMap = (mapScript)FindObjectOfType(typeof(mapScript));
        if (Input.GetKeyDown(KeyCode.K))
        {
            isActive = !isActive;
        }

        if (isActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 50, 1 << 8))
                {
                    int xPos, zPos;
                    xPos = hit.collider.gameObject.GetComponent<WallInfo>().xPos;
                    zPos = hit.collider.gameObject.GetComponent<WallInfo>().zPos;
                    if (mainMap)
                    {
                        mainMap.map[xPos, zPos].terrainType = terrainType;
                        mainMap.updateMap();
                    }
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 50, 1 << 8))
                {
                    int xPos, zPos;
                    xPos = hit.collider.gameObject.GetComponent<WallInfo>().xPos;
                    zPos = hit.collider.gameObject.GetComponent<WallInfo>().zPos;
                    if (mainMap)
                    {
                        mainMap.map[xPos, zPos].terrainType = 1;
                        mainMap.updateMap();
                    }
                }
            }
            if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                terrainType = 3;
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                terrainType = 6;
            }
        }
	}
}
