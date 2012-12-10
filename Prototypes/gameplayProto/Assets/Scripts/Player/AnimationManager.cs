using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour
{
	public List<AnimationClip> clips;

	// a dictionary for fast access to the clips via name
	private Dictionary<string, AnimationClip> clipNameDict;

	// Use this for initialization
	void Start()
	{
		clipNameDict = new Dictionary<string, AnimationClip>();

		foreach (AnimationClip clip in clips)
		{
			animation.AddClip(clip, clip.name);
			clipNameDict.Add(clip.name, clip);
		}

		clipNameDict[GlobalNames.ANIM.Player_Walk].wrapMode = WrapMode.Loop;
		//clipNameDict[GlobalNames.ANIM.Player_Walk].

		//animation.Play(GlobalNames.ANIM.Player_Walk);

		Messenger<GameObject, bool>.AddListener(GlobalNames.EVENT.Player_Walking, PlayerWalking);
		Messenger<GameObject>.AddListener(GlobalNames.EVENT.Player_EnterStand, PlayerIdle);
	}

	void PlayerWalking(GameObject player, bool started)
	{
		animation.CrossFade(GlobalNames.ANIM.Player_Walk);
	}

	void PlayerIdle(GameObject player)
	{
		animation.CrossFade(GlobalNames.ANIM.Player_Idle);
	}


}
