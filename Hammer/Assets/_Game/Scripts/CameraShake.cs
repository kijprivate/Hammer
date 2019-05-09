using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    Hammer hammer;
    float amplitude;

    private void Awake()
    {
        EventManager.EventHammerHit += OnHammerHit;
    }

    private void OnDestroy()
    {
        EventManager.EventHammerHit -= OnHammerHit;
    }

    private void Start()
    {
        hammer = FindObjectOfType<Hammer>();
        if (virtualCamera != null)
        { virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>(); }
    }

    void OnHammerHit()
    {
        StartCoroutine(ShakeCamera());
    }

    IEnumerator ShakeCamera()
    {
        var hammerStrength = hammer.GetStrength();
        switch (hammerStrength)
        {
            case 1:
                virtualCameraNoise.m_AmplitudeGain = 1f;
                break;
            case 2:
                virtualCameraNoise.m_AmplitudeGain = 2f;
                break;
            case 3:
                virtualCameraNoise.m_AmplitudeGain = 4f;
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(0.3f);
        virtualCameraNoise.m_AmplitudeGain = 0f;
    }
}
