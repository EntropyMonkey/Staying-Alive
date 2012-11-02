using UnityEngine;
using System.Collections;

public class AppearingPlatform : MonoBehaviour
{
    //Is the player whistle sphere in contact with this rock?
    private bool isTriggered = false;
    //Delay for full appearing
    public float AppearingDelay = 2f;
    //How long should it remain active if triggered?
    public float StayingActiveTime = 4f; 

    //How long the platform has been appearing (time)
    private float elapsedAppearingTime = 0;
    //Is the 
    private bool isFullyAppeared;
    private bool justFullyActivated;

    Timer timer = new Timer();

    private BoxCollider rockCollider;
    private BoxCollider rockTrigger;
    private bool isPlayerInside = false;

    void Start()
    {
        rockCollider = transform.parent.GetComponent<BoxCollider>();
        rockTrigger = GetComponent<BoxCollider>();
    }

    void Update()
    {
        //Debug.Log(isPlayerInside);
        timer.Update(Time.deltaTime);

        if (isTriggered && timer.IsActive)
            timer.Start(StayingActiveTime);

        if (!timer.IsActive)
        {
            if (isTriggered)
            {
                elapsedAppearingTime += Time.deltaTime;
                if (elapsedAppearingTime > AppearingDelay)
                    elapsedAppearingTime = AppearingDelay;
            }
            else
            {
                elapsedAppearingTime -= Time.deltaTime;
                if (elapsedAppearingTime < 0)
                    elapsedAppearingTime = 0;
            }
        }
        float percentageAppeared = elapsedAppearingTime / AppearingDelay;
        if (timer.IsActive)
            percentageAppeared = 1;
        if (percentageAppeared > 0.9f || timer.IsActive) //If 90% activated, then make it collide
        {
            if(!isPlayerInside)
            {
                rockCollider.enabled = true;
            }
        }
        else
        {
			if(!timer.IsActive)
            	rockCollider.enabled = false;
        }

        //Set the transparency of the rock
        var color = transform.parent.renderer.material.color;
        if (timer.IsActive)
            color.a = 1F; //If the timer is active, then it's fully appeared and shouldn't be transparent
        else
            color.a = percentageAppeared * 0.7f + 0.3f; //Min of 30% transparency
        transform.parent.renderer.material.color = color;

        if(percentageAppeared > 0.9f)
        {
            if (!justFullyActivated && !isFullyAppeared)
                justFullyActivated = true;
            isFullyAppeared = true;
        }
        else
        {
            isFullyAppeared = false;
            justFullyActivated = false;
        }
        if (isTriggered && justFullyActivated)  
        {
            Debug.Log("Start");
            timer.Start(StayingActiveTime);
        }

        justFullyActivated = false;
    }

    void OnTriggerEnter(Collider other)
    {
		if(other.gameObject.tag == GlobalNames.TAG.Player)
        {
            isPlayerInside = true;
        }
        if (other.gameObject.tag == GlobalNames.TAG.WhistlingTrigger)
        {
            isTriggered = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == GlobalNames.TAG.Player)
        {
            isPlayerInside = false;
        }
        if (other.gameObject.tag == GlobalNames.TAG.WhistlingTrigger)
        {   
            isTriggered = false;
        }
    }
}
