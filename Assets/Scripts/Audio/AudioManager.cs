using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;
	public int numOfPositionalSources = 100;

	public AudioMixerGroup mixerGroup;

	[SerializeField]
	private Sound[] sounds;

	private Queue<AudioSource> positionalSources = new Queue<AudioSource>();

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
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
	/// Get the integer ID from a Sound name
	/// </summary>
	/// <param name="name"></param>
	/// <returns>The ID of Sound with name. -1 if no such name.</returns>
	public int GetSoundID(string name)
    {
		int id = Array.FindIndex(sounds, sound => sound.Name == name);
		if (id == -1)
        {
			Debug.LogWarning("Sound with name: " + name + " does not exist!");
        }
		return id;
    }

	public AudioSource PlaySound(string soundName)
    {
		return PlaySound(GetSoundID(soundName));
    }

	public AudioSource PlaySound(int soundID)
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

	public AudioSource PlaySoundAtPosition(string soundName, Vector3 position)
    {
		return PlaySoundAtPosition(GetSoundID(soundName), position);
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
		positionalSources.Enqueue(source);
		return source;
	}

	public void StopPlaying(int soundID)
    {
		Sound sound = sounds[soundID];
		if (sound.source != null)
		{
			sound.source.volume = sound.volume * (1f + UnityEngine.Random.Range(-sound.volumeVariance / 2f, sound.volumeVariance / 2f));
			sound.source.pitch = sound.pitch * (1f + UnityEngine.Random.Range(-sound.pitchVariance / 2f, sound.pitchVariance / 2f));

			sound.source.Stop();
		}
	}

	public void StopPlaying(string soundName)
	{
		StopPlaying(GetSoundID(soundName));
	}
}
