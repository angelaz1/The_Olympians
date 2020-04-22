using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip goodSound;
    public AudioClip badSound;
    public AudioClip clickSound;

    private AudioSource source;

    void Start()
    {
        source = this.GetComponent<AudioSource>();
    }

    public IEnumerator playSound() {
        source.Play();
        yield return new WaitForSeconds(source.clip.length);
    }

    public void playGoodSound() {
        source.clip = goodSound;
        StartCoroutine(playSound());
    }

    public void playBadSound() {
        source.clip = badSound;
        StartCoroutine(playSound());
    }

    public void playClickSound() {
        source.clip = clickSound;
        StartCoroutine(playSound());
    }
}
