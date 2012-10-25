using UnityEngine;
using System.Collections;

public class DamageHealth : MonoBehaviour
{
	public float Damage = 1;

	public bool ShowDeathArea = true;
	
	void OnTriggerEnter(Collider other)
	{
		Health h = other.gameObject.GetComponent<Health>();
		if (h != null)
		{
			h.HealthPoints -= Damage;
		}
	}

	void Update()
	{
		renderer.enabled = ShowDeathArea;
	}
}
