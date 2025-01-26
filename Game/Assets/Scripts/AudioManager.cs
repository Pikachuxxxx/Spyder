using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public List<AudioClip> musicClips;
    public List<AudioClip> sfxClips;

    private Dictionary<string, AudioClip> musicDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeAudioDictionaries();
    }

    private void InitializeAudioDictionaries()
    {
        foreach (var clip in musicClips)
        {
            musicDict[clip.name] = clip;
        }

        foreach (var clip in sfxClips)
        {
            sfxDict[clip.name] = clip;
        }
    }

    public void PlayMusic(string clipName, bool loop = true)
    {
        if (musicDict.TryGetValue(clipName, out AudioClip clip))
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Music clip '{clipName}' not found!");
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(string clipName)
    {
        if (sfxDict.TryGetValue(clipName, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SFX clip '{clipName}' not found!");
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }
}

    //Notes
// Play Music or SFX:
//     Call AudioManager.Instance.PlayMusic("clipName") to play background music.
//     Call AudioManager.Instance.PlaySFX("clipName") to play a sound effect.
// Control Volume:
//     Use AudioManager.Instance.SetMusicVolume(float volume) and AudioManager.Instance.SetSFXVolume(float volume).