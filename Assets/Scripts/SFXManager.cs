using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip goodSound;
    public AudioClip badSound;
    public AudioClip clickSound;
    public AudioClip phoneUpSound;
    public AudioClip phoneDownSound;
    public AudioClip popSound;

    private AudioSource source;

    void setSource() {
        source = this.GetComponent<AudioSource>();
    }

    public IEnumerator playSound() {
        source.Play();
        yield return new WaitForSeconds(source.clip.length);
    }

    public void playGoodSound() {
        setSource();
        source.clip = goodSound;
        source.pitch = 1;
        StartCoroutine(playSound());
    }

    public void playBadSound() {
        setSource();
        source.clip = badSound;
        source.pitch = 1;
        StartCoroutine(playSound());
    }

    public void playClickSound() {
        setSource();
        source.clip = popSound;//clickSound;
        source.pitch = Random.Range(0.9f, 1.2f);
        StartCoroutine(playSound());
    }

    public void playPhoneUpSound() {
        setSource();
        source.clip = phoneUpSound;
        source.pitch = 1;
        StartCoroutine(playSound());
    }

    public void playPhoneDownSound() {
        setSource();
        source.clip = phoneDownSound;
        source.pitch = 1;
        StartCoroutine(playSound());
    }

    public void playPopSound() {
        setSource();
        source.clip = popSound;
        source.pitch = 1;
        StartCoroutine(playSound());
    }
}
