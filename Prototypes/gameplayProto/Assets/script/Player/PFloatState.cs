using UnityEngine;
using System.Collections;

public class PFloatState : FSMState<Player>
{
	public override void Enter(Player player)
	{
		player.rigidbody.useGravity = false;
		Vector3 newVel = player.rigidbody.velocity;
		newVel.y = 0;
		player.rigidbody.velocity = newVel;
	}

	public override void Execute(Player player)
	{
		player.rigidbody.AddForce(
			Vector3.down * player.settings.floatDownSpeed * Time.deltaTime,
			ForceMode.Impulse);

		if (player.rigidbody.velocity.y < -player.settings.maxFloatDownSpeed)
		{
			Vector3 vel = player.rigidbody.velocity;
			vel.y = -player.settings.maxFloatDownSpeed;
			player.rigidbody.velocity = vel;
		}

		if (!Input.GetKey(KeyCode.E))
		{
			player.FSM.ChangeState(player.FallState);
		}
	}

	public override void Exit(Player player)
	{
		player.rigidbody.useGravity = true;
	}
}
