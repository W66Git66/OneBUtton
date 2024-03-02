using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShaker : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin noise;
    private void OnEnable()
    {
        EventManager.OnCameraShake += ShakeRequest;
        EventManager.OutCameraShake += EndShake;
    }
    private void Awake()
    {
        noise = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void OnDisable()
    {
        EventManager.OnCameraShake -= ShakeRequest;
        EventManager.OutCameraShake -= EndShake;
    }

    private void ShakeRequest()
    {
        noise.m_AmplitudeGain = 1;
        noise.m_FrequencyGain = 1;
    }

    private void EndShake()
    {
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }
}
