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
	}

	public class NAME
	{
		public const string OSCSenderName = "Kunigunde";
		public const string OSCReceiverName = "Willibald";
	}

	public class SOUND
	{
		public const string JumpStart = "jumpStart";
		public const string JumpExit = "jumpExit";
	}
}
