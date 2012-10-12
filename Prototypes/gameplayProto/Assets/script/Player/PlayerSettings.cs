using UnityEngine;
using System.Collections;

[System.Serializable] // show the properties in the editor when used as variable in player
public class PlayerSettings
{
	// influences how the player can change his velocity in midair
	public Vector2			jumpAcceleration = new Vector2(5, 5);
	// the maximum horizontal speed a player can have while jumping 
	// (influences how far the jump reaches)
	public float			maxHorizontalJumpVelocity = 2;
	// After this time, the jump is aborted, player cannot jump for as long
	// as he wants to
	public float			maxJumpTime = 0.4f;

	// the player can influence the horizontal movement while falling down
	public float			horizontalFallAcceleration = 5;
	
	// the movement acceleration when on the floor
	public float			movementAcceleration = 8;
	// the maximum velocity the player can have when he is on the floor
	public float			maxMovementVelocity = 8;

	// the downward acceleration when floating
	public float			verticalFloatAcceleration = 1;
	// the maximum velocity when floating down
	public float			maxVerticalFloatVelocity = 1;
}
