using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class CameraZoom1 : MonoBehaviour
{
    public PlayerMovement1 PlayerMovement;
    public CinemachineVirtualCamera VirtualCamera;

    [Range(0, 70)]
    public float minFOV = 50f;
    public float maxFOV = 70f;
    public float maxSpeed = 20f;
    public GameObject particleSpeed;
    public bool isShaking => PlayerMovement.currentVelocity >= 55f;     
    public float shakeAmplitude = 2.0f;
    public float shakeFrequency = 2.0f;
    public Volume postProcessVolume;

    private CinemachineComponentBase lensComponent;
    private CinemachineBasicMultiChannelPerlin perlin;
    public float lerpSpeed = 5f;


    public float minYOffset = 2f;
    public float maxYOffset = 1f;
    private CinemachineTransposer transposer;
    private ChromaticAberration chromaticAberration;
    void Start()
    {
        lensComponent = VirtualCamera.GetCinemachineComponent<CinemachineComponentBase>();
        transposer = VirtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        perlin = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Debug.Log(perlin);
        postProcessVolume.profile.TryGet(out chromaticAberration);
    }

    void Update()
    {
        float currentSpeed = PlayerMovement.currentVelocity;
        float targetFOV = Mathf.Lerp(minFOV, maxFOV, currentSpeed / maxSpeed);
        VirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(VirtualCamera.m_Lens.FieldOfView, targetFOV, lerpSpeed * Time.deltaTime);


        float targetYOffset = Mathf.Lerp(minYOffset, maxYOffset, currentSpeed / maxSpeed);
        Vector3 newOffset = transposer.m_FollowOffset;
        newOffset.y = Mathf.Lerp(newOffset.y, targetYOffset, lerpSpeed * Time.deltaTime);
        transposer.m_FollowOffset = newOffset;
       ShakeCamera();


    }

    void ShakeCamera()
    {
        
        if (perlin != null)
        {
            if (isShaking)
            {
                Debug.Log("Shlomi");
                perlin.m_AmplitudeGain = Mathf.Lerp(perlin.m_AmplitudeGain, shakeAmplitude, Time.deltaTime * 0.9f);
                perlin.m_FrequencyGain = Mathf.Lerp(perlin.m_FrequencyGain, shakeFrequency, Time.deltaTime * 0.9f);
                chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, 1f, Time.deltaTime * 2);
                particleSpeed.SetActive(true);
            }
            else
            {
                Debug.Log("Tal");
                perlin.m_AmplitudeGain = Mathf.Lerp(perlin.m_AmplitudeGain, 0f, Time.deltaTime * 5);
                perlin.m_FrequencyGain = Mathf.Lerp(perlin.m_FrequencyGain, 0f, Time.deltaTime * 5);
                chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, 0f, Time.deltaTime * 2);
                particleSpeed.SetActive(false);
            }
        }
    }
}
