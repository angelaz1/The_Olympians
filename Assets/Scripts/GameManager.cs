using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    private string currentCharacterName = "Aphrodite";
    private DialogueManager dialogueManager;
    private InteractUIManager interactUIManager;
    private PostingManager postingManager;
    private Dictionary<string, Character> allCharacters;

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (allCharacters == null) {
            allCharacters = new Dictionary<string, Character>();
        }

        if (!allCharacters.ContainsKey(currentCharacterName)) {
            Character character = new Character(currentCharacterName);
            allCharacters.Add(currentCharacterName, character);
        }

        if (scene.name == "PhoneUIDemo") {
            dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
            interactUIManager = GameObject.Find("InteractUIManager").GetComponent<InteractUIManager>();
            
            dialogueManager.setDialogue(allCharacters[currentCharacterName].getDialogue());
            interactUIManager.setCharacter(allCharacters[currentCharacterName]);
            interactUIManager.setCharacterPortrait(CharacterExpression.Default);
            interactUIManager.setBackgroundImage();
            startGreetingDialogue();
        }

        else if (scene.name == "PostDemo") {
            postingManager = GameObject.Find("PostingManager").GetComponent<PostingManager>();

            postingManager.setPostImages(allCharacters[currentCharacterName].getPostImages());
            postingManager.startPostingMinigame();
        }
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

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

    void startGreetingDialogue() {
        // Check if this is first time meeting character
        if (allCharacters[currentCharacterName].isFirstMeeting()) {
            dialogueManager.startCheckpointDialogue(0);
        } else {
            dialogueManager.startGreetingDialogue();
        }
    }

    public void addAffection(int amount) {
        Character c = allCharacters[currentCharacterName];
        c.addAffection(amount);
        if (amount > 0) {
            interactUIManager.playAffectionParticles();
        }
        interactUIManager.updateHearts();
        
        if(c.completedCheckpoint()) {
            interactUIManager.unlockDate();
        } else {
            interactUIManager.lockDate();
        }
    }

    public void moveToLocation(string location) {
        switch(location) {
            case "Mall": {
                currentCharacterName = "Aphrodite";
                break;
            }
            case "Gym": {
                currentCharacterName = "Ares";
                break;
            }
            default: break;
        }

        SceneManager.LoadScene("PhoneUIDemo");
    }
}
