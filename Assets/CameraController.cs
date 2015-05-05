using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0)
		{
			transform.position = new Vector3(transform.position.x + Input.GetAxis("Vertical")/150,transform.position.y,transform.position.z);
		}
		if(Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0)
		{
			transform.position = new Vector3(transform.position.x ,transform.position.y, transform.position.z + -(Input.GetAxis("Horizontal")/150));
		}
	}
}
