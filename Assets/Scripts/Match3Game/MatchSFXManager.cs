using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSFXManager : MonoBehaviour
{
    public AudioClip matchComboGreat;
    public AudioClip matchComboGood;
    public AudioClip matchComboOk;
    public AudioClip popSound;
    public AudioClip phoneUpSound;
    public AudioClip phoneDownSound;

    private AudioSource source;

    private bool isPlaying;

    void Start()
    {
        source = this.GetComponent<AudioSource>();
        isPlaying = false;
    }

    public IEnumerator playSound(AudioClip clip) {
        if(!isPlaying) {
            isPlaying = true;
            if (source == null) source = this.GetComponent<AudioSource>();
            source.clip = clip;
            source.Play();
            yield return new WaitForSeconds(source.clip.length);
            isPlaying = false;
        }
    }

    public void playGreatCombo() {
        StartCoroutine(playSound(matchComboGreat));
    }

    public void playGoodCombo() {
        StartCoroutine(playSound(matchComboGood));
    }

    public void playOkCombo() {
        StartCoroutine(playSound(matchComboOk));
    }

    public void playPopSound() {
        StartCoroutine(playSound(popSound));
    } 

    public void playPhoneUp() {
        StartCoroutine(playSound(phoneUpSound));
    }

    public void playPhoneDown() {
        StartCoroutine(playSound(phoneDownSound));
    }
}
