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

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityOSC;

using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Helper to monitor incoming at outgoing OSC messages from the Unity Editor.
/// You should have this script placed at the /Editor folder.
/// Show the panel helper by selecting "Window->OSC Helper" from the Unity menu.
/// </summary>
public class OSCHelper 
	#if UNITY_EDITOR
	: EditorWindow
#endif
{
	#region Member variables
	private string _status = "";
	private string _selected = "none";
	private List<string> _output = new List<string>();
	private int _portselected = 0;
	
	private Dictionary<string, SenderLog> _senders = new Dictionary<string, SenderLog>();
	private Dictionary<string, ReceiverLog> _receivers = new Dictionary<string, ReceiverLog>();
	#endregion
	
	#if UNITY_EDITOR
	/// <summary>
	/// Initializes the OSC Helper and creates an entry in the Unity menu.
	/// </summary>
	[MenuItem("Window/OSC Helper")]
	static void Init ()
	{
		OSCHelper window = (OSCHelper)EditorWindow.GetWindow (typeof(OSCHelper));
		window.Show();
	}
	#endif
	
	/// <summary>
	/// Executes OnGUI in the panel within the Unity Editor
	/// </summary>
	void OnGUI ()
	{	
		#if UNITY_EDITOR
		if(EditorApplication.isPlaying)
		{
			_status = "";
			GUILayout.Label(_status, EditorStyles.boldLabel);
			GUILayout.Label(String.Concat("SELECTED: ", _selected));
			
			_senders = OSCHandler.Instance.Senders;//Get the clients
			_receivers = OSCHandler.Instance.Receivers;//Get the servers
			
			foreach(KeyValuePair<string, SenderLog> pair in _senders)
			{
				if(GUILayout.Button(String.Format("Sender '{0}' port: {1}", pair.Key, pair.Value.sender.Port)))
				{
					_selected = pair.Key;
					_portselected = pair.Value.sender.Port;
				}
			}
			
			foreach(KeyValuePair<string, ReceiverLog> pair in _receivers)
			{
				if(GUILayout.Button(String.Format("Receiver '{0}' port: {1}", pair.Key, pair.Value.receiver.LocalPort)))
				{
					_selected = pair.Key;
					_portselected = pair.Value.receiver.LocalPort;
				}
			}
			
			GUILayout.TextArea(FromListToString(_output));
		}
		else
		{
			_status = "\n Enter the play mode in the Editor to see \n running senders and receivers";
			GUILayout.Label(_status, EditorStyles.boldLabel);
		}
#endif
	}
	
	/// <summary>
	/// Updates the logs of the running clients and servers.
	/// </summary>
	void Update()
	{
		#if UNITY_EDITOR
		if(EditorApplication.isPlaying)
		{
			OSCHandler.Instance.UpdateLogs();
			
			if(_senders.ContainsKey(_selected) && _senders[_selected].sender.Port == _portselected)
			{
                _output = _senders[_selected].log.ConvertAll(p => p.Log);
			}
			else if(_receivers.ContainsKey(_selected) && _receivers[_selected].receiver.LocalPort == _portselected)
			{
				_output = _receivers[_selected].log.ConvertAll(p => p.Log);
			}
			
			Repaint();
		}
#endif
	}
	
	/// <summary>
	/// Formats a collection of strings to a single concatenated string.
	/// </summary>
	/// <param name="input">
	/// A <see cref="List<System.String>"/>
	/// </param>
	/// <returns>
	/// A <see cref="System.String"/>
	/// </returns>
	private string FromListToString(List<string> input)
	{
        StringBuilder sb = new StringBuilder(input.Count);
		
		foreach(string value in input)
		{
            sb.Append(value);
		}
		
		return sb.ToString();	
	}
}