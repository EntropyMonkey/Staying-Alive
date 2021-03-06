﻿using UnityEngine;
using System.Collections;

public class PFloatState : PWalkState
{
	public override void Enter(Player player)
	{
		Vector3 newVel = player.rigidbody.velocity;
		newVel.y = 0;
		player.rigidbody.velocity = newVel;

		player.FloatParticleSystem.emissionRate = player.settings.FloatParticleSpawnRate;

		Messenger<GameObject, string, bool>.Invoke(
			GlobalNames.EVENT.Player_Noise, player.gameObject,
			GlobalNames.SOUND.Player_Singing, false);
	}

	public override void Execute(Player player)
	{
		// dont execute walkstate execute, because there the state change to the jump state happens
		// and players should not be able to jump when in the float state

		if (player.rigidbody.velocity.y < -player.settings.MaxVerticalFloatVelocity)
		{
			Vector3 vel = player.rigidbody.velocity;
			vel.y = -player.settings.MaxVerticalFloatVelocity;
			player.rigidbody.velocity = vel;
		}

        if (!(player.PlayerTwoActive && 
			player.oscManager.Singing) && 
            !Input.GetKey(player.settings.DEBUG_KeySinging) || player.Grounded)
		{
			player.FSM.ChangeState(player.FallState);
		}
	}

	public override void ExecuteFixed(Player player)
	{
		// sideways movement in PWalkState
		ExecuteMovement(player); 
		
		// add different gravity when floating
		player.rigidbody.AddForce(
			 Vector3.down * player.settings.FloatGravity * Time.deltaTime,
			 ForceMode.Impulse);

	}

	public override void Exit(Player player)
	{
		player.FloatParticleSystem.emissionRate = 0;
		Messenger<GameObject, string, bool>.Invoke(
			GlobalNames.EVENT.Player_Noise, player.gameObject,
			GlobalNames.SOUND.Player_Singing, true);
	}
}
