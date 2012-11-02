using UnityEngine;
using System.Collections;

public class RockScript : MonoBehaviour {
    public float ShushDelay = 2F;
    private Rigidbody physics;
    private bool isSleeping = false;

    [HideInInspector]
    public RockSpawner spawnParent;


	// Use this for initialization
	void Start () {
        physics = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    Vector3 velocity;

    IEnumerator Rewake()
    {
        Debug.Log("Sleep");
        if(!isSleeping) //If already is sleeping, then the velocity would be zero and the object wouldn't move on rewake
            velocity = rigidbody.velocity;
        yield return new WaitForSeconds(ShushDelay);
        Debug.Log("Awake");
        isSleeping = false;
        rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
        rigidbody.velocity = velocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isSleeping)
        {
            if (collision.gameObject.tag == GlobalNames.TAG.Player)
            {
                collision.gameObject.GetComponent<Player>().Reset();
                spawnParent.ChildDeath(this);   
                GameObject.Destroy(transform.gameObject);
            }
            else 
            {
                spawnParent.ChildDeath(this);
                GameObject.Destroy(transform.gameObject);
                
            }
        }
    }   
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == GlobalNames.TAG.ShushTrigger)
        {
            StartCoroutine(Rewake());
            isSleeping = true;
            gameObject.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
