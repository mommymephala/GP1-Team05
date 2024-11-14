using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    private AudioHighPassFilter musicHighPassFilter;

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

        musicHighPassFilter = musicSource.GetComponent<AudioHighPassFilter>();
    }

    private void Start() 
    {
        // PlayMusic("GameplayMusic");
        // PlaySFX("WindAtmos");

        SetVolume();
        
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

    public void SetHighPassCutoff(float cutoff)
    {
        if (musicHighPassFilter != null)
        {
            musicHighPassFilter.cutoffFrequency = Mathf.Clamp(cutoff, 10f, 5000f); // Clamps to a safe range
        }
    }

    public void SetVolume()
    {
        if(PlayerPrefs.HasKey("Volume"))
            GetComponentInChildren<AudioSource>().volume = PlayerPrefs.GetFloat("Volume");
        
        
        
    }
}