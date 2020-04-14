using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProgress
{
    private int currentCheckpoint;
    private int currentProgress;
    private CharacterVars vars;

    public CharacterProgress(CharacterVars vars) {
        this.vars = vars;
    }
}
