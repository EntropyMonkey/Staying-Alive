using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// http://www.blog.silentkraken.com/2010/04/06/audiomanager/

public class SoundManager : MonoBehaviour
{
	[System.Serializable]
	public class AudioData
	{
		public string name;
		public AudioClip clip;
		public float volume;
		public float pitch;
		[HideInInspector]
		public GameObject soundObject;
	}

	public List<AudioData> clips;

	void Start()
	{
		foreach (AudioData data in clips)
		{
			data.soundObject = new GameObject("Audio: " + data.name);
			data.soundObject.AddComponent<AudioSource>();
			data.soundObject.active = false;
		}
	}

	void Update()
	{

	}

	void Play(string clipName, Transform location, float volume = 1.0f)
	{
		AudioData data = clips.Find(item => item.name == clipName);
		if (data != null)
		{
			data.soundObject.transform.position = location.position;
			data.soundObject.transform.parent = location;

			data.soundObject.audio.clip = data.clip;
			data.soundObject.audio.volume = data.volume;
			data.soundObject.audio.pitch = data.pitch;
			data.soundObject.active = true;
			data.soundObject.audio.Play();

			StartCoroutine(DeactivateSoundObject(data.soundObject, data.clip.length));
		}
		else
		{
			Debug.LogWarning("Trying to play a sound which does not exist.");
		}
	}

	IEnumerator DeactivateSoundObject(GameObject soundObject, float time)
	{
		float currentTime = time;
		while (currentTime - Time.deltaTime > 0)
		{
			yield return new WaitForSeconds(Time.deltaTime);
		}

		soundObject.active = false;
	}
}
