using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour
{
	public int Id = 0;

	public Vector3 position
	{
		get;
		private set;
	}

	void Start()
	{
		position = transform.position;
	}
}
