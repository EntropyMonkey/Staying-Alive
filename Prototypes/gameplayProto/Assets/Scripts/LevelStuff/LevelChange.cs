using UnityEngine;
using System.Collections;

public class LevelChange : MonoBehaviour {

    public int NextLevelID = -1;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GlobalNames.TAG.Player)
        {
            Player p = other.gameObject.GetComponent<Player>();
            p.LastCheckpoint = null;
			
			Messenger.Invoke(MessengerEvents.ClearQueuedEvents);

			if (NextLevelID >= 0)
				Application.LoadLevel(NextLevelID); // load next level
			else
	            Application.LoadLevel(Application.loadedLevel + 1); // load next level
        }
    }
}
