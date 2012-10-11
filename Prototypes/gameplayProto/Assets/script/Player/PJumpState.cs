using UnityEngine;
using System.Collections;

public class PJumpState : FSMState<Player>
{
	private float jumpTimer;

	public override void Enter(Player player)
	{
		player.renderer.material.color = new Color(1, 0, 1);
		jumpTimer = 0;
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
		player.rigidbody.AddForce(
			Vector3.up * player.settings.jumpSpeed.y * Time.deltaTime,
			ForceMode.Impulse);

		if (Input.GetKey(KeyCode.D))
		{
			player.rigidbody.AddForce(
				Vector3.right * player.settings.jumpSpeed.x * Time.deltaTime,
				ForceMode.Impulse);
		}
		else if (Input.GetKey(KeyCode.A))
		{
			player.rigidbody.AddForce(
				Vector3.left * player.settings.jumpSpeed.x * Time.deltaTime,
				ForceMode.Impulse);
		}
	}

	public override void Exit(Player player)
	{
	}
}
