using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    public AudioSource currentBMusic;

    public GameObject sourcePrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Play(AudioClip clip, Transform parent)
    {
        GameObject source = Instantiate(sourcePrefab, parent);
        source.transform.name = clip.name;
        AudioSource AudioP = source.GetComponent<AudioSource>();

        AudioP.clip = clip;
        AudioP.Play();

        Destroy(source, clip.length);
    }

    public void PlayMusic(AudioClip clip, float volume, Transform parent)
    {
        if (currentBMusic != null)
        {
            Destroy(currentBMusic.gameObject);
        }

        GameObject source = Instantiate(sourcePrefab, parent);
        source.transform.name = clip.name;
        AudioSource AudioP = source.GetComponent<AudioSource>();

        AudioP.clip = clip;
        AudioP.loop = true;
        AudioP.volume = volume;
        AudioP.Play();

        currentBMusic = AudioP;
    }
}
