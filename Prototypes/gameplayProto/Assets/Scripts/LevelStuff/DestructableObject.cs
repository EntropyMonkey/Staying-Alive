using UnityEngine;
using System.Collections;

public class DestructableObject : MonoBehaviour
{
    // Save original position
    private Transform startTransform;

    public void Reset()
    {
        // reset position and velocity
        transform.position = startTransform.position;

        gameObject.active = true;
        for (int i = 0; i < transform.GetChildCount(); i++)
        {
            transform.GetChild(i).gameObject.active = true;
        }
    }

    void Start()
    {
        startTransform = transform;
    }

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
