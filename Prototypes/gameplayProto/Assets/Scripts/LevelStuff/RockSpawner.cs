using UnityEngine;
using System.Collections;

public class RockSpawner : MonoBehaviour {
    public float DelayBetweenSpawns = 3F;
    public GameObject PrefabRock;

    Timer timer = new Timer();
	// Use this for initialization
	void Start () {
	}

    private bool isChildDead = true;

    public void ChildDeath(RockScript child)
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
                rot.eulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                GameObject rock = Instantiate(PrefabRock, transform.position, rot) as GameObject;
                rock.GetComponent<RockScript>().spawnParent = this;
                isChildDead = false;

            }
            timer.Start(DelayBetweenSpawns);
        }
	}
}
