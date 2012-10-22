using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
	public float HealthPoints
	{
		get
		{
			return healthPoints;	
		}
		set
		{
			healthPoints = value;
			if (healthPoints <= 0 && player != null)
			{
				healthPoints = player.settings.MaxHealthPoints;
				player.Reset();
			}
			
			if (healthPoints > player.settings.MaxHealthPoints)
			{
				healthPoints = player.settings.MaxHealthPoints;	
			}
		}
	}
	
	private float healthPoints;
	
	private Player player;
	
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag(GlobalNames.TAG.Player).GetComponent<Player>();
	}
}


