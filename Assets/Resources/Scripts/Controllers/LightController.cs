using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour {


	// Update is called once per frame
	void Update () 
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit))
		{
			transform.position = new Vector3(hit.point.x, 2.2f, hit.point.z);
		}
	}
}
