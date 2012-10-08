using UnityEngine;
using System.Collections;

public class PJumpState : FSMState<Player>
{
	private float jumpTimer;

	public override void Enter(Player player)
	{
		jumpTimer = 0;
	}

	public override void Execute(Player player)
	{
		jumpTimer += Time.deltaTime;

		if (jumpTimer > player.settings.maxJumpTime)
		{
			player.FSM.ChangeState(player.FallState);
		}

		if (Input.GetKey(KeyCode.E))
		{
			player.FSM.ChangeState(player.FloatState);
		}
	}

	public override void ExecuteFixed(Player player)
	{
		if (Input.GetKey(KeyCode.W))
		{
			player.rigidbody.AddForce(
				Vector3.up * player.settings.jumpSpeed * Time.deltaTime,
				ForceMode.Impulse);
		}
		else
		{
			player.FSM.ChangeState(player.FallState);
		}
	}

	public override void Exit(Player player)
	{
	}
}
