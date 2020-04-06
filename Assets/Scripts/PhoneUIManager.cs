using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneUIManager : MonoBehaviour
{
    private GameObject phone;
    private bool phoneDown;
    // Start is called before the first frame update
    void Start()
    {
        phone = GameObject.Find("Phone");
        phoneDown = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void arrowPressed() {
        Debug.Log("Pressed!");
        if (phoneDown) {
            phone.GetComponent<Animator>().SetTrigger("PullUpPhone");
            phone.GetComponent<Animator>().ResetTrigger("PutAwayPhone");
            Debug.Log("PullUp!");
            phoneDown = false;
        } else {
            phone.GetComponent<Animator>().SetTrigger("PutAwayPhone");
            phone.GetComponent<Animator>().ResetTrigger("PullUpPhone");
            Debug.Log("PutAway!");
            phoneDown = true;
        }
    }
}
