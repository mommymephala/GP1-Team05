﻿using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;
using System.Collections;

public class BlackHoleTracker : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 12f;
    public float maxDistance = 20f;
    public float vignetteStartDistance = 10f;
    public float vignetteMaxIntensity = 1.0f;
    public CinemachineVirtualCamera deathCamera;
    public float deathDelay = 2f;
    private Rigidbody rb;
    public Material fullScreenMaterial;
    
    
    // Vignette and Chromatic Aberration
    private Volume globalVolume;
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;
    private bool isConsumingPlayer = false;
    private float vignetteTimer = 0f;
    private PlayerMovement1 playerMovement;
    public float vignetteDarkenDuration = 8f;
    public float speedThreshold = 10f;
    
    
    
    // Camera Shake
    private CinemachineBasicMultiChannelPerlin noise;
    public float maxShakeIntensity = 1f; // Maximum shake intensity near death

    private void Start()
    {
        globalVolume = FindObjectOfType<Volume>();
        playerMovement = player.GetComponent<PlayerMovement1>();
        rb = GetComponent<Rigidbody>();
        
        
        if (globalVolume == null)
        {
            Debug.LogError("Global Volume not found");
            return;
        }

        // Retrieve Vignette and Chromatic Aberration effects from Volume Profile
        if (!globalVolume.profile.TryGet(out vignette))
        {
            Debug.LogError("Vignette not found in Volume Profile");
        }
        else
        {
            vignette.intensity.value = 0f; // Start with no vignette effect
        }

        if (!globalVolume.profile.TryGet(out chromaticAberration))
        {
            Debug.LogError("Chromatic Aberration not found in Volume Profile");
        }
        else
        {
           // chromaticAberration.intensity.value = 0f; // Start with no chromatic aberration
        }

        // Retrieve Cinemachine Noise Component for Camera Shake
        if (deathCamera != null)
        {
            noise = deathCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (noise == null)
            {
                Debug.LogError("No Cinemachine Basic Multi-Channel Perlin component found on deathCamera.");
            }
        }
    }

    private void Update()
    {
        if (isConsumingPlayer || vignette == null || chromaticAberration == null) return;

        rb.velocity = new Vector3(0, 0, followSpeed);
        if (Vector3.Distance(transform.position, player.position) < maxDistance)
            transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z - maxDistance);

        float playerSpeed = playerMovement.currentVelocity;

        // Calculate high-pass filter cutoff based on vignette timer
        float normalizedVignetteIntensity = vignetteTimer / vignetteDarkenDuration;
        float highPassCutoff = Mathf.Lerp(10f, 1000f, normalizedVignetteIntensity); // Ramps up to 1000 Hz as vignette intensifies
        AudioManager.Instance.SetHighPassCutoff(highPassCutoff);

        if (playerSpeed <= speedThreshold)
        {
            vignetteTimer += Time.deltaTime;

            // Adjust vignette intensity
            float vignetteIntensity = Mathf.Lerp(0, vignetteMaxIntensity, vignetteTimer / vignetteDarkenDuration);
           
            vignette.intensity.value = vignetteIntensity;
           
            
            // Adjust chromatic aberration intensity
            float chromaticIntensity = Mathf.Lerp(0, 1f, vignetteTimer / vignetteDarkenDuration);

            // Trigger death camera effect if fully darkened
            if (vignetteTimer >= vignetteDarkenDuration)
            {
                StartDeathCameraEffect(player.GetComponent<Collider>());
            }
        }
        else
        {
            // Decrease vignette and chromatic aberration if player speeds up
            vignetteTimer -= Time.deltaTime;
            vignetteTimer = Mathf.Clamp(vignetteTimer, 0, vignetteDarkenDuration);

            vignette.intensity.value = Mathf.Lerp(0, vignetteMaxIntensity, vignetteTimer / vignetteDarkenDuration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartDeathCameraEffect(other);
        }
    }

    private void StartDeathCameraEffect(Collider player)
    {
        isConsumingPlayer = true;

        if (deathCamera != null)
        {
            deathCamera.Priority = 11; // Make the death camera active
        }

        StartCoroutine(DelayBeforeDeath(player));
    }

    private IEnumerator DelayBeforeDeath(Collider player)
    {
        float elapsedTime = 0f;

        while (elapsedTime < deathDelay)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / deathDelay;

            if (vignette != null)
            {
                vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, vignetteMaxIntensity, t);
            }

         //   if (chromaticAberration != null)
          //  {
               // chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, 1f, t);
          //  }

            yield return null;
        }

        player.GetComponent<PlayerDeath>().Die();
    }
}