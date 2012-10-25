using UnityEngine;
using System.Collections;

public class DestructableObject : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == GlobalNames.TAG.ShoutingTrigger)
		{
			gameObject.active = false;
		}
	}
}
