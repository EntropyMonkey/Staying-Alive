using UnityEngine;
using System.Collections;

public class PWalkState : FSMState<Player>
{
	public override void Enter(Player player)
	{
		Messenger<GameObject, bool>.Invoke(
			GlobalNames.EVENT.Player_Walking, player.gameObject, true);
	}

	public override void Execute(Player player)
	{
		// transition to jump state
		if (player.JumpKeyReleased && Input.GetKey(player.settings.KeyJump))
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
		ExecuteMovement(player);

		// add gravity (just in case of fast changes between falling/walking)
		player.rigidbody.AddForce(Vector3.down * player.settings.Gravity);		
	}

	protected void ExecuteMovement(Player player)
	{
		// move r/l
		if (Input.GetKey(player.settings.KeyLeft))
		{
			player.transform.rotation = Quaternion.AngleAxis(180, new Vector3(0, 1, 0));

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
			player.transform.rotation = Quaternion.AngleAxis(0, new Vector3(0, 1, 0));

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
		Messenger<GameObject, bool>.Invoke(
			GlobalNames.EVENT.Player_Walking, player.gameObject, false);
	}
}
