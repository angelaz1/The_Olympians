using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUIManager : MonoBehaviour
{
    // private Dialogue[] currDialogue;
    private GameObject dialogueUI;
    private GameObject textBox;
    private GameObject nextButton;
    private GameObject[] optionButtons;
    private int currIndex;

    // Start is called before the first frame update
    void Awake()
    {
        initVariables();
    }

    void initVariables() {
        dialogueUI = GameObject.Find("DialogueUI");
        optionButtons = new GameObject[3];
        foreach (Transform child in dialogueUI.transform){
            if (child.name == "TextBox"){
                textBox = child.gameObject;
            }
            if (child.name == "NextButton"){
                nextButton = child.gameObject;
            }
            if (child.name == "OptionButton0"){
                optionButtons[0] = child.gameObject;
            }
            if (child.name == "OptionButton1"){
                optionButtons[1] = child.gameObject;
            }
            if (child.name == "OptionButton2"){
                optionButtons[2] = child.gameObject;
            }
        }
        dialogueUI.GetComponent<Canvas>().enabled = false;
    }

    public IEnumerator updateTextUI(string targetText, Option[] options) {
        disableAllButtons();
        string currText = "";
        for(int i = 0; i < targetText.Length; i++) {
            currText += targetText[i];
            textBox.GetComponentInChildren<TextMeshProUGUI>().text = currText;
            yield return new WaitForSeconds(0.02f);
            if (Input.GetMouseButtonDown(0)) {
                textBox.GetComponentInChildren<TextMeshProUGUI>().text = targetText;
                break;
            }
        }
        
        setOptions(options);
    }

    public void setOptions(Option[] options) {

        if(options.Length == 1) {
            nextButton.SetActive(true);
        } else {
            nextButton.SetActive(false);
            for(int i = 0; i < options.Length; i++) {
                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = options[i].optionText;
                optionButtons[i].GetComponent<Animator>().SetTrigger("flyIn");
                optionButtons[i].GetComponent<Animator>().ResetTrigger("flyOut");
            }
        }
    }

    public void disableAllButtons() {
        for(int i = 0; i < optionButtons.Length; i++) {
            optionButtons[i].GetComponent<Animator>().SetTrigger("flyOut");
            optionButtons[i].GetComponent<Animator>().ResetTrigger("flyIn");
        }
        nextButton.SetActive(false);
    }

    public void startDialogueUI() {
        // Manage the UI
        if (dialogueUI == null) {
            initVariables();
        }
        dialogueUI.GetComponent<Canvas>().enabled = true;
    }

    public void finishDialogueUI() {
        dialogueUI.GetComponent<Canvas>().enabled = false;
    }
}
