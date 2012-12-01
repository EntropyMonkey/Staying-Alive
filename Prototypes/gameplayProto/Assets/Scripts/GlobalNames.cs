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
	}

	public class EVENT
	{
		public const string Player_Walking = "player is walking";
		public const string Player_JumpStart = "player starts jumping";
		public const string Player_JumpEnd = "player lands";
		public const string Pillar_Destroyed = "pillar destroyed";
		public const string Cookie_Collected = "cookie collected";
	}
}
