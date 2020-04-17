﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public enum CharacterExpression{Default, Amused};
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
            updateHearts();
        }
    }

    public void unlockDate() {
        GameObject.Find("DateButton").GetComponent<Button>().interactable = true;
    }

    public void lockDate() {
        GameObject.Find("DateButton").GetComponent<Button>().interactable = false;
    }

    public void updateHearts() {
        int currCheckpoint = currentCharacter.getCurrentCheckpoint();
            int currProgress = currentCharacter.getCurrentAffectionProgress();
            for(int i = 0; i < currCheckpoint; i++) {
                //All hearts before currCheckpoint are completed
                hearts[i].GetComponent<Image>().sprite = heartStates[heartStates.Length - 1];
            }
            GameObject heart = hearts[currCheckpoint];
            if(currProgress < 20) {
                heart.GetComponent<Image>().sprite = heartStates[0];
            } else if(currProgress < 40) {
                heart.GetComponent<Image>().sprite = heartStates[1];
            } else if(currProgress < 60) {
                heart.GetComponent<Image>().sprite = heartStates[2];
            } else if(currProgress < 80) {
                heart.GetComponent<Image>().sprite = heartStates[3];
            } else if(currProgress < 100) {
                heart.GetComponent<Image>().sprite = heartStates[4];
            } else {
                heart.GetComponent<Image>().sprite = heartStates[5];
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
        if(portrait != null && portrait != characterImage.GetComponent<Image>().sprite) {
            characterImage.GetComponent<Image>().sprite = portrait;
            characterImage.GetComponent<Animator>().ResetTrigger("swappedState");
            characterImage.GetComponent<Animator>().SetTrigger("swappedState");
        }
    }

    public void playAffectionParticles() {
        characterImage.GetComponentInChildren<ParticleSystem>().Play();
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
