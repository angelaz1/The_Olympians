using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateButtonListener : ButtonListener
{
    public override void onHover() {
        if (this.GetComponent<Button>().interactable) {
            this.GetComponent<Animator>().SetBool("isHovering", true);
            this.GetComponent<AudioSource>().Play();
        }
    }

    public override void offHover() {
        if (this.GetComponent<Button>().interactable) {
            this.GetComponent<Animator>().SetBool("isHovering", false);
        }
    }

    public override void onClick() {
        GameObject.Find("InteractButtonManager").GetComponent<InteractButtonManager>().allFlyOff();
        GameObject.Find("GameManager").GetComponent<GameManager>().checkStartingDate();
    }
}
