using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;


public class CameraShakeHandler : MonoBehaviour
{
    public static CameraShakeHandler Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private CinemachineVirtualCamera CMVCam;

    [Header("States")]
    [SerializeField] private State state;

    public static event EventHandler<OnCameraShakeEventArgs> OnCameraShakeStart;

    public enum State {NotShaking, Shaking }

    public State CameraState => state;

    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    public class OnCameraShakeEventArgs : EventArgs
    {
        public CameraShake cameraShake;
    }

    private void Awake()
    {
        cinemachineBasicMultiChannelPerlin = CMVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        SetSingleton();
    }

    private void Start()
    {
        SetCameraState(State.NotShaking);
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one CameraShakeHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void SetCameraState(State state) => this.state = state;

    public void ShakeCamera(CameraShake cameraShake)
    {
        if (state != State.NotShaking) return;

        StopAllCoroutines();
        StartCoroutine(ShakeCameraCoroutine(cameraShake.amplitude, cameraShake.frequency, cameraShake.shakeTime, cameraShake.fadeInTime, cameraShake.fadeOutTime));
        OnCameraShakeStart?.Invoke(this, new OnCameraShakeEventArgs { cameraShake = cameraShake });
    }

    private IEnumerator ShakeCameraCoroutine(float amplitude, float frequency, float shakeTime, float fadeInTime, float fadeOutTime)
    {
        SetCameraState(State.Shaking);

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0f;

        float time = 0f;

        while (time <= fadeInTime)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(0f, amplitude, time / fadeInTime);
            cinemachineBasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(0f, frequency, time / fadeInTime);

            time += Time.deltaTime;
            yield return null;
        }

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitude;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequency;

        yield return new WaitForSeconds(shakeTime);

        time = 0f;

        while (time <= fadeOutTime)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(amplitude, 0f, time / fadeOutTime);
            cinemachineBasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(frequency, 0f, time / fadeOutTime);

            time += Time.deltaTime;
            yield return null;
        }

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0f;

        SetCameraState(State.NotShaking);
    }
}

[Serializable]
public class CameraShake
{
    public int id;
    public string logToStart;
    [Range(0, 2f)] public float amplitude;
    [Range(0, 5f)] public float frequency;
    [Range(0.5f, 10f)] public float shakeTime;
    [Range(0f, 10f)] public float fadeInTime;
    [Range(0f, 10f)] public float fadeOutTime;
}
