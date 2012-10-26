using UnityEngine;
using System.Collections;

public class DestructableObject : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == GlobalNames.TAG.ShoutingTrigger)
		{
			gameObject.active = false;
			for (int i = 0; i < transform.GetChildCount(); i++)
			{
				transform.GetChild(i).gameObject.active = false;		
			}
		}
	}
}
