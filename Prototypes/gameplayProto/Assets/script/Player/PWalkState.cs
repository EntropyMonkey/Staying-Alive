using UnityEngine;
using System.Collections;

public class PWalkState : FSMState<Player>
{
	public override void Enter(Player player)
	{
	}

	public override void Execute(Player player)
	{
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
