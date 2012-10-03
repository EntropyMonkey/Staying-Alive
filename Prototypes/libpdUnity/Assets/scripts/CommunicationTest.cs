using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class CommunicationTest : MonoBehaviour
{

	public string IpAddress;

	private Socket sendingSocket;
	private IPAddress sendToAddress;
	private IPEndPoint sendingEndPoint;

	// Use this for initialization
	void Start () 
	{
		sendingSocket = new Socket(AddressFamily.InterNetwork,
			SocketType.Dgram,
			ProtocolType.Udp);

		sendToAddress = IPAddress.Parse(IpAddress);

		sendingEndPoint = new IPEndPoint(sendToAddress, 3001);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			byte[] sendBuffer = Encoding.ASCII.GetBytes("bang!");

			Debug.Log("sending to address:" + sendingEndPoint.Address + 
				" port: " +	sendingEndPoint.Port);

			sendingSocket.SendTo(sendBuffer, sendingEndPoint);
		}
	}
}
