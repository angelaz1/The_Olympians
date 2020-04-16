using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProgress
{
    private int currentCheckpoint;
    private int currentAffection;
    private int currentFollowers;
    private CharacterVars vars;

    public CharacterProgress(string characterName) {
        this.vars = getVariablesFor(characterName);
        currentCheckpoint = 0;
        currentAffection = 0;
        currentFollowers = 0;
    }

    public CharacterVars getVariablesFor(string name) {
        switch(name) {
            case "Aphrodite": return new AphroditeVars();
            case "Ares": return new AresVars();
            default: return new AphroditeVars();
        }
    }

    public void addAffection(int amount) {
        currentAffection += amount;
    }

    public void addFollowers(int amount) {
        currentFollowers += amount;
    }
}
