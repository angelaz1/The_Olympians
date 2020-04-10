using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager dm;

    // We will use the speakerName to find and parse the dialogue JSON
    // The dialogue for all characters should be in a JSON file named their name
    public string speakerName;
    private CharacterDialogue dialogue;

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
        speakerName = "Aphrodite";
        readDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startConversationDialogue() {
        // Pick a random conversation dialogue to do
        int convoLen = dialogue.conversationDialogue.Length;
        int index = Random.Range(0, convoLen);
 
        Dialogue[] selected = dialogue.conversationDialogue[index].dialogue;
        GameObject.Find("DialogueUIManager").GetComponent<DialogueUIManager>().startDialogue(selected);
    }

    public void finishDialogue() {
        GameObject.Find("InteractButtonManager").GetComponent<InteractButtonManager>().allFlyIn();
    }

    public void readDialogue() {
        string json = File.ReadAllText(Application.dataPath + "/" + speakerName + ".json");
        dialogue = JsonUtility.FromJson<CharacterDialogue>(json);
        Debug.Log(dialogue.checkpointDialogue[0].dialogue[0].text);
    }
}
