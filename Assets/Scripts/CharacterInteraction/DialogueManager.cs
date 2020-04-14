using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager dm;

    // We will use the speakerName to find and parse the dialogue JSON
    // The dialogue for all characters should be in a JSON file named their name
    private enum DialogueType {Checkpoint, Conversation, Greeting, Post};
    private string speakerName;
    private CharacterDialogue dialogue;
    private Dialogue[] currDialogue;
    private int currIndex;
    private DialogueType currType;
    private DialogueUIManager dialogueUI;
    private GameManager gameManager;

    private void Awake() {
        if (dm == null)
        {
            dm = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        dialogueUI = GameObject.Find("DialogueUIManager").GetComponent<DialogueUIManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void setSpeakerName(string name) {
        speakerName = name;
    }

    void startDialogue() {
        currIndex = 0;
        if (dialogueUI == null) {
            dialogueUI = GameObject.Find("DialogueUIManager").GetComponent<DialogueUIManager>();
        }

        dialogueUI.startDialogueUI();
        updateText();
    }

    public void startConversationDialogue() {
        // Pick a random conversation dialogue to do
        int convoLen = dialogue.conversationDialogue.Length;
        int index = Random.Range(0, convoLen);

        Dialogue[] selected = dialogue.conversationDialogue[index].dialogue;
        currDialogue = selected;
        currType = DialogueType.Conversation;
        startDialogue();
    }

    public void startGreetingDialogue() {
        // Get greeting dialogue
        Dialogue[] selected = dialogue.greetDialogue[0].dialogue;
        currDialogue = selected;
        currType = DialogueType.Greeting;
        startDialogue();
    }

    public void startCheckpointDialogue(int i) {
        // Get the ith checkpoint dialogue
        Dialogue[] selected = dialogue.checkpointDialogue[i].dialogue;
        currDialogue = selected;
        currType = DialogueType.Conversation;
        startDialogue();
    }

    public void startPostDialogue() {
        // Get pre-post minigame dialogue
        Dialogue[] selected = dialogue.postDialogue[0].dialogue;
        currDialogue = selected;
        currType = DialogueType.Post;
        startDialogue();
    }

    public void updateText() {
        string targetText = currDialogue[currIndex].text;
        Option[] options = currDialogue[currIndex].options;
        StartCoroutine(dialogueUI.updateTextUI(targetText, options));
    }

    public void advanceText() {
        int nextIndex = currDialogue[currIndex].options[0].link;
        if(nextIndex == -1) {
            dialogueUI.finishDialogueUI();
            finishDialogue();
        } else {
            currIndex = nextIndex;
            updateText();
        }
    }

    public void selectOption(int i) {
        //REQUIRES: i must be a selectable option in current dialogue
        int affectionBonus = currDialogue[currIndex].options[i].affectionBonus;
        gameManager.addAffection(affectionBonus);

        int nextIndex = currDialogue[currIndex].options[i].link;
        if(nextIndex == -1) {
            dialogueUI.finishDialogueUI();
            finishDialogue();
        } else {
            currIndex = nextIndex;
            updateText();
        }
    }

    // Finish dialogue & load next UI
    public void finishDialogue() {
        switch(currType) {
            case DialogueType.Conversation: {
                GameObject.Find("InteractButtonManager").GetComponent<InteractButtonManager>().allFlyIn();
                break;
            }
            case DialogueType.Checkpoint: {
                GameObject.Find("InteractButtonManager").GetComponent<InteractButtonManager>().allFlyIn();
                break;
            }
            case DialogueType.Greeting: {
                GameObject.Find("InteractButtonManager").GetComponent<InteractButtonManager>().allFlyIn();
                break;
            }
            case DialogueType.Post: {
                // Load post minigame scene
                SceneManager.LoadScene("PostDemo");
                break;
            }
        }        
    }

    public void readDialogue() {
        TextAsset jsonTextFile = Resources.Load<TextAsset>(speakerName);
        dialogue = JsonUtility.FromJson<CharacterDialogue>(jsonTextFile.ToString());
    }
}
