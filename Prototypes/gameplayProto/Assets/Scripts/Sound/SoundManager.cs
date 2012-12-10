
//#define DEBUG_SOUNDS

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// http://www.blog.silentkraken.com/2010/04/06/audiomanager/

public class SoundManager : MonoBehaviour
{
	[System.Serializable]
	public class AudioData
	{
		public string name; // should be same name as in global names file
		public AudioClip clip; // the clip needs to be set in the editor!
		public float volume = 1.0f;
		public float pitch = 1.0f;
	}

	[System.Serializable]
	public class PlayingSound
	{
		public string name; // same name as in clip list
		public GameObject soundObject; // the object which pseudo-emits the sound
		public GameObject soundSource; // the new gameobject with the audio component
	}

	public List<AudioData> clips; // the list containing the clips which can be played
	
	public Dictionary<string, PlayingSound> playingSounds; // all the sounds which are currently playing

	void Start()
	{
		playingSounds = new Dictionary<string, PlayingSound>();

		// register for sound events
		Messenger<GameObject, bool>.AddListener(GlobalNames.EVENT.Player_Walking, PlayerWalking);
		Messenger<GameObject>.AddListener(GlobalNames.EVENT.Player_JumpStart, PlayerJumpStart);
		Messenger<GameObject>.AddListener(GlobalNames.EVENT.Player_EnterStand, PlayerJumpEnd);
		Messenger<GameObject>.AddListener(GlobalNames.EVENT.Pillar_Destroyed, PillarDestroyed);
		Messenger<GameObject>.AddListener(GlobalNames.EVENT.Cookie_Collected, CookieCollected);
		Messenger<GameObject, string, bool>.AddListener(GlobalNames.EVENT.Player_Noise, PlayerMakingNoise);
	}

	void OnDisabled()
	{
		// only receive events when the object is active
		Messenger<GameObject, bool>.RemoveListener(GlobalNames.EVENT.Player_Walking, PlayerWalking);
		Messenger<GameObject>.RemoveListener(GlobalNames.EVENT.Player_JumpStart, PlayerJumpStart);
		Messenger<GameObject>.RemoveListener(GlobalNames.EVENT.Player_EnterStand, PlayerJumpEnd);
		Messenger<GameObject>.RemoveListener(GlobalNames.EVENT.Pillar_Destroyed, PillarDestroyed);
		Messenger<GameObject>.RemoveListener(GlobalNames.EVENT.Cookie_Collected, CookieCollected);
		Messenger<GameObject, string, bool>.RemoveListener(GlobalNames.EVENT.Player_Noise, PlayerMakingNoise);
	}

	void Update()
	{
	}

	void PlayerJumpStart(GameObject player)
	{
		PlayingSound ps = Play(GlobalNames.SOUND.Player_Jump, player);
		
		StartCoroutine(Stop(ps, ps.soundSource.audio.clip.length));
	}

	void PlayerJumpEnd(GameObject player)
	{
		PlayingSound ps = Play(GlobalNames.SOUND.Player_Land, player);
		// HACK this event is called even after the component has been destroyed
		if (this != null)
			StartCoroutine(Stop(ps, ps.soundSource.audio.clip.length));
	}

	void PlayerMakingNoise(GameObject player, string sound, bool stop)
	{
		if (!stop)
		{
			PlayingSound ps = Play(sound, player);
			ps.soundSource.audio.loop = true;
			StartCoroutine(UpdatePosition(ps));
		}
		else if (playingSounds.ContainsKey(sound))
		{
			PlayingSound ps = playingSounds[sound];
			StartCoroutine(Fade(
				ps, 
				clips.Find(item =>item.name == sound).volume, 0,
				ps.soundSource.audio.clip.length));
			StartCoroutine(Stop(ps, ps.soundSource.audio.clip.length));
		}
	}

