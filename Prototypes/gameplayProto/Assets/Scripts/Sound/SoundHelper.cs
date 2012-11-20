using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadeHelper
{
	public enum FadeType { IN, OUT }

	// usage: StartCoroutine(Fade(source, 0.5f, FadeType.IN));
	public static IEnumerator Fade(AudioSource soundSource, float fadeTime, FadeType type)
	{
		float start = (type == FadeType.IN) ? 0.0f : 1.0f;
		float end = (type == FadeType.IN) ? 1.0f : 0.0f;

		float i = 0.0f;
		float step = 1.0f / fadeTime;

		while (i <= 1.0f)
		{
			i += step * Time.deltaTime;
			soundSource.volume = Mathf.Lerp(start, end, i);
			yield return new WaitForSeconds(step * Time.deltaTime);
		}
	}
}
