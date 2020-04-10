using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractButtonManager : MonoBehaviour
{
    private ButtonListener[] listeners; 

    // Start is called before the first frame update
    void Start()
    {
        listeners = new ButtonListener[3];
        listeners[0] = GameObject.Find("ConverseButton").GetComponent<ButtonListener>();
        listeners[1] = GameObject.Find("PostButton").GetComponent<ButtonListener>();
        listeners[2] = GameObject.Find("DateButton").GetComponent<ButtonListener>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void allFlyOff() {
        for(int i = 0; i < listeners.Length; i++) {
            listeners[i].setFlyingOff();
        }
    }

    public void allFlyIn() {
        for(int i = 0; i < listeners.Length; i++) {
            listeners[i].setFlyingOn();
        }
    }
}
