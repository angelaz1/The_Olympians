using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    private GameObject canvas;
    private int clickedTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clickedNotif() {
        canvas.GetComponent<Animator>().SetTrigger("ClickedNotif");
    }

    IEnumerator SwapScenes() {
        canvas.GetComponent<Animator>().SetTrigger("SwapScenes");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("PhoneUIDemo");    
    }
    
    public void advanceText() {
        if(clickedTime == 0) {
            canvas.GetComponent<Animator>().SetTrigger("MoveText1");
        }
        if(clickedTime == 1) {
            canvas.GetComponent<Animator>().SetTrigger("MoveText2");
        }
        if(clickedTime == 2) {
            canvas.GetComponent<Animator>().SetTrigger("MoveText3");
        }
        if(clickedTime == 3) {
            StartCoroutine(SwapScenes());
        }
        clickedTime++;
    }
}
