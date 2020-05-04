using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationButtonListener : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject screen;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        screen = GameObject.Find("BlackScreen");
    }

    IEnumerator SwapScenes() {
        screen.GetComponent<Animator>().SetTrigger("SwapScene");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("PhoneUIDemo");    
    }

    public void moveToLocation(string location) {
        string characterName = "Aphrodite";
        switch(location) {
            case "Mall": {
                characterName = "Aphrodite";
                break;
            }
            case "Gym": {
                characterName = "Ares";
                break;
            }
            default: break;
        }

        gameManager.setCharacter(characterName);
        StartCoroutine(SwapScenes());
    }
}
