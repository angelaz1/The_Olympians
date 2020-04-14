using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager dm;

    // We will use the speakerName to find and parse the dialogue JSON
    // The dialogue for all characters should be in a JSON file named their name
    private enum DialogueType {Checkpoint, Conversation, Greeting};
    private string speakerName;
    private CharacterDialogue dialogue;
    private Dialogue[] currDialogue;
    private int currIndex;
    private DialogueType currType;
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
        dialogueUI = GameObject.Find("DialogueUIManager").GetComponent<DialogueUIManager>();
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
        currType = DialogueType.Conversation;
        
        if (dialogueUI == null) {
            dialogueUI = GameObject.Find("DialogueUIManager").GetComponent<DialogueUIManager>();
        }

        dialogueUI.startDialogueUI();
        updateText();
    }

    public void startCheckpointDialogue(int i) {
        // Get the ith checkpoint dialogue
        Dialogue[] selected = dialogue.checkpointDialogue[i].dialogue;

        currDialogue = selected;
        currIndex = 0;
        currType = DialogueType.Conversation;
        
        if (dialogueUI == null) {
            dialogueUI = GameObject.Find("DialogueUIManager").GetComponent<DialogueUIManager>();
        }

        dialogueUI.startDialogueUI();
        updateText();
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
        }        
    }

    // IEnumerator loadStreamingAsset(string fileName)
    // {
    //     string filePath = System.IO.Path.Combine(Application.streamingAssetsPath + "/", fileName);
    //     // Debug.Log(filePath);
    //     string result;
    //     if (filePath.Contains("://") || filePath.Contains(":///"))
    //     {
    //         UnityWebRequest www = new UnityWebRequest(filePath);
    //         yield return www.SendWebRequest();
    //         result = www.downloadHandler.text;
    //     }
    //     else
    //         result = System.IO.File.ReadAllText(filePath);

    //     Debug.Log(filePath);
    //     dialogue = JsonUtility.FromJson<CharacterDialogue>(result);
    //     // Debug.Log(dialogue.checkpointDialogue[0].dialogue[0].text);
    // }
    public void readDialogue() {
        // Debug.Log(System.IO.Path.Combine(Application.streamingAssetsPath, speakerName+".json"));
        TextAsset jsonTextFile = Resources.Load<TextAsset>(speakerName);
        dialogue = JsonUtility.FromJson<CharacterDialogue>(jsonTextFile.ToString());
        // StartCoroutine(loadStreamingAsset(speakerName+".json"));
        // string json = File.ReadAllText(Application.streamingAssetsPath + "/" + speakerName + ".json");
        // dialogue = JsonUtility.FromJson<CharacterDialogue>(json);
        // Debug.Log(dialogue.checkpointDialogue[0].dialogue[0].text);
    }
}
