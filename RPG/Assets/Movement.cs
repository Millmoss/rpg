using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	public float acceleration = 1.3f;
	public float maxVelocity = 2.5f;
	public float velocityLerping = 0.9f;
	public float turnSpeed = 5f;
	public Camera c;
	public Rigidbody playerBody;
	public GlobalInput input;
	private Vector3 velocity = Vector3.zero;

    void Start()
    {
		velocityLerping = velocityLerping / 0.013333f;
	}

	void Update()
	{
		float x = input.getXTiltMove();
		float z = input.getZTiltMove();
		Vector3 f = c.transform.forward;
		f = new Vector3(f.x, 0, f.z).normalized;
		Vector3 r = c.transform.right;
		r = new Vector3(r.x, 0, r.z).normalized;
		velocity += x * acceleration * Time.deltaTime * r +
					z * acceleration * Time.deltaTime * f;
		velocity = Vector3.ClampMagnitude(velocity, maxVelocity);

		playerBody.rotation = Quaternion.Slerp(playerBody.rotation, Quaternion.LookRotation(velocity.normalized, transform.up), turnSpeed * Time.deltaTime);
	}

	void FixedUpdate()
    {
		Vector3 velocityCurrent = new Vector3(playerBody.velocity.x, 0, playerBody.velocity.z);
		velocityCurrent = Vector3.Lerp(velocityCurrent, velocity, velocityLerping * Time.deltaTime);
		playerBody.velocity = new Vector3(velocityCurrent.x, playerBody.velocity.y, velocityCurrent.z);
    }
}
