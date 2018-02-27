using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager> {

    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip bossMusic;

    public AudioClip BakgroundMusic { get { return backgroundMusic; } }
    public AudioClip BossMusic { get { return BossMusic; } }

    private AudioSource source;

    public void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private IEnumerator SoundFadeOut()
    {
        if (source != null)
        {
            float startVolume = source.volume;

            while (source.volume > 0)
            {
                source.volume -= startVolume * Time.deltaTime / 2;
                yield return null;
            }

            source.Stop();
            source.volume = startVolume;
        }
    }

    private IEnumerator PlayMusic(AudioClip clip)
    {
        StartCoroutine(SoundFadeOut());
        yield return new WaitForSeconds(2);
        source.clip = clip;
        source.Play();
    }

    public void StopMusic()
    {
        StartCoroutine(SoundFadeOut());
    }

    public void PlayBackground()
    {
        StartCoroutine(PlayMusic(backgroundMusic));
    }

    public void PlayBoss()
    {
        StartCoroutine(PlayMusic(bossMusic));
    }
}
