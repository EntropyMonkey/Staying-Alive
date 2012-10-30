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
            // Player 2 controls singing
		else if ((player.activePlayerInput == 2 && player.oscManager.Singing) || Input.GetKey(player.settings.DEBUG_KeySinging))
		{
			player.FSM.ChangeState(player.FloatState);
		}
	}

	public override void ExecuteFixed(Player player)
	{
		if (Input.GetKey(player.settings.KeyRight))
		{
			player.rigidbody.AddForce(
				Vector3.right * player.settings.HorizontalFallAcceleration * Time.deltaTime,
				ForceMode.Impulse);
		}
		else if (Input.GetKey(player.settings.KeyLeft))
		{
			player.rigidbody.AddForce(
				Vector3.left * player.settings.HorizontalFallAcceleration * Time.deltaTime,
				ForceMode.Impulse);
		}

		if (Mathf.Abs(player.rigidbody.velocity.x) > player.settings.MaxHorizontalJumpVelocity)
		{
			Vector3 vel = player.rigidbody.velocity;
			vel.x = player.settings.MaxHorizontalJumpVelocity * (player.rigidbody.velocity.x > 0 ? 1 : -1);
			player.rigidbody.velocity = vel;
		}
	}

	public override void Exit(Player player)
	{
	}
}
