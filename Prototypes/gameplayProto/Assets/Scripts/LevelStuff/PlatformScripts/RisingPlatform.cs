using UnityEngine;
using System.Collections;

public class RisingPlatform : MonoBehaviour {   
    public float RisingDuration = 2.5f;
    public float StayingActiveDuration = 5.0f;

    private float closenessForStaying = 10; //How close in % the platform should be to its trigger the delay

    private bool isTriggered;
    
    private Timer timer = new Timer();

    private Vector3 triggerPosition;
    private Vector3 startPlatformPosition;
    private GameObject platform;

    private float elapsedAppearingTime = 0; //How long the platform has been appearing for

	// Use this for initialization
	void Start () {
        triggerPosition = transform.position;

        foreach (Transform child in transform) //Find child box trigger
        {
            var box = child.GetComponent<BoxCollider>();
            if (box != null)
            {
                platform = box.gameObject;
                startPlatformPosition = box.transform.position;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        timer.Update(Time.deltaTime);

        if (isTriggered)
        {
            elapsedAppearingTime += Time.deltaTime;
        }
        else if(!timer.IsActive) //if the timer is active, then the platform should not move back
        {
            elapsedAppearingTime -= Time.deltaTime;
        }

        elapsedAppearingTime = Mathf.Clamp(elapsedAppearingTime, 0, RisingDuration);

        float percentageCloseness = elapsedAppearingTime / RisingDuration;
        platform.transform.position = Vector3.Lerp(startPlatformPosition, triggerPosition, percentageCloseness);


        if (percentageCloseness > 0.95f)
        {
            if (!timer.IsActive && isTriggered)
            {
                timer.Start(RisingDuration);
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GlobalNames.TAG.WhistlingTrigger)
        {
            isTriggered = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == GlobalNames.TAG.WhistlingTrigger)
        {
            isTriggered = false;
        }
    }
}
