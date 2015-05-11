using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float cameraSpeed;

	void Start()
	{
		Camera.main.depthTextureMode = DepthTextureMode.Depth;
	}

	// Update is called once per frame
	void Update () 
	{
		if(Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0)
		{
			transform.position = new Vector3(transform.position.x + Input.GetAxis("Vertical") * Time.deltaTime * cameraSpeed,transform.position.y,transform.position.z);
		}
		if(Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0)
		{
			transform.position = new Vector3(transform.position.x ,transform.position.y, transform.position.z + -(Input.GetAxis("Horizontal") * Time.deltaTime * cameraSpeed));
		}
	}
}
