using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class LegIK : MonoBehaviour
{
	public Animator animator;

	public bool ikActive = false;
	public Transform leftFootCenter;
	public Transform rightFootCenter;
	public GameObject leftFootBall;
	public GameObject rightFootBall;
	public LayerMask walkableMask;

	void Start()
	{

	}

	//a callback for calculating IK
	void OnAnimatorIK()
	{
		if (animator)
		{
			if (ikActive)
			{
				RaycastHit h;
				if (Physics.Raycast(leftFootCenter.position, transform.up, out h, .5f, walkableMask.value))
				{
					leftFootBall.transform.position = h.point;
					animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
					animator.SetIKPosition(AvatarIKGoal.LeftFoot, h.point);
				}
				if (Physics.Raycast(leftFootCenter.position, -transform.up, out h, .5f, walkableMask.value))
				{
					leftFootBall.transform.position = h.point;
					animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
					animator.SetIKPosition(AvatarIKGoal.LeftFoot, h.point);
				}

				if (Physics.Raycast(rightFootCenter.position, transform.up, out h, .5f, walkableMask.value))
				{
					rightFootBall.transform.position = h.point;
					animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
					animator.SetIKPosition(AvatarIKGoal.RightFoot, h.point);
				}
				if (Physics.Raycast(rightFootCenter.position, -transform.up, out h, .5f, walkableMask.value))
				{
					rightFootBall.transform.position = h.point;
					animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
					animator.SetIKPosition(AvatarIKGoal.RightFoot, h.point);
				}
			}

			//if the IK is not active, set the position and rotation of the hand and head back to the original position
			else
			{
				animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
				animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
			}
		}
	}
}
