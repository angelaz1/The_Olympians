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
    private SFXManager sfxManager;
    private PostingManager postingManager;
    private Board boardManager;
    private Dictionary<string, Character> allCharacters;

    private bool postedPhoto;
    private int photoQuality;
    private int addedFollowers;
    private Feedback nextFeedback;

    private bool justDated;
    private bool failedDate;

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
            sfxManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
            
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

        else if (scene.name == "Match3Game") {
            boardManager = GameObject.Find("BoardManager").GetComponent<Board>();
            boardManager.setVals(allCharacters[currentCharacterName]);
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
        } else if (justDated) {
            justDated = false;
            if (failedDate) {
                dialogueManager.startFailDateDialogue();
            } else {
                int currCheckpoint = allCharacters[currentCharacterName].getCurrentCheckpoint();
                if (currCheckpoint >= 5) {
                    dialogueManager.startFinalCheckpointDialogue();
                } else {
                    dialogueManager.startCheckpointDialogue(currCheckpoint);
                }
            }
        } else if (postedPhoto) {
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

    public void checkStartingDate() {
        Character c = allCharacters[currentCharacterName];

        if(c.completedCheckpoint()) {
            dialogueManager.startDateDialogue();
        } else {
            if(!c.completedAffection()) {
                sfxManager.playBadSound();
                dialogueManager.startInsufficientAffectionDialogue();
            } else {
                sfxManager.playBadSound();
                dialogueManager.startInsufficientFollowersDialogue();
            }
        }
    }

    public void advanceCheckpoint() {
        Character c = allCharacters[currentCharacterName];
        if(c.completedCheckpoint()) {
            c.advanceCheckpoint();
        }
    }

    public void addAffection(int amount) {
        Character c = allCharacters[currentCharacterName];
        c.addAffection(amount);
        if (amount > 0) {
            interactUIManager.playAffectionParticles();
            sfxManager.playGoodSound();
        } else if (amount < 0) {
            sfxManager.playBadSound();
        }
        interactUIManager.updateHearts();
    }

    public void addFollowers(int amount) {
        Character c = allCharacters[currentCharacterName];
        c.addFollowers(amount);
        interactUIManager.updateFollowers();
    }

    public void showFeedback() {
        interactUIManager.showFeedback(nextFeedback);
    }

    public void postImage(Filter filter, Caption caption) {
        postedPhoto = true;

        string feedbackText = "";
        if (filter == null || caption == null) {
            photoQuality = -1;
            addedFollowers = Random.Range(-400, -200);
            
            if (filter == null && caption == null) feedbackText = "no filter?? default text?? that's so sad, they legit don't even care";
            else if (filter == null) feedbackText = "omg did they not even use a filter? that's so weird!";
            else feedbackText = "pfft default caption text. shows how much they care, hm?";
        } else {
            int filterEff = filter.effectIndicator;
            int captionEff = caption.effectIndicator;
            if (filterEff > 0 && captionEff > 0) {
                // Good combination
                photoQuality = 1;
                addedFollowers = Random.Range(300, 500);

                feedbackText = "omg that " + filter.filterName + " filter really suits the image! and the caption matched it perfectly!";
            } else if (filterEff < 0 && captionEff < 0) {
                // Bad combination
                photoQuality = -1;
                addedFollowers = Random.Range(-200, 10);
                feedbackText = "wasn't that... like... the worst filter-caption-image combination you've ever seen?!";
            } else {
                // Ok combination
                photoQuality = 0;
                addedFollowers = Random.Range(50, 200);
                if (captionEff < filterEff) feedbackText = "the filter seems aight, but yikes, was that caption bad!!";
                else if (filterEff < captionEff) feedbackText = "ooo the " + filter.filterName + " filter is a little lacking...";
                else feedbackText = "ehh... both the filter and the caption could use improvement";
            } 
        }

        nextFeedback = new Feedback(addedFollowers, feedbackText);
    }

    public void setCharacter(string characterName) {
        currentCharacterName = characterName;
    }

    public void setDateCondition(bool passed) {
        this.justDated = true;
        this.failedDate = !passed;
        if (passed) {
            this.advanceCheckpoint();
        }
    }
}
