using UnityEngine;
using Cinemachine;

public class VirtualCameraShake : MonoBehaviour
{
    public float shakeAmplitude;
    public float shakeFrequency;

    private CinemachineVirtualCamera virtualCamera;
    private float shakeTimer = 0f;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (shakeTimer > 0f)
        {
            // 활성화된 노이즈의 Amplitude와 Frequency를 설정합니다.
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = shakeAmplitude;
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = shakeFrequency;

            shakeTimer -= Time.deltaTime;
        }
        else
        {
            // 노이즈를 비활성화합니다.
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0f;
        }
    }

    public void ShakeCamera(float shakeDuration, float a , float f)
    {
        shakeTimer = shakeDuration;
        shakeAmplitude = a;
        shakeFrequency = f;
    }
}
