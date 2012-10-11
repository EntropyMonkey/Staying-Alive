using UnityEngine;
using System.Collections;
using System.Net;

public class OSCTest : MonoBehaviour {
    private string clientNames = "SuperCollider";

    public GameObject sphereSizer;

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
            float size = float.Parse(newLogs[0].Log);
            size += 50.0f;
            size /= 10.0f;
            sphereSizer.transform.localScale = new Vector3(size, size, size);
        }
	}
}
