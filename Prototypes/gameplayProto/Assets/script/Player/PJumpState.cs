using UnityEngine;
using System.Collections;

public class PJumpState : FSMState<Player>
{
	private float jumpTimer;

	public override void Enter(Player player)
	{
		player.renderer.material.color = new Color(1, 0, 1);
		jumpTimer = 0;

		Vector3 vel = player.rigidbody.velocity;
		vel.y = player.settings.jumpAcceleration.y;
		player.rigidbody.velocity = vel;
	}

	public override void Execute(Player player)
	{
		jumpTimer += Time.deltaTime;

		if (jumpTimer > player.settings.maxJumpTime || !Input.GetKey(KeyCode.L))
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
		//player.rigidbody.AddForce(
		//    Vector3.up * player.settings.jumpSpeed.y * Time.deltaTime,
		//    ForceMode.Impulse);

		if (Input.GetKey(KeyCode.D))
		{
			player.rigidbody.AddForce(
				Vector3.right * player.settings.jumpAcceleration.x * Time.deltaTime,
				ForceMode.Impulse);
		}
		else if (Input.GetKey(KeyCode.A))
		{
			player.rigidbody.AddForce(
				Vector3.left * player.settings.jumpAcceleration.x * Time.deltaTime,
				ForceMode.Impulse);
		}

		if (Mathf.Abs(player.rigidbody.velocity.x) > player.settings.maxHorizontalJumpVelocity)
		{
			Vector3 vel = player.rigidbody.velocity;
			vel.x = player.settings.maxHorizontalJumpVelocity * (player.rigidbody.velocity.x > 0 ? 1 : -1);
			player.rigidbody.velocity = vel;
		}


	}

	public override void Exit(Player player)
	{
	}
}
