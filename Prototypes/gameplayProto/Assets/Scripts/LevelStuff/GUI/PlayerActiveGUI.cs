using UnityEngine;
using System.Collections;

public class PlayerActiveGUI : MonoBehaviour
{

	public Texture PlayerOneActiveTexture;
	public Texture PlayerTwoActiveTexture;
	public Texture NoPlayerActiveTexture;
	public Texture BothPlayersActiveTexture;

	private GUIStyle playerActiveStyle;
	private bool player1Active, player2Active;

	private Player player;

	// Use this for initialization
	void Start()
	{
		playerActiveStyle = new GUIStyle();

		player = GameObject.FindGameObjectWithTag(GlobalNames.TAG.Player).GetComponent<Player>();
	}

	// Update is called once per frame
	void Update()
	{
		if (player.BothPlayersActive)
		{
			renderer.material.mainTexture = BothPlayersActiveTexture;
		}
		else if (player.PlayerOneActive)
		{
			renderer.material.mainTexture = PlayerOneActiveTexture;
		}
		else if (player.PlayerTwoActive)
		{
			renderer.material.mainTexture = PlayerTwoActiveTexture;
		}
		else
		{
			renderer.material.mainTexture = NoPlayerActiveTexture;
		}
	}
}
