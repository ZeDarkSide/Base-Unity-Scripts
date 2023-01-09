using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

	public float sensX;
	
	public float sensY;

	public Transform orient;

	float xRot;
	float yRot;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update()
    {
		float mousex = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
		float mousey = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

		yRot += mousex;

		xRot -= mousey;
		xRot = Mathf.Clamp(xRot, -90f, 90f);

		transform.rotation = Quaternion.Euler(xRot, yRot, 0);
		orient.rotation = Quaternion.Euler(0, yRot, 0);


	}


}
