using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class ButtonListener : MonoBehaviour
{   
    public void onHover() {
        this.GetComponent<Animator>().SetBool("isHovering", true);
        this.GetComponent<AudioSource>().Play();
    }

    public void offHover() {
        this.GetComponent<Animator>().SetBool("isHovering", false);
    }
    
    public void setFlyingOff() {
        this.GetComponent<Animator>().SetTrigger("flyOff");
        this.GetComponent<Animator>().ResetTrigger("flyIn");
    }

    public void setFlyingOn() {
        this.GetComponent<Animator>().ResetTrigger("flyOff");
        this.GetComponent<Animator>().SetTrigger("flyIn");
    }

    public abstract void onClick();
}
