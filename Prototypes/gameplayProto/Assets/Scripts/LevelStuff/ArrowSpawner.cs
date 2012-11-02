using UnityEngine;
using System.Collections;

public class ArrowSpawner : MonoBehaviour {
    public float DelayBetweenSpawns = 3F;
    public GameObject PrefabArrow;

    public Vector3 ArrowFlyDirection;

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
                GameObject arrow = Instantiate(PrefabArrow, transform.position, transform.rotation) as GameObject;
                var arrowScript = arrow.GetComponent<ArrowScript>();
                arrowScript.spawnParent = this;
                arrowScript.FlyDirection = ArrowFlyDirection;
                isChildDead = false; 

            }
            timer.Start(DelayBetweenSpawns);
        }
	}
}
