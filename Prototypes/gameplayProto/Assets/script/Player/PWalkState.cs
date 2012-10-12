using UnityEngine;
using System.Collections;

public class PWalkState : FSMState<Player>
{
	public override void Enter(Player player)
	{
		player.renderer.material.color = new Color(0, 1, 1);
	}

	public override void Execute(Player player)
	{
		// transition to jump state
		if (player.CanJump && Input.GetKey(player.settings.KeyJump))
		{
			player.FSM.ChangeState(player.JumpState);
		}

		// transition to stand state
		float epsilon = 0.1f;
		if (Mathf.Abs(player.rigidbody.velocity.x) < epsilon)
		{
			player.FSM.ChangeState(player.StandState);
		}

		// transition to fall state
		if (!player.Grounded)
		{
			player.FSM.ChangeState(player.FallState);
		}
	}

	public override void ExecuteFixed(Player player)
	{
		// move r/l
		if (Input.GetKey(player.settings.KeyLeft))
		{
			player.rigidbody.AddForce(
				Vector3.left * player.settings.MovementAcceleration * Time.deltaTime, 
				ForceMode.Impulse);

			if (player.rigidbody.velocity.x < -player.settings.MaxMovementVelocity)
			{
				Vector3 vel = player.rigidbody.velocity;
				vel.x = -player.settings.MaxMovementVelocity;
				player.rigidbody.velocity = vel;
			}
		}
		else if (Input.GetKey(player.settings.KeyRight))
		{
			player.rigidbody.AddForce(
				Vector3.right * player.settings.MovementAcceleration * Time.deltaTime,
				ForceMode.Impulse);

			if (player.rigidbody.velocity.x > player.settings.MaxMovementVelocity)
			{
				Vector3 vel = player.rigidbody.velocity;
				vel.x = player.settings.MaxMovementVelocity;
				player.rigidbody.velocity = vel;
			}
		}
	}

	public override void Exit(Player player)
	{
	}
}
