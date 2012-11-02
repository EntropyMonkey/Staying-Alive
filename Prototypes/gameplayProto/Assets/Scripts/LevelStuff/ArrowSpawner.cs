using UnityEngine;
using System.Collections;

public class ArrowSpawner : MonoBehaviour {
    public float DelayBetweenSpawns = 3F;
    public GameObject PrefabArrow;

    Timer timer = new Timer();
	// Use this for initialization
	void Start () {
	}

    private bool isChildDead = true;

    public void ChildDeath(ArrowScript child)
    {
        isChildDead = true;
    }
	
	// Update is called once per frame
	void Update () {
        timer.Update(Time.deltaTime);
        if (!timer.IsActive)
        {
            if (isChildDead)
            {
                Quaternion rot = new Quaternion();
                GameObject arrow = Instantiate(PrefabArrow, transform.position, transform.rotation) as GameObject;
                arrow.GetComponent<ArrowScript>().spawnParent = this;
                isChildDead = false;

            }
            timer.Start(DelayBetweenSpawns);
        }
	}
}
