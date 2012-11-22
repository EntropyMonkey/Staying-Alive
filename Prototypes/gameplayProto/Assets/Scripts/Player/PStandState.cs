using UnityEngine;
using System.Collections;

public class PStandState : FSMState<Player>
{
	public override void Enter(Player player)
	{
	}

	public override void Execute(Player player)
	{
		if (Input.GetKey(player.settings.KeyLeft) || Input.GetKey(player.settings.KeyRight))
		{
			player.FSM.ChangeState(player.WalkState);
		}
		else if(player.JumpKeyReleased && Input.GetKey(player.settings.KeyJump))
		{
			player.FSM.ChangeState(player.JumpState);
		}

		if (!player.Grounded)
		{
			player.FSM.ChangeState(player.FallState);
		}
	}

	public override void  ExecuteFixed(Player player)
	{
		// add gravity (in case of jittering when changing between standing/falling)
		player.rigidbody.AddForce(Vector3.down * player.settings.Gravity);
	}

	public override void Exit(Player player)
	{
	}
}
