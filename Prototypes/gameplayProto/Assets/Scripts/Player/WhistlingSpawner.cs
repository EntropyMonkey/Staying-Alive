using UnityEngine;
using System.Collections;

public class WhistlingSpawner : MonoBehaviour {
    public Material[] NoteMaterials;

    public GameObject WhistlingParticleSystem;

    public float SpawnRate;

    private float lifespan;

    Timer timer = new Timer();

	public bool Enabled
	{
		get
		{
			return isEnabled;
		}
	}

	// Use this for initialization
	void Start () {
        if (WhistlingParticleSystem == null)
            Debug.LogWarning("Particle system for whistling couldn't be found");
        lifespan = WhistlingParticleSystem.GetComponent<ParticleSystem>().startLifetime;
	}


    private bool isEnabled = false;
    public void EnableSpawning()
    {
        if (!isEnabled)
        {
            Spawn();
            timer.Start(SpawnRate);
            isEnabled = true;
        }
    }

    public void DisableSpawning()
    {
        isEnabled = false;
        timer.Stop();
    }

    private void Spawn()
    {
        Vector3 pos = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-0.3f, 2.0f), 0);
        pos += transform.position;
        GameObject go = Instantiate(WhistlingParticleSystem, pos, Quaternion.identity) as GameObject;
        Destroy(go, lifespan);
        go.GetComponent<ParticleSystem>().renderer.material = NoteMaterials[Random.Range(0, NoteMaterials.Length)];
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
