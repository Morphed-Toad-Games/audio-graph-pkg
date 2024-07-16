using Josephus.AudioGraph;
using UnityEngine;

public class AudioSourceVolumeCone : MonoBehaviour
{
    [SerializeField, HideInInspector] float maxAngle = 360;
    [SerializeField, HideInInspector] float minAngle = 360;

    [SerializeField, HideInInspector] AudioSource audioSource;

    Transform trans;

    private void Start()
    {
        trans = transform;
    }

    public void SetAngles(float minAngle, float maxAngle)
    {
        this.minAngle = minAngle;
        this.maxAngle = maxAngle;
    }

    public void SetAudioSource(AudioSource audioSource)
    {
        this.audioSource = audioSource;
    }

    private void Update()
    {
        audioSource.volume = CalculateConeVolume(trans.position, AudioGraphSource.GetActiveAudioListener().transform.position);
    }

    public float CalculateConeVolume(Vector3 sourcePosition, Vector3 listenerPosition)
    {
        var targetDirection = listenerPosition - sourcePosition;
        var angle = Vector3.Angle(targetDirection, trans.forward);
        if (angle < minAngle)
            return 1;

        if (angle > maxAngle)
            return 0;

        return Mathf.InverseLerp(maxAngle, minAngle, angle);
    }
}
