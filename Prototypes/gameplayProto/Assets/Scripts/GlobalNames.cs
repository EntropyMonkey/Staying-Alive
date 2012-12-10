using UnityEngine;
using System.Collections;


/// <summary>
/// Used for names, tags, layers etc
/// </summary>
public class GlobalNames
{
	public class TAG
	{
		public const string Player = "Player";
		public const string StartPoint = "StartPoint";
        public const string ShoutingTrigger = "ShoutingTrigger";
        public const string WhistlingTrigger = "WhistlingTrigger";
        public const string ShushTrigger = "ShushTrigger";
		public const string MovingPlatformTag = "MovingPlatform";
		public const string FloatParticleSystem = "FloatParticleSystem";
		public const string ShoutParticleSystem = "ShoutParticleSystem";
	}

	public class NAME
	{
		public const string OSCSenderName = "Kunigunde";
		public const string OSCReceiverName = "Willibald";
	}

	// the names for the sounds
	public class SOUND
	{
		public const string JumpStart = "jumpStart";
		public const string JumpExit = "jumpExit";
		public const string Player_Walk = "Player_Walk";
		public const string Player_Jump = "Player_Jump";
		public const string Player_Land = "Player_Land";
		public const string Pillar_Destroyed = "Pillar_Destroyed";
		public const string Cookie_Collected = "Cookie_Collected";
		public const string Player_Whistling = "Player_Whistling";
		public const string Player_Shushing = "Player_Shushing";
		public const string Player_Shouting = "Player_Shouting";
		public const string Player_Singing = "Player_Singing";
	}

	public class ANIM
	{
		public const string Player_Walk = "walk";
		public const string Player_Idle = "idle";
	}

	public class EVENT
	{
		public const string Player_Walking = "player is walking";
		public const string Player_JumpStart = "player starts jumping";
		public const string Player_EnterStand = "player enters stand state";
		public const string Pillar_Destroyed = "pillar destroyed";
		public const string Cookie_Collected = "cookie collected";
		public const string Player_Noise = "player is making noise";
	}
}
