using UnityEngine;
using System.Collections;

// player can change from the jump and the float state to the fall state
// when the player appears to be standing on a platform, he is in the fall
// state, because right now there is no difference between falling and 
// standing on a platform
public class PFallState : FSMState<Player>
{
	public override void Enter(Player player)
	{
		player.renderer.material.color = new Color(1, 0, 0);
	}

	public override void Execute(Player player)
	{
		if (player.Grounded)
		{
			player.FSM.ChangeState(player.StandState);
		}
		else if (Input.GetKey(KeyCode.E))
		{
			player.FSM.ChangeState(player.FloatState);
		}
	}

	public override void ExecuteFixed(Player player)
	{
		if (Input.GetKey(KeyCode.D))
		{
			player.rigidbody.AddForce(
				Vector3.right * player.settings.horizontalFallAcceleration * Time.deltaTime,
				ForceMode.Impulse);
		}
		else if (Input.GetKey(KeyCode.A))
		{
			player.rigidbody.AddForce(
				Vector3.left * player.settings.horizontalFallAcceleration * Time.deltaTime,
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
