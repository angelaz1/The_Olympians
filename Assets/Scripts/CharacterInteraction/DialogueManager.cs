using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager dm;

    // We will use the speakerName to find and parse the dialogue JSON
    // The dialogue for all characters should be in a JSON file named their name
    private string speakerName;
    private CharacterDialogue dialogue;
    private Dialogue[] currDialogue;
    private int currIndex;
    private DialogueUIManager dialogueUI;

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

    // Start is called before the first frame update
    void Start()
    {
        speakerName = "Aphrodite"; //TO BE REPLACED
        readDialogue();
        dialogueUI = GameObject.Find("DialogueUIManager").GetComponent<DialogueUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSpeakerName(string name) {
        speakerName = name;
    }

    public void startConversationDialogue() {
        // Pick a random conversation dialogue to do
        int convoLen = dialogue.conversationDialogue.Length;
        int index = Random.Range(0, convoLen);

        Dialogue[] selected = dialogue.conversationDialogue[index].dialogue;
        currDialogue = selected;
        currIndex = 0;

        dialogueUI.startDialogueUI();
        updateText();
        // dialogueUI.startDialogue(selected);
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
            updateText();
        }
    }

    public void selectOption(int i) {
        //REQUIRES: i must be a selectable option in current dialogue
        int nextIndex = currDialogue[currIndex].options[i].link;
        if(nextIndex == -1) {
            dialogueUI.finishDialogueUI();
            finishDialogue();
        } else {
            currIndex = nextIndex;
            updateText();
        }
    }



    public void finishDialogue() {
        GameObject.Find("InteractButtonManager").GetComponent<InteractButtonManager>().allFlyIn();
    }

    public void readDialogue() {
        string json = File.ReadAllText(Application.streamingAssetsPath + "/" + speakerName + ".json");
        dialogue = JsonUtility.FromJson<CharacterDialogue>(json);
        Debug.Log(dialogue.checkpointDialogue[0].dialogue[0].text);
    }
}
