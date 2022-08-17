using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    float masterVolume = 1.0f;

    public static AudioManager instance;

    void Awake()
    {
        if (AudioManager.instance == null)
        {
            AudioManager.instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume * masterVolume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    void Start()
    {
        this.Play("Forest");
        StartCoroutine(DelayMusic());
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        if (sound != null)
        {
            sound.source.Play();
        }
        else
        {
            Debug.Log("Tried to play sound that doesn't exist!");
        }
    }

    public void Pause(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        if (sound != null)
        {
            sound.source.Pause();
        }
        else
        {
            Debug.Log("Tried to pause sound that doesn't exist!");
        }
    }

    public void UpdateMasterVolume(float volume)
    {
        masterVolume = volume;
        Debug.Log("Slider updated: " + volume);

        foreach (Sound sound in sounds)
        {
            sound.source.volume = sound.volume * masterVolume;
        }
    }

    IEnumerator DelayMusic()
    {
        yield return new WaitForSeconds(5.0f);

        this.Play("Music");
    }
}