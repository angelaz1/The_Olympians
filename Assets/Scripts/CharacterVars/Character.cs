using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    private CharacterProgress progress;
    private CharacterDialogue dialogue;
    private CharacterPostImages postImages;
    private CharacterVars vars;
    private string name;
    private bool hasntMet;

    // Constructor for a character
    public Character(string characterName) {
        this.name = characterName;
        this.progress = new CharacterProgress();
        
        TextAsset jsonTextFile = Resources.Load<TextAsset>(name);
        this.vars = JsonUtility.FromJson<CharacterVars>(jsonTextFile.ToString());
        readDialogue(vars.dialoguePath);
        readPostImages(vars.postImagePath);

        this.hasntMet = true;
    }

    // Reads the dialogue from the path
    void readDialogue(string dialoguePath) {
        TextAsset jsonTextFile = Resources.Load<TextAsset>(dialoguePath);
        this.dialogue = JsonUtility.FromJson<CharacterDialogue>(jsonTextFile.ToString());
    }

    // Reads the post images info from the path
    void readPostImages(string postImagePath) {
        TextAsset jsonTextFile = Resources.Load<TextAsset>(postImagePath);
        this.postImages = JsonUtility.FromJson<CharacterPostImages>(jsonTextFile.ToString());
    }

    // Accessor method for dialogue
    public CharacterDialogue getDialogue() {
        return this.dialogue;
    }

    // Accessor method for postImages
    public CharacterPostImages getPostImages() {
        return this.postImages;
    }

    // Accessor method for name of character
    public string getName() {
        return name;
    }

    // Accessor method for current checkpoint with character
    public int getCurrentCheckpoint() {
        return progress.getCurrentCheckpoint();
    }

    // Accessor method for current affection progress with character
    public int getCurrentAffectionProgress() {
        int affection = progress.getCurrentAffection();
        int checkpointVal = vars.checkpointAffectionPts[this.getCurrentCheckpoint()];
        if (affection > checkpointVal) affection = checkpointVal;
        return affection * 100 / checkpointVal;
    }

    // Accessor method for follower goal with character
    public int getCurrentFollowerGoal() {
        return vars.checkpointFollowerCount[this.getCurrentCheckpoint()];
    }

    // Accessor method for follower count with character
    public int getCurrentFollowerCount() {
        return progress.getCurrentFollowers();
    }

    // Checks if this is the first meeting with the character; if so, sets to false
    public bool isFirstMeeting() {
        if(hasntMet) {
            hasntMet = false;
            return true;
        }
        return false;
    }

    // Mutator method that adds affection
    public void addAffection(int amount) {
        if (progress.getCurrentAffection() < vars.checkpointAffectionPts[this.getCurrentCheckpoint()]
            && amount > 0) {
            this.progress.addAffection(amount);
        }
        else if (progress.getCurrentAffection() > 0 && amount < 0) {
            this.progress.addAffection(amount);
        }
    }

    // Mutator method that adds followers
    public void addFollowers(int amount) {
        this.progress.addFollowers(amount);
    }

    // Mutator method that adds followers
    public void advanceCheckpoint() {
        this.progress.advanceCheckpoint();
    }

    public bool completedAffection() {
        int affection = progress.getCurrentAffection();
        int checkpointAffectionVal = vars.checkpointAffectionPts[this.getCurrentCheckpoint()];
        return affection >= checkpointAffectionVal;
    }

    public bool completedFollowers() {
        int followers = progress.getCurrentFollowers();
        int checkpointFollowerVal = vars.checkpointFollowerCount[this.getCurrentCheckpoint()];
        return followers >= checkpointFollowerVal;
    }

    // Returns true if current checkpoint values have been met
    public bool completedCheckpoint() {
        return completedAffection() && completedFollowers();
    } 

    // Accessor method for character portrait
    public Sprite getCharacterPortrait(string name) {
        string path = vars.portraitPath + (name.ToLower());
        return Resources.Load<Sprite>(path);
    } 

    // Accessor method for character background
    public Sprite getBackground() {
        string path = vars.backgroundPath;
        return Resources.Load<Sprite>(path);
    }

    // Accessor method for date moveLimit
    public int getMoveLimit() {
        return vars.moveLimit[this.getCurrentCheckpoint()];
    }

    // Accessor method for date scoreReq
    public int getScoreReq() {
        return vars.dateScoreReq[this.getCurrentCheckpoint()];
    }
}
