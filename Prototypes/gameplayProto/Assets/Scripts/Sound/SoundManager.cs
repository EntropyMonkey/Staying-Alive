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

	public class PlayingSound
	{
		public string name; // same name as in clip list
		public GameObject soundObject; // the object which pseudo-emits the sound
		public GameObject soundSource; // the new gameobject with the audio component
	}

	public List<AudioData> clips; // the list containing the clips which can be played
	private Dictionary<string, PlayingSound> playingSounds; // all the sounds which are currently playing
	private Dictionary<string, PlayingSound> inactiveSounds; // all the sounds which have been used are pushed here and reused

	void Start()
	{
		playingSounds = new Dictionary<string, PlayingSound>();
		inactiveSounds = new Dictionary<string, PlayingSound>();

		// register for sound events
		Messenger<GameObject, bool>.AddListener(GlobalNames.EVENT.Player_Walking, PlayerWalking);
	}

	// onenabled is not called at the start of the game
	void OnEnabled()
	{
		Messenger<GameObject, bool>.AddListener(GlobalNames.EVENT.Player_Walking, PlayerWalking);
	}

	void OnDisabled()
	{
		// only receive events when the object is active
		Messenger<GameObject, bool>.RemoveListener(GlobalNames.EVENT.Player_Walking, PlayerWalking);
	}

	void Update()
	{

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
			StartCoroutine(Fade(
				playingSounds[GlobalNames.SOUND.Player_Walk],
				1.0f, 0.0f,
				1.0f));

			StartCoroutine(
				Stop(playingSounds[GlobalNames.SOUND.Player_Walk], 1.0f));
		}
	}

	PlayingSound Play(string name, GameObject soundObject, float volume = -1.0f, float pitch = -1.0f)
	{
		PlayingSound ps;

		if (!playingSounds.ContainsKey(name))
		{
			if (inactiveSounds.ContainsKey(name))
			{
				ps = inactiveSounds[name];
				inactiveSounds.Remove(name);
			}
			else
			{
				GameObject newSoundSource =
						new GameObject("Audio: " + name);
				newSoundSource.AddComponent<AudioSource>();
				ps = new PlayingSound
				{
					name = GlobalNames.SOUND.Player_Walk,
					soundSource = newSoundSource,
					soundObject = soundObject
				};
			}

			playingSounds.Add(ps.name, ps);

			ps.soundSource.active = true;
		}
		else
		{
			ps = playingSounds[name];
		}

		AudioData data = clips.Find(item => item.name == name);
		ps.soundSource.audio.clip = data.clip;
		ps.soundSource.audio.volume = volume < 0 ? data.volume : volume;
		ps.soundSource.audio.pitch = pitch < 0 ? data.pitch : pitch;

		ps.soundObject = soundObject;
		
		ps.soundSource.transform.position = ps.soundObject.transform.position;

		ps.soundSource.audio.Play();

		return ps;
	}

	IEnumerator Stop(PlayingSound sound, float time = 0.0f)
	{
		float currentTime = time;
		while ((currentTime -= Time.deltaTime) > 0)
		{
			yield return new WaitForEndOfFrame();
		}

		if (sound.soundSource.active)
		{
			sound.soundSource.audio.Stop();
			sound.soundSource.active = false;
			playingSounds.Remove(sound.name);
			inactiveSounds.Add(sound.name, sound);
		}
	}

	IEnumerator UpdatePosition(PlayingSound sound)
	{
		yield return new WaitForSeconds(Time.deltaTime);
		// if the sound is set inactive it does not follow the object anymore
		if (sound.soundSource.active)
		{
			sound.soundSource.transform.position = sound.soundObject.transform.position;
			UpdatePosition(sound);
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
				if (!sound.soundSource.active)
					break;

				i += step * Time.deltaTime;
				sound.soundSource.audio.volume = Mathf.Lerp(from, to, i);

				yield return new WaitForSeconds(step * Time.deltaTime);				
			}
		}
	}
}