	void PillarDestroyed(GameObject pillar)
	{
		PlayingSound ps = Play(GlobalNames.SOUND.Pillar_Destroyed, pillar);
		StartCoroutine(Stop(ps, ps.soundSource.audio.clip.length));
	}

	void CookieCollected(GameObject player)
	{
		PlayingSound ps = 
			Play(GlobalNames.SOUND.Cookie_Collected + Random.Range(1, 5),
			player);
		StartCoroutine(UpdatePosition(ps));
		StartCoroutine(Stop(ps, ps.soundSource.audio.clip.length));
	}

	void PlayerWalking(GameObject player, bool startWalking)
	{
		if (startWalking)
		{
			PlayingSound ps = Play(GlobalNames.SOUND.Player_Walk, player);

			ps.soundSource.audio.loop = true;
			// use this for moving sounds
			StartCoroutine(UpdatePosition(ps));
			// fade from 0 to 1 in 1 second
			StartCoroutine(Fade(ps, 0.0f, 1.0f, 1.0f));
		}
			// if the walking sound is still playing but the player stopped
			// walking
		else if (playingSounds.ContainsKey(GlobalNames.SOUND.Player_Walk))
		{
			StartCoroutine(
				Stop(playingSounds[GlobalNames.SOUND.Player_Walk]));
		}
	}

	PlayingSound Play(string name, GameObject soundObject, float volume = -1.0f, float pitch = -1.0f)
	{
#if DEBUG_SOUNDS
		Debug.Log("play sound: " + name);
#endif
		PlayingSound ps;
		if (!playingSounds.ContainsKey(name))
		{
			GameObject newSoundSource =
					new GameObject("Audio: " + name);
			newSoundSource.AddComponent<AudioSource>();

			ps = new PlayingSound
			{
				name = name,
				soundSource = newSoundSource,
				soundObject = soundObject
			};

			playingSounds.Add(ps.name, ps);

			ps.soundSource.active = true;

			AudioData data = clips.Find(item => item.name == name);
			ps.soundSource.audio.clip = data.clip;
			ps.soundSource.audio.volume = volume < 0 ? data.volume : volume;
			ps.soundSource.audio.pitch = pitch < 0 ? data.pitch : pitch;

			ps.soundSource.transform.position = ps.soundObject.transform.position;

			ps.soundSource.audio.Play();
		}
		else
		{
			ps = playingSounds[name];
		}

		return ps;
	}

	IEnumerator Stop(PlayingSound sound, float time = 0.0f)
	{
		float currentTime = time;
		while ((currentTime -= Time.deltaTime) > 0)
		{
			yield return new WaitForEndOfFrame();
		}

		if (sound.soundSource)
		{
			sound.soundSource.audio.Stop();
			sound.soundSource.active = false;
			Destroy(sound.soundSource);
			playingSounds.Remove(sound.name);
#if DEBUG_SOUNDS
			Debug.Log("stop sound: " + sound.name);
#endif
		}
	}

	IEnumerator UpdatePosition(PlayingSound sound)
	{
		// if the sound is set inactive it does not follow the object anymore
		if (sound.soundSource != null)
		{
			sound.soundSource.transform.position = sound.soundObject.transform.position;
			yield return new WaitForEndOfFrame();
			StartCoroutine(UpdatePosition(sound));
		}
	}

	IEnumerator Fade(PlayingSound sound, float from, float to, float time)
	{
		if (sound.soundSource.active)
		{
			float step = 1.0f / time;
			float i = 0.0f;

			while (i <= 1.0f)
			{
				// stop fading if sound source has been deactivated
				if (sound.soundSource == null || !sound.soundSource.active)
					break;

				i += step * Time.deltaTime;
				sound.soundSource.audio.volume = Mathf.Lerp(from, to, i);

#if DEBUG_SOUNDS
				Debug.Log("fade sound: " + sound.name + " volume at " + sound.soundSource.audio.volume);
#endif

				yield return new WaitForSeconds(step * Time.deltaTime);				
			}
		}
	}
}
