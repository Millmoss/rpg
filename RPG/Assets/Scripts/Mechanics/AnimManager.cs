using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : MonoBehaviour
{
	public Animator anim;
	private AnimationClip[] animClips;
	private string[] animNames;
	private float cutTimeTemp = 0;
	private float t = 0;

    void Start()
	{
		animClips = anim.runtimeAnimatorController.animationClips;
		animNames = new string[animClips.Length];
		for (int i = 0; i < animClips.Length; i++)
		{
			animNames[i] = animClips[i].name;
			if (animNames[i] == "Armature|CutRight")
				cutTimeTemp = animClips[i].length;
		}
	}
	
    void Update()
    {
		if (Input.GetMouseButtonDown(0))// && t < cutTimeTemp)
		{
			//t += Time.deltaTime;
			anim.Play("Cut", 0, 0);
		}
		else
		{
			//t = 0;
		}
    }
}
