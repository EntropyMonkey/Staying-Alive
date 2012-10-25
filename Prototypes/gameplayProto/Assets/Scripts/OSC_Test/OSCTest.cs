using UnityEngine;
using System.Collections;
using System.Net;

public class OSCTest : MonoBehaviour {
    private string clientNames = "SuperCollider";

    [HideInInspector]
    public bool Shouting;
    [HideInInspector]
    public bool Shushing;
    [HideInInspector]
    public bool Whistling;
    [HideInInspector]
    public bool Singing;

	// Use this for initialization
	void Start () {
        OSCHandler.Instance.Init();

        OSCHandler.Instance.CreateReciever("OSCServer", 31000); // Receiver

        OSCHandler.Instance.CreateSender(clientNames, IPAddress.Parse("127.0.0.1"), 30000); //Sender
	}
	
	// Update is called once per frame	
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Space))
        {
            OSCHandler.Instance.SendMessageToClient(clientNames,"/Hej",5);
        }
        OSCHandler.Instance.UpdateLogs();
        var newLogs = OSCHandler.Instance.FetchNewReceiverLogs();

        if (newLogs.Count > 0)
        {
			foreach(var log in newLogs)
			{
	            var split = log.Log.Split('#');
				//Debug.Log (log.Log);
							
				float shoutSize = float.Parse(split[0]);
				float shushSize = float.Parse (split[1]);
				float singingSize = float.Parse (split[2]);
                float whistlingSize = float.Parse(split[3]);

                if (singingSize > 0.5f)
                    Singing = true;
                else
                    Singing = false;

                if (shushSize > 0.5f)
                    Shushing = true;
                else
                    Shushing = false;

                if (shoutSize > 0.5f)
                    Shouting = true;
                else
                    Shouting = false;

                if (whistlingSize > 0.5f)
                    Whistling = true;
                else
                    Whistling = false;
			}
        }
	}
}
