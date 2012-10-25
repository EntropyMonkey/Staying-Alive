using UnityEngine;
using System.Collections;
using System.Net;

public class OSCTest : MonoBehaviour {
    private string clientNames = "SuperCollider";

    public GameObject shouting;
    public GameObject singing;
    public GameObject shush;
    public GameObject whistling;

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
				Debug.Log (log.Log);
							
				float shoutSize = float.Parse(split[0]);
				float shushSize = float.Parse (split[1]);
				float singingSize = float.Parse (split[2]);
                float whistlingSize = float.Parse(split[3]);
				if(shushSize > 1)
					shushSize = 1;
				
				singing.transform.localScale = new Vector3(singingSize, singingSize, singingSize);
				shush.transform.localScale = new Vector3(shushSize, shushSize, shushSize);
				shouting.transform.localScale = new Vector3(shoutSize,shoutSize,shoutSize);
                whistling.transform.localScale = new Vector3(whistlingSize, whistlingSize, whistlingSize);

				
//	            float size = float.Parse(split[0]);
//				
//	            if (split[1].StartsWith("shout")  && size > 0.01f)	
//	            {
//					//Debug.Log(split[1] + " , " + size);
//	                shouting.transform.localScale = new Vector3(size, size, size);
//	            }
//	            else if (split[1].StartsWith("singing") && size > 0.01f)
//	            {
//					//Debug.Log(split[1] + " , " + size);
//	                singing.transform.localScale = new Vector3(4, 4, 4);
//	            }
//	            else if (split[1].StartsWith("whistling") && size  > 0.01f)
//	            {						
//					//Debug.Log(split[1] + " , " + size);
//					
//	                whistlig.transform.localScale = new Vector3(size*3, size*3, size*3);
//	            }
			}
        }
	}
}
