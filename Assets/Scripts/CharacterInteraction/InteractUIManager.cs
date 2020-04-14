using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class InteractUIManager : MonoBehaviour
{
    private GameObject phone;
    private GameObject buttons;
    private bool phoneDown;
    // Start is called before the first frame update
    void Start()
    {
        phone = GameObject.Find("Phone");
        phoneDown = true;
        buttons = GameObject.Find("Buttons");
    }

    // Update is called once per frame
    void Update()
    {
        // GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
        // if(currentSelected != null && currentSelected.transform.parent == buttons) {
        //     currentSelected.GetComponent<Animator>().SetBool("isHovering", true);
        // }
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

    public void loadMap() {
        SceneManager.LoadScene("MapDemo");
    }

    public void exitGame() {
        Application.Quit();
    }
}
