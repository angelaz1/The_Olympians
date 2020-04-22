using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip goodSound;
    public AudioClip badSound;

    private AudioSource source;

    void Start()
    {
        source = this.GetComponent<AudioSource>();
    }

    public void playGoodSound() {
        source.clip = goodSound;
        source.Play();
    }

    public void playBadSound() {
        source.clip = badSound;
        source.Play();
    }
}
