using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
	public float xSensitivity = 1f;
	public float ySensitivity = -1;
	public float maxDist = 4;
	public float minDist = .5f;
	public Camera c;
	public Transform cameraFocus;
	public Transform target;
	public GlobalInput input;
	public LayerMask castMask;
	private float xLook;
	private float yLook;
	public bool mouseLock = true;
	private bool rotateCamera = false;
	private float camDist;
	private float goalDist;

	void Start()
	{
		cameraFocus.position = target.position;
		c.transform.LookAt(c.transform.position - cameraFocus.position);
		Cursor.visible = false;
		rotateCamera = true;
		Cursor.lockState = CursorLockMode.Locked;
		camDist = goalDist = maxDist;
	}

	void Update()
	{
		//camera move input

		if (rotateCamera)
		{
			xLook += xSensitivity * input.getXTiltLook();
			yLook += ySensitivity * input.getZTiltLook();
			if (xLook > 180f)
				xLook -= 360f;
			if (xLook < -180f)
				xLook += 360f;
			if (yLook >= 90f)
				yLook = 89f;
			if (yLook < -70f)
				yLook = -70f;
		}

		castPosition();

		if (true)
			mouseLocking();
	}

	void LateUpdate()		//TODO : debug minor jitter later
	{
		transform.eulerAngles = new Vector3(0, xLook, 0);
		c.transform.localEulerAngles = new Vector3(yLook, 0);
		camDist = Mathf.Lerp(camDist, goalDist, Time.deltaTime);
		transform.position = cameraFocus.position - c.transform.forward * camDist * .6f;
		cameraFocus.position = Vector3.Lerp(cameraFocus.position, target.position, 10 * Time.deltaTime);
	}

	void castPosition()		//TODO : add distance checking for how close to object
	{
		RaycastHit h;
		if (Physics.Raycast(cameraFocus.position, transform.position - cameraFocus.position, out h, maxDist, castMask.value))
		{
			goalDist = (transform.position - h.point).magnitude * 0.8f;
			if (goalDist < minDist)
				goalDist = minDist;
		}
		else
		{
			goalDist = maxDist;
		}
	}

	void mouseLocking()
	{
		if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.LeftAlt))
		{
			if (rotateCamera)
			{
				Cursor.visible = true;
				rotateCamera = false;
				Cursor.lockState = CursorLockMode.None;
			}
			else
			{
				Cursor.visible = false;
				rotateCamera = true;
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
	}
}
