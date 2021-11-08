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

	public Sound[] sounds;

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

	public void PlaySound(string soundGroupName)
    {
		if (soundGroupName == "")
		{
			return;
		}
		Sound group = Array.Find(sounds, item => item.nameID == soundGroupName);
		if (group == null)
		{
			Debug.LogWarning("Sound Group: " + soundGroupName + " not found!");
			return;
		}

		group.source.clip = group.GetRandomAudioClip();
		group.source.volume = group.volume * (1f + UnityEngine.Random.Range(-group.volumeVariance / 2f, group.volumeVariance / 2f));
		group.source.pitch = group.pitch * (1f + UnityEngine.Random.Range(-group.pitchVariance / 2f, group.pitchVariance / 2f));

		group.source.Play();
	}

	public void PlaySoundAtPosition(string soundGroupName, Vector3 position)
    {
		if (soundGroupName == "")
		{
			return;
		}
		Sound group = Array.Find(sounds, item => item.nameID == soundGroupName);
		if (group == null)
		{
			Debug.LogWarning("Sound Group: " + soundGroupName + " not found!");
			return;
		}

		group.source.volume = group.volume * (1f + UnityEngine.Random.Range(-group.volumeVariance / 2f, group.volumeVariance / 2f));
		group.source.minDistance = group.minimunDistance;

		AudioSource source = positionalSources.Dequeue();
		source.pitch = group.pitch * (1f + UnityEngine.Random.Range(-group.pitchVariance / 2f, group.pitchVariance / 2f));
		source.transform.position = position;
		source.clip = group.GetRandomAudioClip();
		source.Play();
		positionalSources.Enqueue(source);
	}

	public void PlaySoundAtPosition(AudioClip sound, Vector3 position)
    {
		AudioSource source = positionalSources.Dequeue();
		source.pitch = 1;
		source.transform.position = position;
		source.clip = sound;
		source.Play();
		positionalSources.Enqueue(source);
	}

	public void PlaySoundAtPosition(Sound sound, Vector3 position)
    {
		AudioSource source = positionalSources.Dequeue();
		source.pitch = sound.pitch;
		source.volume = sound.volume;
		source.transform.position = position;
		source.clip = sound.GetRandomAudioClip();
		source.Play();
		positionalSources.Enqueue(source);
	}

	public void StopPlaying(string soundName)
	{
		Sound sound = Array.Find(sounds, item => item.nameID == soundName);
		if (sound == null)
		{
			Debug.LogWarning("Sound: " + soundName + " not found!");
			return;
		}
		if (soundName == "")
		{
			return;
		}

		if (sound.source != null)
        {
			sound.source.volume = sound.volume * (1f + UnityEngine.Random.Range(-sound.volumeVariance / 2f, sound.volumeVariance / 2f));
			sound.source.pitch = sound.pitch * (1f + UnityEngine.Random.Range(-sound.pitchVariance / 2f, sound.pitchVariance / 2f));

			sound.source.Stop();
		}
	}
}
