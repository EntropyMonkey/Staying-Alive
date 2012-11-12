using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{

	private Transform player;

	// Use this for initialization
	void Start()
	{
		player = GameObject.FindGameObjectWithTag(GlobalNames.TAG.Player).transform;
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 pos = transform.position;
		pos.x = player.position.x;
		pos.y = player.position.y;
		transform.position = pos;
	}
}
