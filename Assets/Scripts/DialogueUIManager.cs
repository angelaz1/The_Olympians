using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUIManager : MonoBehaviour
{
    private Dialogue[] currDialogue;
    private GameObject dialogueUI;
    private GameObject textBox;
    private GameObject nextButton;
    private GameObject[] optionButtons;
    private int currIndex;

    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setOptions() {
        Option[] options = currDialogue[currIndex].options;

        for(int i = 0; i < optionButtons.Length; i++) {
            optionButtons[i].SetActive(false);
        }

        if(options.Length == 1) {
            nextButton.SetActive(true);
        } else {
            nextButton.SetActive(false);
            for(int i = 0; i < options.Length; i++) {
                optionButtons[i].SetActive(true);
                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = options[i].optionText;
            }
        }
    }

    public void updateText() {
        textBox.GetComponentInChildren<TextMeshProUGUI>().text = currDialogue[currIndex].text;
        setOptions();
    }

    public void startDialogue(Dialogue[] dialogue) {
        currDialogue = dialogue;
        currIndex = 0;

        // Manage the UI
        dialogueUI.GetComponent<Canvas>().enabled = true;
        updateText();
    }

    public void advanceText() {
        int nextIndex = currDialogue[currIndex].options[0].link;
        if(nextIndex == -1) {
            dialogueUI.GetComponent<Canvas>().enabled = false;
            GameObject.Find("DialogueManager").GetComponent<DialogueManager>().finishDialogue();
        } else {
            currIndex = nextIndex;
            updateText();
        }
    }

    public void selectOption(int i) {
        //REQUIRES: i must be a selectable option in current dialogue
        int nextIndex = currDialogue[currIndex].options[i].link;
        if(nextIndex == -1) {
            dialogueUI.GetComponent<Canvas>().enabled = false;
            GameObject.Find("DialogueManager").GetComponent<DialogueManager>().finishDialogue();
        } else {
            currIndex = nextIndex;
            updateText();
        }
    }
}
