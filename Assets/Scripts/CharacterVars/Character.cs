using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    private CharacterProgress progress;
    private CharacterDialogue dialogue;
    private CharacterVars vars;
    private string name;
    private bool hasntMet;

    public Character(string characterName) {
        this.name = characterName;
        this.progress = new CharacterProgress();
        
        TextAsset jsonTextFile = Resources.Load<TextAsset>(name);
        this.vars = JsonUtility.FromJson<CharacterVars>(jsonTextFile.ToString());
        readDialogue(vars.dialoguePath);

        this.hasntMet = true;
    }

    void readDialogue(string dialoguePath) {
        TextAsset jsonTextFile = Resources.Load<TextAsset>(dialoguePath);
        this.dialogue = JsonUtility.FromJson<CharacterDialogue>(jsonTextFile.ToString());
    }

    public CharacterDialogue getDialogue() {
        return this.dialogue;
    }

    public string getName() {
        return name;
    }

    public int getCurrentCheckpoint() {
        return progress.getCurrentCheckpoint();
    }

    public int getCurrentAffectionProgress() {
        int affection = progress.getCurrentAffection();
        int checkpointVal = vars.checkpointAffectionPts[this.getCurrentCheckpoint()];
        if (affection > checkpointVal) affection = checkpointVal;
        return affection * 100 / checkpointVal;
    }

    public bool isFirstMeeting() {
        if(hasntMet) {
            hasntMet = false;
            return true;
        }
        return false;
    }

    public void addAffection(int amount) {
        this.progress.addAffection(amount);
    }

    public bool completedCheckpoint() {
        //TODO: MAKE DEPENDENT ON FOLLOWERS
        int affection = progress.getCurrentAffection();
        int checkpointVal = vars.checkpointAffectionPts[this.getCurrentCheckpoint()];
        if (affection >= checkpointVal) return true;
        return false;
    } 

    public Sprite getCharacterPortrait(string name) {
        string path = vars.portraitPath + (name.ToLower());
        return Resources.Load<Sprite>(path);
    } 

    public Sprite getBackground() {
        string path = vars.backgroundPath;
        return Resources.Load<Sprite>(path);
    }
}
