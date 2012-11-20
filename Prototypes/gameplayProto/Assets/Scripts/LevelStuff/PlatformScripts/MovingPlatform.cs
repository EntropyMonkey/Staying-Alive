using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// a moving platform class
public class MovingPlatform : MonoBehaviour
{
	public float movementSpeed = 1;
	public float threshold = 0.1f;  

	private Waypoint[] waypoints;
	private int next; // next waypoint
	private int last; // last waypoint

	void Start()
	{
		// order waypoints according to their indices
		waypoints = transform.GetComponentsInChildren<Waypoint>();
		Array.Sort(waypoints, 
			delegate(Waypoint w1, Waypoint w2)
			{
				return w1.Id.CompareTo(w2.Id);
			}
		);

		next = 1;
		last = 0;
	}

	void Update()
	{
		Vector3 velocity = waypoints[next].position - waypoints[last].position;

		velocity = velocity.normalized * movementSpeed * Time.deltaTime;

		Vector3 position = transform.position + velocity;
		transform.position = position;

		if (Vector3.Distance(position, waypoints[next].position) < threshold)
		{
			last = next;
			next++;

			if (next >= waypoints.Length)
				next = 0;
		}
	}
}
