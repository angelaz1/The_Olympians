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
    private enum DialogueType {Checkpoint, Conversation, Greeting, Post, AfterPost, Date, FailedDate, FinalCheckpoint, Insufficient, Hated};
    private CharacterDialogue dialogue;
    private Dialogue[] currDialogue;
    private int currIndex;
    private DialogueType currType;
    private DialogueUIManager dialogueUI;
    private GameManager gameManager;
    private SFXManager sfxManager;
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
        sfxManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
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

    public void startFinalCheckpointDialogue() {
        // Get the final checkpoint dialogue
        Dialogue[] selected = dialogue.checkpointDialogue[5].dialogue;
        currDialogue = selected;
        currType = DialogueType.FinalCheckpoint;
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
        int diaLen = dialogue.badPostDialogue.Length;
        int index = Random.Range(0, diaLen);
        Dialogue[] selected = dialogue.badPostDialogue[index].dialogue;
        currDialogue = selected;
        currType = DialogueType.AfterPost;
        startDialogue();
    }

    public void startOkPostDialogue() {
        // Get ok after-post minigame dialogue
        int diaLen = dialogue.okPostDialogue.Length;
        int index = Random.Range(0, diaLen);
        Dialogue[] selected = dialogue.okPostDialogue[index].dialogue;
        currDialogue = selected;
        currType = DialogueType.AfterPost;
        startDialogue();
    }

    public void startGoodPostDialogue() {
        // Get good after-post minigame dialogue
        int diaLen = dialogue.goodPostDialogue.Length;
        int index = Random.Range(0, diaLen);
        Dialogue[] selected = dialogue.goodPostDialogue[index].dialogue;
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

    public void startFailDateDialogue() {
        // Get failed date dialogue
        int diaLen = dialogue.goodPostDialogue.Length;
        int index = Random.Range(0, diaLen);
        Dialogue[] selected = dialogue.failDateDialogue[index].dialogue;
        currDialogue = selected;
        currType = DialogueType.FailedDate;
        startDialogue();
    }

    public void startInsufficientAffectionDialogue() {
        // Get insufficient affection dialogue
        int diaLen = dialogue.insufficientAffectionDialogue.Length;
        int index = Random.Range(0, diaLen);
        Dialogue[] selected = dialogue.insufficientAffectionDialogue[index].dialogue;
        currDialogue = selected;
        currType = DialogueType.Insufficient;
        startDialogue();
    }

    public void startInsufficientFollowersDialogue() {
        // Get insufficient followers dialogue
        int diaLen = dialogue.insufficientFollowersDialogue.Length;
        int index = Random.Range(0, diaLen);
        Dialogue[] selected = dialogue.insufficientFollowersDialogue[index].dialogue;
        currDialogue = selected;
        currType = DialogueType.Insufficient;
        startDialogue();
    }

    public void startHatedDialogue() {
        // Get insufficient followers dialogue
        int diaLen = dialogue.hatedDialogue.Length;
        int index = Random.Range(0, diaLen);
        Dialogue[] selected = dialogue.hatedDialogue[index].dialogue;
        currDialogue = selected;
        currType = DialogueType.Hated;
        startDialogue();
    }

    void reshuffle(Option[] texts)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < texts.Length; t++ )
        {
            Option tmp = texts[t];
            int r = Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[r] = tmp;
        }
    }

    public void updateText() {
        string targetText = currDialogue[currIndex].text;
        reshuffle(currDialogue[currIndex].options);
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
        GameObject.Find("SFXManager").GetComponent<SFXManager>().playClickSound();
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
        GameObject.Find("SFXManager").GetComponent<SFXManager>().playClickSound();
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
                GameObject.Find("GameManager").GetComponent<GameManager>().checkIfHated();
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
                GameObject.Find("InteractUIManager").GetComponent<InteractUIManager>().loadPost();
                break;
            }
            case DialogueType.AfterPost: {
                GameObject.Find("GameManager").GetComponent<GameManager>().showFeedback();
                break;
            }
            case DialogueType.FailedDate: {
                GameObject.Find("InteractButtonManager").GetComponent<InteractButtonManager>().allFlyIn();
                GameObject.Find("InteractUIManager").GetComponent<InteractUIManager>().showTopBar();
                GameObject.Find("GameManager").GetComponent<GameManager>().addAffection(-1000);
                break;
            }
            case DialogueType.Date: {
                // Load date minigame scene
                GameObject.Find("InteractUIManager").GetComponent<InteractUIManager>().loadMatch3Game();
                break;
            }
            case DialogueType.FinalCheckpoint: {
                GameObject.Find("InteractUIManager").GetComponent<InteractUIManager>().loadWinScreen();
                // GameObject.Find("InteractUIManager").GetComponent<InteractUIManager>().showTopBar();
                break;
            }
            case DialogueType.Insufficient: {
                GameObject.Find("InteractButtonManager").GetComponent<InteractButtonManager>().allFlyIn();
                GameObject.Find("InteractUIManager").GetComponent<InteractUIManager>().showTopBar();
                break;
            }
            case DialogueType.Hated: {
                break;
            }
        }        
    }
}
