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
    private enum DialogueType {Checkpoint, Conversation, Greeting, Post, AfterPost, Date};
    private CharacterDialogue dialogue;
    private Dialogue[] currDialogue;
    private int currIndex;
    private DialogueType currType;
    private DialogueUIManager dialogueUI;
    private GameManager gameManager;
    private int lastConvoIndex;

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

    public void setDialogue(CharacterDialogue dialogue) {
        this.dialogue = dialogue;
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
        while(convoLen > 1 && index == lastConvoIndex) {
            index = Random.Range(0, convoLen);
        }
        lastConvoIndex = index;

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
        currType = DialogueType.Checkpoint;
        startDialogue();
    }

    public void startPostDialogue() {
        // Get pre-post minigame dialogue
        Dialogue[] selected = dialogue.postDialogue[0].dialogue;
        currDialogue = selected;
        currType = DialogueType.Post;
        startDialogue();
    }

    public void startBadPostDialogue() {
        // Get bad after-post minigame dialogue
        Dialogue[] selected = dialogue.badPostDialogue[0].dialogue;
        currDialogue = selected;
        currType = DialogueType.AfterPost;
        startDialogue();
    }

    public void startOkPostDialogue() {
        // Get ok after-post minigame dialogue
        Dialogue[] selected = dialogue.okPostDialogue[0].dialogue;
        currDialogue = selected;
        currType = DialogueType.AfterPost;
        startDialogue();
    }

    public void startGoodPostDialogue() {
        // Get good after-post minigame dialogue
        Dialogue[] selected = dialogue.goodPostDialogue[0].dialogue;
        currDialogue = selected;
        currType = DialogueType.AfterPost;
        startDialogue();
    }

    public void startDateDialogue() {
        // Get date dialogue
        Dialogue[] selected = dialogue.dateDialogue[0].dialogue;
        currDialogue = selected;
        currType = DialogueType.Date;
        startDialogue();
    }

    public void updateText() {
        string targetText = currDialogue[currIndex].text;
        Option[] options = currDialogue[currIndex].options;
        
        if(currDialogue[currIndex].state == null) {
            dialogueUI.setState(CharacterExpression.Default);
        } else {
            CharacterExpression expr = (CharacterExpression)System.Enum.Parse(typeof(CharacterExpression), currDialogue[currIndex].state);
            dialogueUI.setState(expr); 
        }
          
        StartCoroutine(dialogueUI.updateTextUI(targetText, options));
    }

    public void advanceText() {
        int nextIndex = currDialogue[currIndex].options[0].link;
        if(nextIndex == -1) {
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
            finishDialogue();
        } else {
            currIndex = nextIndex;
            updateText();
        }
    }

    // Finish dialogue & load next UI
    public void finishDialogue() {
        dialogueUI.finishDialogueUI();
        switch(currType) {
            case DialogueType.Conversation: {
                GameObject.Find("InteractButtonManager").GetComponent<InteractButtonManager>().allFlyIn();
                break;
            }
            case DialogueType.Checkpoint: {
                GameObject.Find("InteractButtonManager").GetComponent<InteractButtonManager>().allFlyIn();
                GameObject.Find("InteractUIManager").GetComponent<InteractUIManager>().showTopBar();
                break;
            }
            case DialogueType.Greeting: {
                GameObject.Find("InteractButtonManager").GetComponent<InteractButtonManager>().allFlyIn();
                GameObject.Find("InteractUIManager").GetComponent<InteractUIManager>().showTopBar();
                break;
            }
            case DialogueType.Post: {
                // Load post minigame scene
                SceneManager.LoadScene("PostDemo");
                break;
            }
            case DialogueType.AfterPost: {
                GameObject.Find("InteractButtonManager").GetComponent<InteractButtonManager>().allFlyIn();
                GameObject.Find("InteractUIManager").GetComponent<InteractUIManager>().showTopBar();
                GameObject.Find("GameManager").GetComponent<GameManager>().showFeedback();
                break;
            }
            case DialogueType.Date: {
                // Load post minigame scene
                SceneManager.LoadScene("Match3Game");
                break;
            }
        }        
    }
}
