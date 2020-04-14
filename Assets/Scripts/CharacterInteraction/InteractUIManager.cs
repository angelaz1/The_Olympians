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

    void Start()
    {
        phone = GameObject.Find("Phone");
        phoneDown = true;
        buttons = GameObject.Find("Buttons");
    }

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

    IEnumerator loadMapScene() {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MapDemo");    
    }

    public void loadMap() {
        phone.GetComponent<Animator>().SetTrigger("OpenApp");
        Debug.Log("Map pressed");
        StartCoroutine(loadMapScene());
    }

    public void exitGame() {
        Application.Quit();
    }
}
