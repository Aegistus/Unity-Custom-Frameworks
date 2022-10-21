using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
	public static SoundManager Instance { get; private set; }
	public int numOfPositionalSources = 100;

	public AudioMixerGroup mixerGroup;

	[SerializeField]
	private Sound[] sounds;

	private Queue<AudioSource> positionalSources = new Queue<AudioSource>();

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			//DontDestroyOnLoad(gameObject);
		}
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.GetRandomAudioClip();
			s.source.loop = s.loop;
			s.source.outputAudioMixerGroup = mixerGroup;
		}
		Transform positionalSourceParent = new GameObject("Positional Audio Sources").transform;
        for (int i = 0; i < numOfPositionalSources; i++)
        {
			AudioSource newPositional = new GameObject().AddComponent<AudioSource>();
			newPositional.transform.parent = positionalSourceParent;
			newPositional.spatialBlend = 1;
			newPositional.maxDistance = 15;
			positionalSources.Enqueue(newPositional);
        }
	}

	/// <summary>
	/// Play sound with ID globally.
	/// </summary>
	/// <param soundID="soundID"></param>
	/// <returns>The AudioSource that is playing the sound. null if not found.</returns>
	public AudioSource PlaySoundGlobal(int soundID)
    {
		if (soundID < 0 || soundID >= sounds.Length)
		{
			Debug.LogWarning("Invalid Sound ID: " + soundID);
			return null;
		}
		Sound sound = sounds[soundID];

		sound.source.clip = sound.GetRandomAudioClip();
		sound.source.volume = sound.volume * (1f + UnityEngine.Random.Range(-sound.volumeVariance / 2f, sound.volumeVariance / 2f));
		sound.source.pitch = sound.pitch * (1f + UnityEngine.Random.Range(-sound.pitchVariance / 2f, sound.pitchVariance / 2f));

		sound.source.Play();
		return sound.source;
	}

	public AudioSource PlaySoundAtPosition(int soundID, Vector3 position)
    {
		if (soundID < 0 || soundID >= sounds.Length)
		{
			Debug.LogWarning("Invalid Sound ID: " + soundID);
			return null;
		}
		Sound sound = sounds[soundID];

		AudioSource source = positionalSources.Dequeue();
		source.volume = sound.volume * (1f + UnityEngine.Random.Range(-sound.volumeVariance / 2f, sound.volumeVariance / 2f));
		source.pitch = sound.pitch * (1f + UnityEngine.Random.Range(-sound.pitchVariance / 2f, sound.pitchVariance / 2f));
		source.minDistance = sound.minimunDistance;
		source.transform.position = position;
		source.clip = sound.GetRandomAudioClip();
		source.Play();
		source.transform.SetParent(null); // in case parent has been set before.
		positionalSources.Enqueue(source);
		return source;
	}

	public AudioSource PlaySoundAtPosition(int soundID, Vector3 position, Transform parent)
	{
		AudioSource source = PlaySoundAtPosition(soundID, position);
		source.transform.SetParent(parent);
		return source;
	}

	public void StopPlayingGlobal(int soundID)
    {
		Sound sound = sounds[soundID];
		if (sound.source != null)
		{
			sound.source.volume = sound.volume * (1f + UnityEngine.Random.Range(-sound.volumeVariance / 2f, sound.volumeVariance / 2f));
			sound.source.pitch = sound.pitch * (1f + UnityEngine.Random.Range(-sound.pitchVariance / 2f, sound.pitchVariance / 2f));

			sound.source.Stop();
		}
	}
}
