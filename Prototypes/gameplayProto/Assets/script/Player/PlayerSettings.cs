using UnityEngine;
using System.Collections;

[System.Serializable] // show the properties in the editor when used as variable in player
public class PlayerSettings
{
	public float			jumpSpeed = 20;
	public float			maxJumpTime = 0.5f;
	
	public float			movementSpeed = 10;
	public float			maxMovementSpeed = 20;

	public float			floatDownSpeed = 1;
	public float			maxFloatDownSpeed = 1;
}
