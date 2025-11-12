using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("SFX")]
    public AudioClip Background;
    public AudioClip ElectroBullet;
    public AudioClip FireBullet;
    public AudioClip Freeze;

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void Start()
    {
        musicSource.clip = Background;
        musicSource.Play();
    }
}
