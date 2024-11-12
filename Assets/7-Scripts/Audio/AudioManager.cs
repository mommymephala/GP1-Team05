using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() 
    {
        PlayMusic("PhotonDashGameplay");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Music sound not found: " + name);
            return;
        }

        musicSource.clip = s.clip;
        musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("SFX sound not found: " + name);
            return;
        }

        sfxSource.PlayOneShot(s.clip);
    }

    public void Play3DSoundOnObject(string name, GameObject targetObject)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("3D sound not found: " + name);
            return;
        }

        AudioSource audioSource = targetObject.GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.clip = s.clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource not found on target object: " + targetObject.name);
        }
    }
}