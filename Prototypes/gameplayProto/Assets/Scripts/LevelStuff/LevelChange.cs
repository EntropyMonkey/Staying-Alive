using UnityEngine;
using System.Collections;

public class LevelChange : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GlobalNames.TAG.Player)
        {
            gameObject.active = false;
            GameObject temp = GameObject.FindGameObjectWithTag(GlobalNames.TAG.Player);
            Player p = temp.GetComponent<Player>();
            p.LastCheckpoint = null;
            Application.LoadLevel(++p.currentLevel); // load next level
            //p.Reset();
        }
    }
}
