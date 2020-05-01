using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public enum CharacterExpression{Default, Amused, Happy, Mad, Surprised};
public class InteractUIManager : MonoBehaviour
{
    public Sprite[] heartStates;

    private GameObject phone;
    private GameObject buttons;
    private GameObject characterImage;
    private GameObject backgroundImage;

    private GameObject topBar;
    private GameObject topBarName;
    private GameObject[] hearts;
    private GameObject followerIcon;
    private GameObject followerCount;

    private GameObject feedback;
    private GameObject feedbackName;
    private GameObject feedbackText;
    private GameObject addedFollowersText;
    private bool mouseClicked = true;

    private Character currentCharacter;
    private bool phoneDown;
    
    private SFXManager sfxManager;

    void Start()
    {
       setEverything();
    }

    void setEverything() {
        phone = GameObject.Find("Phone");
        phoneDown = true;
        buttons = GameObject.Find("Buttons");
        setTopBar();
        characterImage = GameObject.Find("CharacterImage");
        backgroundImage = GameObject.Find("BackgroundImage");
        setFeedback();
        sfxManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
    }

    void Update() {
        if (!mouseClicked && Input.GetMouseButtonDown(0)) {
            mouseClicked = true;
            this.feedback.GetComponent<Animator>().ResetTrigger("flyUp");
            this.feedback.GetComponent<Animator>().SetTrigger("flyOff");
            GameObject.Find("InteractButtonManager").GetComponent<InteractButtonManager>().allFlyIn();
        }  
    }

    void setFeedback() {
        feedback = GameObject.Find("Feedback");
        foreach (Transform child in feedback.transform){
            if (child.name == "FeedbackName"){
                feedbackName = child.gameObject;
            }
            if (child.name == "FeedbackText"){
                feedbackText = child.gameObject;
            }
            if (child.name == "AddedFollowers"){
                addedFollowersText = child.gameObject;
            }
        }
    }

    void setTopBar() {
        topBar = GameObject.Find("TopBar");
        hearts = new GameObject[5];
        foreach (Transform child in topBar.transform){
            if (child.name == "Name"){
                topBarName = child.gameObject;
            }
            if (child.name == "Heart0"){
                hearts[0] = child.gameObject;
            }
            if (child.name == "Heart1"){
                hearts[1] = child.gameObject;
            }
            if (child.name == "Heart2"){
                hearts[2] = child.gameObject;
            }
            if (child.name == "Heart3"){
                hearts[3] = child.gameObject;
            }
            if (child.name == "Heart4"){
                hearts[4] = child.gameObject;
            }
            if (child.name == "FollowerIcon"){
                followerIcon = child.gameObject;
            }
            if (child.name == "FollowerCount"){
                followerCount = child.gameObject;
            }
        }
    }

    public void setCharacter(Character character) {
        currentCharacter = character;
        if(topBar == null) {
            setTopBar();
            topBarName.GetComponent<TextMeshProUGUI>().text = character.getName();
            //UPDATE HEARTS AND FOLLOWER COUNT
            updateHearts();
            updateFollowers();
        }
    }

    public void updateHearts() {
        if (topBar == null) {
            setEverything();
        }

        int currCheckpoint = currentCharacter.getCurrentCheckpoint();
        for(int i = 0; i < currCheckpoint; i++) {
            //All hearts before currCheckpoint are completed
            hearts[i].GetComponent<Image>().sprite = heartStates[heartStates.Length - 1];
        }
        if(currCheckpoint == 5) {
            return;
        }
        int currProgress = currentCharacter.getCurrentAffectionProgress();
        GameObject heart = hearts[currCheckpoint];
        if(currProgress < 20) {
            heart.GetComponent<Image>().sprite = heartStates[0];
        } else if(currProgress < 40) {
            heart.GetComponent<Image>().sprite = heartStates[1];
        } else if(currProgress < 60) {
            heart.GetComponent<Image>().sprite = heartStates[2];
        } else if(currProgress < 80) {
            heart.GetComponent<Image>().sprite = heartStates[3];
        } else if(currProgress < 100) {
            heart.GetComponent<Image>().sprite = heartStates[4];
        } else {
            heart.GetComponent<Image>().sprite = heartStates[5];
            heart.GetComponent<Image>().color = new Color32(255, 105, 163, 255);
        }
    }

