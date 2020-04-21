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
        this.progress.addAffection(amount);
    }

    // Returns true if current checkpoint values have been met
    public bool completedCheckpoint() {
        //TODO: MAKE DEPENDENT ON FOLLOWERS
        int affection = progress.getCurrentAffection();
        int checkpointVal = vars.checkpointAffectionPts[this.getCurrentCheckpoint()];
        if (affection >= checkpointVal) return true;
        return false;
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
}
