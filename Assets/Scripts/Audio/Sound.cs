using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "NewSound", menuName = "Sound")]
public class Sound : ScriptableObject
{
	[SerializeField]
    private string soundName;

    public AudioClip[] audioVariants;

	[Range(0f, 1f)]
	public float volume = 1f;
	[Range(0f, 1f)]
	public float volumeVariance = .1f;

	[Range(.1f, 3f)]
	public float pitch = 1f;
	[Range(0f, 1f)]
	public float pitchVariance = .1f;

	public float minimunDistance = 1f;
	public bool loop = false;

	public AudioMixerGroup mixerGroup;

	[HideInInspector]
	public AudioSource source;

	public AudioClip GetRandomAudioClip()
    {
		if (audioVariants.Length == 0)
        {
			return null;
        }
		return audioVariants[Random.Range(0, audioVariants.Length)];
    }

	public static int GetSoundID(string soundName)
	{
		return soundName.GetHashCode();
	}
}
