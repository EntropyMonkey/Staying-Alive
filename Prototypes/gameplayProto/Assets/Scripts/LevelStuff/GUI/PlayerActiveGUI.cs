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
		switch (player.activePlayerInput)
		{
			case Player.PlayerActive.BOTH:
				renderer.material.mainTexture = BothPlayersActiveTexture;
				break;
			case Player.PlayerActive.PLAYER_ONE:
				renderer.material.mainTexture = PlayerOneActiveTexture;
				break;
			case Player.PlayerActive.PLAYER_TWO:
				renderer.material.mainTexture = PlayerTwoActiveTexture;
				break;
			default:
				renderer.material.mainTexture = NoPlayerActiveTexture;
				break;
		}
	}
}
