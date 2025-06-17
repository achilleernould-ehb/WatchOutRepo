using UnityEngine;

public class PhoneRinger : MonoBehaviour
{
    public AudioSource phoneAudioSource;
    public float delay = 5f;

    void Start()
    {
        Invoke("PlayRingtone", delay);
    }

    void PlayRingtone()
    {
        if (phoneAudioSource != null)
        {
            phoneAudioSource.Play();
        }
    }
}