using UnityEngine;
using System.Collections;

public class PStandState : FSMState<Player>
{
	public override void Enter(Player player)
	{
	}

	public override void Execute(Player player)
	{
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
		{
			player.FSM.ChangeGlobalState(player.WalkState);
		}
	}

	public override void  ExecuteFixed(Player player)
	{
	}

	public override void Exit(Player player)
	{
	}
}
