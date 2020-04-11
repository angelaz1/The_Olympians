using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private string currentCharacterName;
    private DialogueManager dialogueManager;
    // Start is called before the first frame update
    void Start()
    {
        currentCharacterName = "Aphrodite";

        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        dialogueManager.setSpeakerName(currentCharacterName);
        dialogueManager.readDialogue();

        // Read/Write to file for character data
        // Have a file containing all the set variables for a character (i.e. checkpoint numbers, 
        // path to the character sprite, Instagram pictures, other responses to posts)
        // Or would we want to make this just a class? We can do that. Then we can also keep
        // track of special flags?? 
        // Then have a file containing the user's progress towards this character
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
