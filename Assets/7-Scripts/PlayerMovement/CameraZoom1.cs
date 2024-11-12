using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom1 : MonoBehaviour
{
    public PlayerMovement1 PlayerMovement;
    public CinemachineVirtualCamera VirtualCamera;

    [Range(0, 70)]
    public float minFOV = 50f;
    public float maxFOV = 70f;
    public float maxSpeed = 20f;
    private CinemachineComponentBase lensComponent;

    public float lerpSpeed = 5f;


    public float minYOffset = 2f;
    public float maxYOffset = 1f;
    private CinemachineTransposer transposer;

    void Start()
    {
        lensComponent = VirtualCamera.GetCinemachineComponent<CinemachineComponentBase>();
        transposer = VirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
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
    }
}
