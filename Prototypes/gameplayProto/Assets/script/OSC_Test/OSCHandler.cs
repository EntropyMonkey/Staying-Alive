//
//	  UnityOSC - Open Sound Control interface for the Unity3d game engine
//
//	  Copyright (c) 2011 Jorge Garcia <info@jorgegarciamartin.com>
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, see <http://www.gnu.org/licenses/>.
//
/// Inspired by http://www.unifycommunity.com/wiki/index.php?title=AManagerClass

using System;
using System.Net;
using System.Collections.Generic;

using UnityEngine;
using UnityOSC;

public class LogInfo
{
    public string Log;
    public DateTime Timestamp;
}

/// <summary>
/// Models a log of a server composed by an OSCServer, a List of OSCPacket and a List of
/// strings that represent the current messages in the log.
/// </summary>
public struct ReceiverLog
{
	public OSCReciever receiver;
	public List<OSCPacket> packets;
	public List<LogInfo> log;
}

/// <summary>
/// Models a log of a client composed by an OSCClient, a List of OSCMessage and a List of
/// strings that represent the current messages in the log.
/// </summary>
public struct SenderLog
{
	public OSCSender sender;
	public List<OSCMessage> messages;
    public List<LogInfo> log;
}

/// <summary>
/// Handles all the OSC servers and clients of the current Unity game/application.
/// Tracks incoming and outgoing messages.
/// </summary>
public class OSCHandler : MonoBehaviour
{
	#region Singleton Constructors
	static OSCHandler()
	{
	}

	OSCHandler()
	{
        
	}

    public void Start()
    {
        OSCHandler.Instance.Init();
    }
	
	public static OSCHandler Instance 
	{
	    get 
		{
	        if (_instance == null) 
			{
				_instance = new GameObject ("OSCHandler").AddComponent<OSCHandler>();
	        }
	       
	        return _instance;
	    }
	}
	#endregion
	
	#region Member Variables
	private static OSCHandler _instance = null;
	private Dictionary<string, SenderLog> _senders = new Dictionary<string, SenderLog>();
	private Dictionary<string, ReceiverLog> _receivers = new Dictionary<string, ReceiverLog>();
    private List<LogInfo> sinceLastCheck = new List<LogInfo>();
	
	private const int _loglength = 25;
	#endregion
	
	/// <summary>
	/// Initializes the OSC Handler.
	/// Here you can create the OSC servers and clientes.
	/// </summary>
	public void Init()
	{
        //Initialize OSC clients (transmitters)
        //Example:		
        //CreateClient("SuperCollider", IPAddress.Parse("127.0.0.1"), 30000);

        //Initialize OSC servers (listeners)
        //Example:

        //CreateServer("AndroidPhone", 6666);		
	}
	
	#region Properties
	public Dictionary<string, SenderLog> Senders
	{
		get
		{
			return _senders;
		}
	}
	
	public Dictionary<string, ReceiverLog> Receivers
	{
		get
		{
			return _receivers;
		}
	}
	#endregion
	
	#region Methods
	
	/// <summary>
	/// Ensure that the instance is destroyed when the game is stopped in the Unity editor
	/// Close all the OSC clients and servers
	/// </summary>
	void OnApplicationQuit() 
	{
		foreach(KeyValuePair<string,SenderLog> pair in _senders)
		{
			pair.Value.sender.Close();
		}
		
		foreach(KeyValuePair<string,ReceiverLog> pair in _receivers)
		{
			pair.Value.receiver.Close();
		}
			
		_instance = null;
	}
	
	/// <summary>
	/// Creates an OSC Client (sends OSC messages) given an outgoing port and address.
	/// </summary>
	/// <param name="clientId">
	/// A <see cref="System.String"/>
	/// </param>
	/// <param name="destination">
	/// A <see cref="IPAddress"/>
	/// </param>
	/// <param name="port">
	/// A <see cref="System.Int32"/>
	/// </param>
	public void CreateSender(string clientId, IPAddress destination, int port)
	{
		SenderLog clientitem = new SenderLog();
		clientitem.sender = new OSCSender(destination, port);
		clientitem.log = new List<LogInfo>();
		clientitem.messages = new List<OSCMessage>();
		
		_senders.Add(clientId, clientitem);
		
		// Send test message
		string testaddress = "/test/alive/";
		OSCMessage message = new OSCMessage(testaddress, destination.ToString());
		message.Append(port); message.Append("OK");

        _senders[clientId].log.Add(new LogInfo { Timestamp = DateTime.UtcNow, Log = DataToString(message.Data) });
		
		_senders[clientId].messages.Add(message);
		
		_senders[clientId].sender.Send(message);
	}
	
	/// <summary>
	/// Creates an OSC Server (listens to upcoming OSC messages) given an incoming port.
	/// </summary>
	/// <param name="serverId">
	/// A <see cref="System.String"/>
	/// </param>
	/// <param name="port">
	/// A <see cref="System.Int32"/>
	/// </param>
	public void CreateReciever(string serverId, int port)
	{
		ReceiverLog serveritem = new ReceiverLog();
		serveritem.receiver = new OSCReciever(port);
		serveritem.log = new List<LogInfo>();
		serveritem.packets = new List<OSCPacket>();
		
		_receivers.Add(serverId, serveritem);
	}
	
