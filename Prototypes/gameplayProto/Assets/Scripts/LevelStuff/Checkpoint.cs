using UnityEngine;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour {

    public void loadCheckpoint(Transform player)
    {
        foreach(Transform child in transform)
        {
            var obj = child.gameObject.GetComponent<DestructableObject>();
            if (obj != null)
            {
                obj.Reset();
            }
        }
        // Reset player!
        player.position = gameObject.transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GlobalNames.TAG.Player && gameObject.active)
        {
            // ensure that check point can only be activated once
            gameObject.active = false;
            // update last activated check point id
            other.gameObject.GetComponent<Player>().lastCheckpoint = gameObject;
        }
    }
}
