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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void readDialogue() {
        string json = File.ReadAllText(Application.dataPath + "/" + speakerName + ".json");
        dialogue = JsonUtility.FromJson<CharacterDialogue>(json);
        Debug.Log(dialogue.checkpointDialogue[0].dialogue[0].text);
    }
}
