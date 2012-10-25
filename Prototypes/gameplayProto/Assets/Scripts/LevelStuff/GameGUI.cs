using System;
using UnityEngine;

public class GameGUI : MonoBehaviour
{
	public Rect pointsPosition = new Rect(100, 100, 50, 50);

	public Font standardFont;
	public int fontSize = 50;

	private Player player;

	void Start()
	{
		GameObject go = GameObject.FindGameObjectWithTag(GlobalNames.TAG.Player);
		if (go == null)
		{
			Debug.LogError("GUI.Start: Could not find a player in the scene.");
		}
		else
		{
			player = go.GetComponent<Player>();
		}
	}

	void OnGUI()
	{
		GUIStyle style = new GUIStyle();
		style.font = standardFont;
		style.fontStyle = FontStyle.Normal;
		style.fontSize = fontSize;

		GUI.Label(pointsPosition, player.Points.ToString(), style);
	}
}