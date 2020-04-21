using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProgress
{
    private int currentCheckpoint;
    private int currentAffection;
    private int currentFollowers;
    // private CharacterVars vars;

    public CharacterProgress() {
        // this.vars = getVariablesFor(characterName);
        currentCheckpoint = 0;
        currentAffection = 0;
        currentFollowers = 0;
    }

    public void addAffection(int amount) {
        currentAffection += amount;
        if (currentAffection < 0) currentAffection = 0;
    }

    public void addFollowers(int amount) {
        currentFollowers += amount;
        if (currentFollowers < 0) currentFollowers = 0;
    }

    public int getCurrentCheckpoint() {
        return currentCheckpoint;
    }

    public int getCurrentAffection() {
        return currentAffection;
    }

    public int getCurrentFollowers() {
        return currentFollowers;
    }
}
