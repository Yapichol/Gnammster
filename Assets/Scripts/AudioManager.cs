using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioClip[] playlist;
    public AudioSource audioSource;
    private int indexMusic;


    // Start is called before the first frame update
    void Start()
    {
        indexMusic = 0;
        audioSource.clip = playlist[indexMusic];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextSong();
        }
    }

    void PlayNextSong()
    {
        indexMusic = (indexMusic + 1) % playlist.Length;
        audioSource.clip = playlist[indexMusic];
        audioSource.Play();
    }
}
