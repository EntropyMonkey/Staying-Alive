using UnityEngine;
using System.Collections;

public class PFloatState : PWalkState
{
	public override void Enter(Player player)
	{
		player.renderer.material.color = new Color(1, 1, 0);

		player.rigidbody.useGravity = false;
		Vector3 newVel = player.rigidbody.velocity;
		newVel.y = 0;
		player.rigidbody.velocity = newVel;
	}

	public override void Execute(Player player)
	{
		// dont execute walkstate execute, because there the state change to the jump state happens

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
			player.FSM.ChangeState(player.JumpState);
		}
	}

	public override void ExecuteFixed(Player player)
	{
		// sideways movement in PWalkState
		base.ExecuteFixed(player);
	}

	public override void Exit(Player player)
	{
		player.rigidbody.useGravity = true;
	}
}
