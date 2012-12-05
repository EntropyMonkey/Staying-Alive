using UnityEngine;
using System.Collections;

public class ConfusingSpawner : MonoBehaviour {
    public GameObject BlueConfusing;
    public GameObject RedConfusing;

    public float SpawnRate = 0.5f;
    private float lifespan;

    private bool lastSpawnedRed = false;
    private Timer timer = new Timer();
	// Use this for initialization
	void Start () {
        lifespan = BlueConfusing.GetComponent<ParticleSystem>().startLifetime;
	}

    private bool isEnabled = false;
    public void EnableSpawning()
    {
        if (!isEnabled)
        {
            timer.Start(SpawnRate);
            Spawn();
            Spawn();
            Spawn();
            Spawn();
            isEnabled = true;
        }
    }

    public void DisableSpawning()
    {
        timer.Stop();
        isEnabled = false;
    }

    private void Spawn()
    {
        GameObject toSpawn = lastSpawnedRed ? RedConfusing : BlueConfusing;
        float xOffset = lastSpawnedRed ? -1 : 1;
        lastSpawnedRed = !lastSpawnedRed;
        var GO = Instantiate(toSpawn, transform.position + new Vector3(xOffset + Random.Range(-0.5f,0.5f), Random.Range(-0.5f,0.5f), 0), Quaternion.identity);
        Destroy(GO, lifespan);
    }
	
	// Update is called once per frame
	void Update () {
        timer.Update(Time.deltaTime);
        if (timer.IsTimeUp)
        {
            Spawn();
            timer.Start(SpawnRate);
        }
	}
}
