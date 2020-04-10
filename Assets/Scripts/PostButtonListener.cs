using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PostButtonListener : ButtonListener
{
    public override void onClick() {
        GameObject.Find("InteractButtonManager").GetComponent<InteractButtonManager>().allFlyOff();
        // START POST

        // Load post minigame scene
        SceneManager.LoadScene("PostDemo");
    }
}
