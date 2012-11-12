using UnityEngine;
using System.Collections;
using System.Net;
using System.Collections.Generic;
using System;

public class OSCManager : MonoBehaviour
{
	public bool DebugMe = false;

	public bool Shouting
	{
		get;
		private set;
	}
	public bool Shushing
	{
		get;
		private set;
	}
	public bool Whistling
	{
		get;
		private set;
	}
	public bool Singing
	{
		get;
		private set;
	}

	// Use this for initialization
	void Start()
	{
        try
        {
            OSCHandler.Instance.Init();

            // Receive
            OSCHandler.Instance.CreateReciever(GlobalNames.NAME.OSCReceiverName, 31000);

            //Sender
            OSCHandler.Instance.CreateSender(GlobalNames.NAME.OSCSenderName,
                IPAddress.Parse("127.0.0.1"), 30000);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Couldn't create server");
        }
        //DontDestroyOnLoad(gameObject);
	}

	// send one value to pd
	public void SendMessageToPD<T>(string message, T value)
	{
		if (DebugMe)
		{
			Debug.Log("OSC-SendMessage: " + message + " -> " + value);
		}
		OSCHandler.Instance.SendMessageToClient(
			GlobalNames.NAME.OSCSenderName, message, value);
	}

	// send several values to pd
	public void SendMessageToPD<T>(string message, List<T> values)
	{
		if (DebugMe)
		{
			Debug.Log("OSC-SendMessage: " + message + " -> " + values.ToString());
		}

		OSCHandler.Instance.SendMessageToClient(
			GlobalNames.NAME.OSCSenderName, message, values);
	}

	// Update is called once per frame	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			OSCHandler.Instance.SendMessageToClient(GlobalNames.NAME.OSCSenderName, "TEST", 5);
		}

		OSCHandler.Instance.UpdateLogs();
		var newLogs = OSCHandler.Instance.FetchNewReceiverLogs();

		if (newLogs.Count > 0)
		{
			foreach (var log in newLogs)
			{
				var split = log.Log.Split('#');
				if (DebugMe)
				{
					Debug.Log(log.Log);
				}

				float shoutSize = float.Parse(split[0]);
				float shushSize = float.Parse(split[1]);
				float singingSize = float.Parse(split[2]);
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
