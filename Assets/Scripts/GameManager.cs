using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    private string currentCharacterName;
    private DialogueManager dialogueManager;
    private Dictionary<string, CharacterProgress> allCharacterProgress;

    private void Awake() {
        if (gm == null)
        {
            gm = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {   
        allCharacterProgress = new Dictionary<string, CharacterProgress>();
        currentCharacterName = "Aphrodite";

        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        dialogueManager.setSpeakerName(currentCharacterName);
        dialogueManager.readDialogue();

        startGreetingDialogue();

        // Read/Write to file for character data
        // Have a file containing all the set variables for a character (i.e. checkpoint numbers, 
        // path to the character sprite, Instagram pictures, other responses to posts)
        // Or would we want to make this just a class? We can do that. Then we can also keep
        // track of special flags?? 
        // Then have a file containing the user's progress towards this character
    }

    void startGreetingDialogue() {
        // Check if this is first time meeting character
        // TODO: MAKE GENERIC
        if (!allCharacterProgress.ContainsKey(currentCharacterName)) {
            CharacterProgress characterProgress = new CharacterProgress(new AphroditeVars());
            allCharacterProgress.Add(currentCharacterName, characterProgress);
            Debug.Log("Hello");
            dialogueManager.startCheckpointDialogue(0);
        } else {
            // dialogueManager.startGreetingDialogue();
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
