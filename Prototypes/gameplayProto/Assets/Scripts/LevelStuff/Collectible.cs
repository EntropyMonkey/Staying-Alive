using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour
{
	public int points;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == GlobalNames.TAG.Player)
		{
			Player p = other.gameObject.GetComponent<Player>();
			p.Points += points;
			Messenger<GameObject>.Invoke(GlobalNames.EVENT.Cookie_Collected, other.gameObject);
			Destroy(gameObject);
		}
	}
}
