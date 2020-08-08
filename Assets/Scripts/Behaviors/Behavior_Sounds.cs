using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior_Sounds : MonoBehaviour
{
    [SerializeField] private AudioSource source_track = null;
    [SerializeField] private AudioSource source_sfx = null;

    [SerializeField] private AudioClip track_main = null;
    [SerializeField] private AudioClip sfx_wind = null;
    [SerializeField] private AudioClip sfx_release = null;
    [SerializeField] private AudioClip sfx_jump = null;

    public static Behavior_Sounds Instance = null;

    public float GetTrackVolume() { return source_track.volume; }
    public float GetSFXVolume() { return source_track.volume; }


    public void SetTrackVolume(float v)
    {
        source_track.volume = v;
    }

    public void SetSFXVolume(float v)
    {
        source_sfx.volume = v;
    }

    public void PlayWind()
    {
        source_sfx.clip = sfx_wind;
        if (!source_sfx.isPlaying)
        {
            source_sfx.Play();
        }
    }

    public void PlayRelease()
    {
        source_sfx.clip = sfx_release;
        if (!source_sfx.isPlaying)
        {
            source_sfx.Play();
        }
    }

    public void PlayJump()
    {
        source_sfx.PlayOneShot(sfx_jump);
    }

    public void StopSFX()
    {
        source_sfx.Stop();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        source_track.loop = true;
        source_track.clip = track_main;
        source_track.Play();

        source_sfx.loop = true;
    }
}
