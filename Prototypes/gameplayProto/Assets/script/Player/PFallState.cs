using UnityEngine;
using System.Collections;

// player can change from the jump and the float state to the fall state
// when the player appears to be standing on a platform, he is in the fall
// state, because right now there is no difference between falling and 
// standing on a platform
public class PFallState : FSMState<Player>
{
	public override void Enter(Player player)
	{
	}

	public override void Execute(Player player)
	{
		if (player.Grounded && Input.GetKey(KeyCode.L))
		{
			player.FSM.ChangeState(player.JumpState);
		}
		
		if (!player.Grounded && Input.GetKey(KeyCode.E))
		{
			player.FSM.ChangeState(player.FloatState);
		}
	}

	public override void Exit(Player player)
	{
	}
}
