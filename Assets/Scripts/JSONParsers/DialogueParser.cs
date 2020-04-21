[System.Serializable]
public class CheckpointDialogue
{
    public int checkpointNum;
    public Dialogue[] dialogue;
}

[System.Serializable]
public class Dialogue
{
    public int id;
    public string text;
    public int numOptions;
    public string state;
    public Option[] options;
}

[System.Serializable]
public class Option
{
    public string optionText;
    public int link;
    public int affectionBonus;
}

[System.Serializable]
public class OtherDialogue
{
    public Dialogue[] dialogue;
}

[System.Serializable]
public class CharacterDialogue
{
    public string name;
    public CheckpointDialogue[] checkpointDialogue;
    public OtherDialogue[] conversationDialogue;
    public OtherDialogue[] greetDialogue;
    public OtherDialogue[] dateDialogue;
    public OtherDialogue[] postDialogue;
    public OtherDialogue[] badPostDialogue;
    public OtherDialogue[] okPostDialogue;
    public OtherDialogue[] goodPostDialogue;   
}