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
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
		{
			player.FSM.ChangeState(player.WalkState);
		}
		else if(Input.GetKey(KeyCode.L))
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
