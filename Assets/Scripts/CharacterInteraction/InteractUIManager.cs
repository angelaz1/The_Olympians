using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public enum CharacterExpression{Default};
public class InteractUIManager : MonoBehaviour
{
    public Sprite[] heartStates;

    private GameObject phone;
    private GameObject buttons;
    private GameObject characterImage;

    private GameObject topBar;
    private GameObject topBarName;
    private GameObject[] hearts;
    private GameObject followerCount;

    private Character currentCharacter;
    private bool phoneDown;
    

    void Start()
    {
        phone = GameObject.Find("Phone");
        phoneDown = true;
        buttons = GameObject.Find("Buttons");
        setTopBar();
        characterImage = GameObject.Find("CharacterImage");
    }

    void setTopBar() {
        topBar = GameObject.Find("TopBar");
        hearts = new GameObject[5];
        foreach (Transform child in topBar.transform){
            if (child.name == "Name"){
                topBarName = child.gameObject;
            }
            if (child.name == "Heart0"){
                hearts[0] = child.gameObject;
            }
            if (child.name == "Heart1"){
                hearts[1] = child.gameObject;
            }
            if (child.name == "Heart2"){
                hearts[2] = child.gameObject;
            }
            if (child.name == "Heart3"){
                hearts[3] = child.gameObject;
            }
            if (child.name == "Heart4"){
                hearts[4] = child.gameObject;
            }
            if (child.name == "FollowerCount"){
                followerCount = child.gameObject;
            }
        }
    }

    public void setCharacter(Character character) {
        currentCharacter = character;
        if(topBar == null) {
            setTopBar();
            topBarName.GetComponent<TextMeshProUGUI>().text = character.getName();
            //UPDATE HEARTS AND FOLLOWER COUNT

        }
    }

    public void showTopBar() {
        if (topBar == null) setTopBar();
        topBar.GetComponent<Animator>().ResetTrigger("ShowBar");
        topBar.GetComponent<Animator>().SetTrigger("ShowBar");
    }

    public void setCharacterPortrait(CharacterExpression expr) {
        Sprite portrait = currentCharacter.getCharacterPortrait(expr.ToString());
        if(characterImage == null) {
            characterImage = GameObject.Find("CharacterImage");
        }
        if(portrait != null) {
            characterImage.GetComponent<Image>().sprite = portrait;
        }
    }

    public void playAffectionParticles() {
        characterImage.GetComponent<ParticleSystem>().Play();
    }

    public void arrowPressed() {
        if (phoneDown) {
            phone.GetComponent<Animator>().SetTrigger("PullUpPhone");
            phone.GetComponent<Animator>().ResetTrigger("PutAwayPhone");
            phoneDown = false;
        } else {
            phone.GetComponent<Animator>().SetTrigger("PutAwayPhone");
            phone.GetComponent<Animator>().ResetTrigger("PullUpPhone");
            phoneDown = true;
        }
    }

    IEnumerator loadMapScene() {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene("MapDemo");    
    }

    public void loadMap() {
        phone.GetComponent<Animator>().SetTrigger("OpenApp");
        StartCoroutine(loadMapScene());
    }

    public void exitGame() {
        Application.Quit();
    }
}
