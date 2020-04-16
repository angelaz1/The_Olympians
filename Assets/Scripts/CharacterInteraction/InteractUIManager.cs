using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public enum CharacterExpression{Default};
public class InteractUIManager : MonoBehaviour
{
    private GameObject phone;
    private GameObject buttons;
    private GameObject topBar;
    private GameObject characterImage;
    private bool phoneDown;
    private Character currentCharacter;

    void Start()
    {
        phone = GameObject.Find("Phone");
        phoneDown = true;
        buttons = GameObject.Find("Buttons");
        topBar = GameObject.Find("TopBar");
        characterImage = GameObject.Find("CharacterImage");
    }

    public void setCharacter(Character character) {
        currentCharacter = character;
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
