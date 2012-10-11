using UnityEngine;
using System.Collections;

[System.Serializable] // show the properties in the editor when used as variable in player
public class PlayerSettings
{
	public Vector2			jumpSpeed = new Vector2(10, 20);
	public float			maxJumpTime = 0.4f;
	
	public float			movementSpeed = 8;
	public float			maxMovementSpeed = 8;

	public float			floatDownSpeed = 1;
	public float			maxFloatDownSpeed = 1;
}