    public void updateFollowers() {
        int currCheckpoint = currentCharacter.getCurrentCheckpoint();
        if(currCheckpoint == 5) {
            return;
        }
        int currFollowerGoal = currentCharacter.getCurrentFollowerGoal();
        int currFollowerCount = currentCharacter.getCurrentFollowerCount();
        followerCount.GetComponent<TextMeshProUGUI>().text = currFollowerCount + "/" + currFollowerGoal;
        if (currFollowerCount >= currFollowerGoal) {
            followerIcon.GetComponent<Image>().color = new Color32(0, 255, 188, 255);
        }
    }

    public void showTopBar() {
        if (topBar == null) setTopBar();
        topBar.GetComponent<Animator>().ResetTrigger("ShowBar");
        topBar.GetComponent<Animator>().SetTrigger("ShowBar");
    }

    public void setCharacterPortrait(CharacterExpression expr) {
        Sprite portrait = currentCharacter.getCharacterPortrait(expr.ToString());
        if(characterImage == null) {
            characterImage = GameObject.Find("CharacterImage");
        }
        if(portrait != null && portrait != characterImage.GetComponent<Image>().sprite) {
            characterImage.GetComponent<Image>().sprite = portrait;
            characterImage.GetComponent<Animator>().ResetTrigger("swappedState");
            characterImage.GetComponent<Animator>().SetTrigger("swappedState");
        }
    }

    public void setBackgroundImage() {
        Sprite background = currentCharacter.getBackground();
        if(backgroundImage == null) {
            backgroundImage = GameObject.Find("BackgroundImage");
        }
        if(background != null) {
            backgroundImage.GetComponent<Image>().sprite = background;
        }
    }

    public void showFeedback(Feedback feedback) {
        feedbackName.GetComponent<TextMeshProUGUI>().text = feedback.getFeedbackName();
        feedbackText.GetComponent<TextMeshProUGUI>().text = feedback.getFeedbackText();
        int addedFollowers = feedback.getAddedFollowers();
        if (addedFollowers >= 0) {
            addedFollowersText.GetComponent<TextMeshProUGUI>().text = "+" + addedFollowers + " followers";
            addedFollowersText.GetComponent<TextMeshProUGUI>().color = new Color32(7, 145, 30, 255);
        } else {
            addedFollowersText.GetComponent<TextMeshProUGUI>().text = addedFollowers + " followers";
            addedFollowersText.GetComponent<TextMeshProUGUI>().color = new Color32(145, 7, 7, 255);
        }
        
        this.feedback.GetComponent<Animator>().ResetTrigger("flyOff");
        this.feedback.GetComponent<Animator>().SetTrigger("flyUp");
        mouseClicked = false;
        sfxManager.playPhoneUpSound();
    }

    public void playAffectionParticles() {
        if (characterImage == null) {
            setEverything();
        }
        characterImage.GetComponentInChildren<ParticleSystem>().Play();
    }

    public void arrowPressed() {
        if (phoneDown) {
            phone.GetComponent<Animator>().SetTrigger("PullUpPhone");
            phone.GetComponent<Animator>().ResetTrigger("PutAwayPhone");
            sfxManager.playPhoneUpSound();
            phoneDown = false;
        } else {
            phone.GetComponent<Animator>().SetTrigger("PutAwayPhone");
            phone.GetComponent<Animator>().ResetTrigger("PullUpPhone");
            sfxManager.playPhoneDownSound();
            phoneDown = true;
        }
    }

    IEnumerator loadMapScene() {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene("MapDemo");    
    }

    public void loadMap() {
        phone.GetComponent<Animator>().SetTrigger("OpenApp");
        StartCoroutine(loadMapScene());
    }

    IEnumerator loadWinScreenScene() {
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene("WinScreen");   
    }

    public void loadWinScreen() {
        GameObject.Find("BigBlackScreen").GetComponent<Animator>().SetTrigger("MoveScreenIn");
        StartCoroutine(loadWinScreenScene());
    }

    IEnumerator loadPostScene() {
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene("PostDemo");   
    }

    public void loadPost() {
        GameObject.Find("BigBlackScreen").GetComponent<Animator>().SetTrigger("MoveScreenIn");
        StartCoroutine(loadPostScene());
    }

    IEnumerator loadMatch3GameScene() {
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene("Match3Game");   
    }

    public void loadMatch3Game() {
        GameObject.Find("BigBlackScreen").GetComponent<Animator>().SetTrigger("MoveScreenIn");
        StartCoroutine(loadMatch3GameScene());
    }

    public void exitGame() {
        Application.Quit();
    }
}
