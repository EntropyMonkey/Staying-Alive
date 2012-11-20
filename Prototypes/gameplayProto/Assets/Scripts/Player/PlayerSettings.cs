using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] // show the properties in the editor when used as variable in player
public class PlayerSettings
{
	// influences how the player can change his velocity in midair
	public Vector2			JumpAcceleration = new Vector2(10, 5);
	// the maximum horizontal speed a player can have while jumping 
	// (influences how far the jump reaches)
	public float			MaxHorizontalJumpVelocity = 2.5f;
	// After this time, the jump is aborted, player cannot jump for as long
	// as he wants to
	public float			MaxJumpTime = 0.5f;
	public float			JumpTimeout = 0.2f;

    //Whistling information
    public float            MaxExpandingTime = 2F;
    public float            MaxWhistlingScale = 22F;
    public float            DeflateTime = 0.5F;
    public float            RockAlphaMin = 0.3f;

	//How long an object should be affected by the shush
    public float            ShushPauseDuration = 5f; 

	// the player can influence the horizontal movement while falling down
	public float			HorizontalFallAcceleration = 5;
	
	// the movement acceleration when on the floor
	public float			MovementAcceleration = 15;
	// the maximum velocity the player can have when he is on the floor
	public float			MaxMovementVelocity = 3;

	// the downward acceleration when floating
	public float			VerticalFloatAcceleration = 1;
	// the maximum velocity when floating down
	public float			MaxVerticalFloatVelocity = 1;

	public int				FloatParticleSpawnRate = 7;

	// the maximum angle a floor can have for the player to not fall off
	public float			MaxFloorAngle = 10;

	public KeyCode			KeyJump = KeyCode.L;
	public KeyCode			KeyRight = KeyCode.D;
	public KeyCode			KeyLeft = KeyCode.A;
	public KeyCode			DEBUG_KeyShout = KeyCode.E;
	public KeyCode			DEBUG_KeySinging = KeyCode.Q;
	public KeyCode			DEBUG_KeyWhistling = KeyCode.R;
    public KeyCode          DEBUG_KeyShushing= KeyCode.T;
    public KeyCode          KeyPlayer1Input = KeyCode.LeftShift;
    public KeyCode          KeyPlayer2Input = KeyCode.RightShift;

	public float			MaxHealthPoints = 1;
}
