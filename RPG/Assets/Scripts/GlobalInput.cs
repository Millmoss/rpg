using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInput : MonoBehaviour
{
	public bool controller = false;

	public float getXTiltMove()
	{
		float tempx = 0;
		float tempz = 0;
		if (controller)
		{
			tempx = Input.GetAxis("Horizontal");
			tempz = Input.GetAxis("Vertical");
		}
		else
		{
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
				tempx += 1;
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
				tempx += -1;
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
				tempz += 1;
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
				tempz += -1;
		}
		Vector3 tempv = Vector3.ClampMagnitude(new Vector3(tempx, 0, tempz), 1);
		if (Mathf.Abs(tempv.x) >= .25f)
			return tempv.x;
		return 0;
	}

	public float getZTiltMove()
	{
		float tempx = 0;
		float tempz = 0;
		if (controller)
		{
			tempx = Input.GetAxis("Horizontal");
			tempz = Input.GetAxis("Vertical");
		}
		else
		{
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
				tempx += 1;
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
				tempx += -1;
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
				tempz += 1;
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
				tempz += -1;
		}
		Vector3 tempv = Vector3.ClampMagnitude(new Vector3(tempx, 0, tempz), 1);
		if (Mathf.Abs(tempv.z) >= .25f)
			return tempv.z;
		return 0;
	}

	public float getXTiltLook()		//TODO : IMPLEMENT FOR CONTROLLER
	{
		float temp = Input.GetAxis("Mouse X");
		if (Mathf.Abs(temp) >= .07f)
			return temp;
		return 0;
	}

	public float getZTiltLook()     //TODO : IMPLEMENT FOR CONTROLLER
	{
		float temp = Input.GetAxis("Mouse Y");
		if (Mathf.Abs(temp) >= .07f)
			return temp;
		return 0;
	}

	public bool getSprint()
	{
		bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

		return sprint;
	}
}