	/// <summary>
	/// Sends an OSC message to a specified client, given its clientId (defined at the OSC client construction),
	/// OSC address and a single value. Also updates the client log.
	/// </summary>
	/// <param name="clientId">
	/// A <see cref="System.String"/>
	/// </param>
	/// <param name="address">
	/// A <see cref="System.String"/>
	/// </param>
	/// <param name="value">
	/// A <see cref="T"/>
	/// </param>
	public void SendMessageToClient<T>(string clientId, string address, T value)
	{
		List<object> temp = new List<object>();
		temp.Add(value);
		
		SendMessageToClient(clientId, address, temp);
	}
	
	/// <summary>
	/// Sends an OSC message to a specified client, given its clientId (defined at the OSC client construction),
	/// OSC address and a list of values. Also updates the client log.
	/// </summary>
	/// <param name="clientId">
	/// A <see cref="System.String"/>
	/// </param>
	/// <param name="address">
	/// A <see cref="System.String"/>
	/// </param>
	/// <param name="values">
	/// A <see cref="List<T>"/>
	/// </param>
	public void SendMessageToClient<T>(string clientId, string address, List<T> values)
	{	
		if(_senders.ContainsKey(clientId))
		{
			OSCMessage message = new OSCMessage(address);
		
			foreach(T msgvalue in values)
			{
				message.Append(msgvalue);
			}
			
			if(_senders[clientId].log.Count < _loglength)
			{
				_senders[clientId].log.Add(new LogInfo{Timestamp = DateTime.UtcNow, Log = DataToString(message.Data)});
				_senders[clientId].messages.Add(message);
			}
			else
			{
				_senders[clientId].log.RemoveAt(0);
				_senders[clientId].messages.RemoveAt(0);

                _senders[clientId].log.Add(new LogInfo { Timestamp = DateTime.UtcNow, Log = DataToString(message.Data) });

				_senders[clientId].messages.Add(message);
			}
			
			_senders[clientId].sender.Send(message);
		}
		else
		{
			Debug.LogError(string.Format("Can't send OSC messages to {0}. Client doesn't exist.", clientId));
		}
	}
	
	/// <summary>
	/// Updates clients and servers logs.
	/// </summary>
	public void UpdateLogs()
	{
		foreach(KeyValuePair<string,ReceiverLog> pair in _receivers)
		{
			if(_receivers[pair.Key].receiver.LastReceivedPacket != null)
			{
				//Initialization for the first packet received
				if(_receivers[pair.Key].log.Count == 0)
				{	
					_receivers[pair.Key].packets.Add(_receivers[pair.Key].receiver.LastReceivedPacket);
					var newLog = new LogInfo{Timestamp = DateTime.UtcNow, Log = DataToString(_receivers[pair.Key].receiver.LastReceivedPacket.Data)};
					_receivers[pair.Key].log.Add(newLog);
                    sinceLastCheck.Add(newLog);
					break;
				}
						
				if(_receivers[pair.Key].receiver.LastReceivedPacket.TimeStamp
				   != _receivers[pair.Key].packets[_receivers[pair.Key].packets.Count - 1].TimeStamp)
				{	
					if(_receivers[pair.Key].log.Count > _loglength - 1)
					{
						_receivers[pair.Key].log.RemoveAt(0);
						_receivers[pair.Key].packets.RemoveAt(0);
					}
		
					_receivers[pair.Key].packets.Add(_receivers[pair.Key].receiver.LastReceivedPacket);

                    var newLog = new LogInfo { Timestamp = DateTime.UtcNow, Log = DataToString(_receivers[pair.Key].receiver.LastReceivedPacket.Data) };
					_receivers[pair.Key].log.Add(newLog);
                    sinceLastCheck.Add(newLog);
				}
			}
		}
	}
	
	/// <summary>
	/// Converts a collection of object values to a concatenated string.
	/// </summary>
	/// <param name="data">
	/// A <see cref="List<System.Object>"/>
	/// </param>
	/// <returns>
	/// A <see cref="System.String"/>
	/// </returns>
	private string DataToString(List<object> data)
	{
		string buffer = "";
		
		for(int i = 0; i < data.Count; i++)
		{
			buffer += data[i].ToString() + " ";
		}
		
		buffer += "\n";
		
		return buffer;
	}

    /// <summary>
    /// Returns the logs created since last fetch
    /// </summary>
    /// <returns></returns>
    public List<LogInfo> FetchNewReceiverLogs()
    {
        var newLogs = new List<LogInfo>(sinceLastCheck);
        sinceLastCheck.Clear();
        return newLogs;
    }
	
	/// <summary>
	/// Formats a milliseconds number to a 000 format. E.g. given 50, it outputs 050. Given 5, it outputs 005
	/// </summary>
	/// <param name="milliseconds">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <returns>
	/// A <see cref="System.String"/>
	/// </returns>
	private string FormatMilliseconds(int milliseconds)
	{	
		if(milliseconds < 100)
		{
			if(milliseconds < 10)
				return String.Concat("00",milliseconds.ToString());
			
			return String.Concat("0",milliseconds.ToString());
		}
		
		return milliseconds.ToString();
	}
			
	#endregion
}	

