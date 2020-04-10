using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateButtonListener : ButtonListener
{
    public override void onClick() {
        GameObject.Find("InteractButtonManager").GetComponent<InteractButtonManager>().allFlyOff();
        // START DATE
    }
}
