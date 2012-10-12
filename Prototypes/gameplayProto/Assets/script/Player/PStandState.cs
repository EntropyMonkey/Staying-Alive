using UnityEngine;
using System.Collections;

public class PStandState : FSMState<Player>
{
	public override void Enter(Player player)
	{
		player.renderer.material.color = new Color(0, 0, 0);
	}

	public override void Execute(Player player)
	{
		if (Input.GetKey(player.settings.KeyLeft) || Input.GetKey(player.settings.KeyRight))
		{
			player.FSM.ChangeState(player.WalkState);
		}
		else if(player.CanJump && Input.GetKey(player.settings.KeyJump))
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
	}

	public override void Exit(Player player)
	{
	}
}
