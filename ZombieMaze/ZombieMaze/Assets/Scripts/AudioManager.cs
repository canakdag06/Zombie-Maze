using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource ambientMusic;
    public AudioSource sfx;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void PlaySFX(AudioClip clip)
    {
        sfx.PlayOneShot(clip);
    }
}
