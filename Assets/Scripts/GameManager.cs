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
    private bool postedPhoto;
    private int photoQuality;
    private int addedFollowers;

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
            checkDateUnlocked();
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
        } else if(postedPhoto) {
            postedPhoto = false;
            addFollowers(addedFollowers);
            switch (photoQuality) {
                case -1: dialogueManager.startBadPostDialogue(); break;
                case 0: dialogueManager.startOkPostDialogue(); break;
                case 1: dialogueManager.startGoodPostDialogue(); break;
            }
        } else {
            dialogueManager.startGreetingDialogue();
        }
    }

    void checkDateUnlocked() {
        Character c = allCharacters[currentCharacterName];

        if(c.completedCheckpoint()) {
            interactUIManager.unlockDate();
        } else {
            interactUIManager.lockDate();
        }
    }

    public void addAffection(int amount) {
        Character c = allCharacters[currentCharacterName];
        c.addAffection(amount);
        if (amount > 0) {
            interactUIManager.playAffectionParticles();
        }
        interactUIManager.updateHearts();
        
        checkDateUnlocked();
    }

    public void addFollowers(int amount) {
        Character c = allCharacters[currentCharacterName];
        c.addFollowers(amount);
        interactUIManager.updateFollowers();
        
        checkDateUnlocked();
    }

    public void postImage(Filter filter, Caption caption) {
        postedPhoto = true;
        if (filter == null || caption == null) {
            photoQuality = -1;
            addedFollowers = Random.Range(-400, -200);
            // Add feedback
            return;
        }

        int filterEff = filter.effectIndicator;
        int captionEff = caption.effectIndicator;
        if (filterEff > 0 && captionEff > 0) {
            // Good combination
            photoQuality = 1;
            addedFollowers = Random.Range(300, 500);
        } else if (filterEff < 0 && captionEff < 0) {
            // Bad combination
            photoQuality = -1;
            addedFollowers = Random.Range(-200, 10);
        } else {
            // Ok combination
            photoQuality = 0;
            addedFollowers = Random.Range(50, 200);
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
