using UnityEngine;
using System.Collections;

public class ShushSpawner : MonoBehaviour {
    public float SpawnNextDelay = 1f;

    public float XOffset = 2.0f;

    public GameObject ShushObject;
    public GameObject EndObject;
    private GameObject toSpawn;
    
    private Timer timer = new Timer();
    private bool alreadySpawned = false;

    private 

	// Use this for initialization
	void Start () {
        toSpawn = ShushObject;
        timer.Start(SpawnNextDelay);
	}

    public void StopShush()
    {
        if (!alreadySpawned)
        {
            toSpawn = EndObject;
        }
    }
	
	// Update is called once per frame
	void Update () {
        timer.Update(Time.deltaTime);

        if (timer.IsTimeUp && ! alreadySpawned)
        {
            alreadySpawned = true;
            if (ShushObject != null)
            {
                GameObject go = Instantiate(toSpawn, transform.position + new Vector3(XOffset, 0, 0), transform.rotation) as GameObject;
                go.transform.parent = transform;
            }
        }
	}
}