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
		if (Input.GetKey(KeyCode.L))
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
		if (Input.GetKey(KeyCode.A))
		{
			player.rigidbody.AddForce(
				Vector3.left * player.settings.movementSpeed * Time.deltaTime, 
				ForceMode.Impulse);

			if (player.rigidbody.velocity.x < -player.settings.maxMovementSpeed)
			{
				Vector3 vel = player.rigidbody.velocity;
				vel.x = -player.settings.maxMovementSpeed;
				player.rigidbody.velocity = vel;
			}
		}
		else if (Input.GetKey(KeyCode.D))
		{
			player.rigidbody.AddForce(
				Vector3.right * player.settings.movementSpeed * Time.deltaTime,
				ForceMode.Impulse);

			if (player.rigidbody.velocity.x > player.settings.maxMovementSpeed)
			{
				Vector3 vel = player.rigidbody.velocity;
				vel.x = player.settings.maxMovementSpeed;
				player.rigidbody.velocity = vel;
			}
		}
	}

	public override void Exit(Player player)
	{
	}
}
