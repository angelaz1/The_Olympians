using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    private string currentCharacterName = "Aphrodite";

    private GameObject characterImage;
    private DialogueManager dialogueManager;
    private Dictionary<string, CharacterProgress> allCharacterProgress;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "PhoneUIDemo") {
            dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
            dialogueManager.setSpeakerName(currentCharacterName);
            dialogueManager.readDialogue();

            characterImage = GameObject.Find("CharacterImage");
            if (allCharacterProgress == null) {
                allCharacterProgress = new Dictionary<string, CharacterProgress>();
            }

            startGreetingDialogue();
        }
    }

    void OnDisable()
    {
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
        // TODO: MAKE GENERIC
        if (!allCharacterProgress.ContainsKey(currentCharacterName)) {
            CharacterProgress characterProgress = new CharacterProgress(new AphroditeVars());
            allCharacterProgress.Add(currentCharacterName, characterProgress);
            dialogueManager.startCheckpointDialogue(0);
        } else {
            dialogueManager.startGreetingDialogue();
        }
    }

    public void addAffection(int amount) {
        CharacterProgress p = allCharacterProgress[currentCharacterName];
        p.addAffection(amount);
        if (amount > 0) {
            characterImage.GetComponent<ParticleSystem>().Play();
        }
    }

    public void moveToLocation(string location) {
        if (location == "Mall") {
            currentCharacterName = "Aphrodite";
        }
        SceneManager.LoadScene("PhoneUIDemo");
    }
}
