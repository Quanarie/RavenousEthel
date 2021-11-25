using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private List<Sound> sounds;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
        DontDestroyOnLoad(gameObject);

        Play("Theme");
    }

    public void Play(string name)
    {
        Sound sound  = Array.Find(sounds.ToArray(), sound => sound.name == name);

        if (sound == null)
        {
            Debug.LogWarning("Not found this sound: " + name);
        }

        sound.source.Play();
    }
}
