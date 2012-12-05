using UnityEngine;
using System.Collections;

public class LevelChange : MonoBehaviour {

    public int NextLevelID;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GlobalNames.TAG.Player)
        {
            Player p = other.gameObject.GetComponent<Player>();
            p.LastCheckpoint = null;
            Application.LoadLevel(NextLevelID); // load next level
        }
    }
}