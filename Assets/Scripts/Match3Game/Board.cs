using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Mister Taft Creates' YouTube Match-3 Tutorial
// https://www.youtube.com/playlist?list=PL4vbr3u7UKWrxEz75MqmTDd899cYAvQ_B

public enum BoardState {moving, stable};

public class Board : MonoBehaviour
{
    public static Board bm;

    [Header("Board Vars")]
    public BoardState boardState = BoardState.stable;
    public int xSize, ySize;
    private float yOffset = 0.5f;

    public GameObject[] orbs;
    public GameObject [,] allOrbs;
    private FindMatches findMatches;

    [Header("UI Elements")]
    public GameObject MovesText;
    public GameObject ScoreText;
    // public GameObject ComboText;
    public GameObject GoalText;
    public int scoreReq;
    private int score = 0;
    public int moveLimit;
    private int movesMade = 0;
    private int baseOrbValue = 100;
    private int multiplier = 1;

    private Character character;
    private GameObject characterImage;
    private bool isHappy;
    private bool isTiming;

    private MatchSFXManager sfxManager;

    void Start()
    {
        bm = GetComponent<Board>();
        sfxManager = FindObjectOfType<MatchSFXManager>();
        findMatches = FindObjectOfType<FindMatches>();
        allOrbs = new GameObject[xSize, ySize];

        MovesText.GetComponent<Text>().text = movesMade + " / " + moveLimit;
        ScoreText.GetComponent<Text>().text = score + " / " + scoreReq;
    }

    public void putAwayPhone() {
        StartCoroutine(SetEverythingUp());
    }

    IEnumerator SetEverythingUp() {
        GameObject.Find("Phone").GetComponent<Animator>().SetTrigger("PutAway");
        sfxManager.playPhoneDown();
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(SetUp());
    }

    void Update()
    {
        MovesText.GetComponent<Text>().text = movesMade + " / " + moveLimit;
        ScoreText.GetComponent<Text>().text = score + " / " + scoreReq;

        if(boardState == BoardState.stable)
        {
            // TODO: UNCOMMENT
            if(score >= scoreReq)
            {
                // Debug.Log("Victory! Checkpoint passed!");
                backToMainGame(true);
            }
            else if (movesMade >= moveLimit && score < scoreReq)
            {
                // Debug.Log("Defeat! Try again?");
                backToMainGame(false);
            }
        }
    }

    public void setVals(Character character) {
        this.character = character;
        this.moveLimit = character.getMoveLimit();
        this.scoreReq = character.getScoreReq();

        this.characterImage = GameObject.Find("CharacterImage");
        characterImage.GetComponent<Image>().sprite = character.getCharacterPortrait("Default");
        isHappy = false;
        isTiming = false;
        sfxManager = FindObjectOfType<MatchSFXManager>();
        sfxManager.playPhoneUp();
    }

    public void swapToHappy() {
        if (!isHappy) {
            characterImage.GetComponent<Image>().sprite = character.getCharacterPortrait("Happy");
            characterImage.GetComponent<Animator>().ResetTrigger("swappedState");
            characterImage.GetComponent<Animator>().SetTrigger("swappedState");
            isHappy = true;
        }
        if (isTiming) {
            isTiming = false;
            characterImage.GetComponent<Animator>().ResetTrigger("swappedState");
            characterImage.GetComponent<Animator>().SetTrigger("swappedState");
        }
    }

    IEnumerator timerSwapDefault() {
        yield return new WaitForSeconds(1.0f);
        if (isTiming) {
            characterImage.GetComponent<Image>().sprite = character.getCharacterPortrait("Default");
            characterImage.GetComponent<Animator>().ResetTrigger("swappedState");
            characterImage.GetComponent<Animator>().SetTrigger("swappedState");
            isHappy = false;
            isTiming = false;
        }
    }

    public void swapToDefault() {
        if (isHappy) {
            isTiming = true;
            StartCoroutine(timerSwapDefault());
        }
    }



    private IEnumerator SetUp()
    {
        for(int j = 0; j < ySize; j++)
        {
            for(int i = 0; i < xSize; i++)
            {
                Vector2 tempPosition = new Vector2(i, ySize + yOffset);
                int orbToUse = Random.Range(0, orbs.Length);

                int maxIterations = 0;
                while(MatchesAt(i, j, orbs[orbToUse]) && maxIterations < 100)
                {
                    orbToUse = Random.Range(0, orbs.Length);
                    maxIterations++;
                }
                maxIterations = 0;

                GameObject orb = Instantiate(orbs[orbToUse], tempPosition, Quaternion.identity);
                orb.GetComponent<Orb>().col = i;
                orb.GetComponent<Orb>().row = j;
                orb.transform.parent = this.transform;
                orb.name = "(" + i + ", " + j + ")";
                allOrbs[i,j] = orb;
                sfxManager.playPopSound();
                yield return new WaitForSeconds(0.03f);
            }
        }
    }

    public void AddMove()
    {
        movesMade++;
    }

    private bool MatchesAt(int col, int row, GameObject orb)
    {
        if(col > 1 && row > 1)
        {
            if(allOrbs[col - 1, row].tag == orb.tag && allOrbs[col - 2, row].tag == orb.tag)
            {
                return true;
            }
            if(allOrbs[col, row - 1].tag == orb.tag && allOrbs[col, row - 2].tag == orb.tag)
            {
                return true;
            }
        }
        else if(col <= 1 || row <= 1)
        {
            if(row > 1)
            {
                if(allOrbs[col, row - 1].tag == orb.tag && allOrbs[col, row - 2].tag == orb.tag)
                {
                    return true;
                }
            }
            if(col > 1)
            {
                if(allOrbs[col - 1, row].tag == orb.tag && allOrbs[col - 2, row].tag == orb.tag)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void DestroyMatchesAt(int col, int row)
    {
        if(allOrbs[col, row].GetComponent<Orb>().isMatched)
        {
            findMatches.currMatches.Remove(allOrbs[col, row]);
            Destroy(allOrbs[col, row]);
            score += baseOrbValue * multiplier;
            allOrbs[col, row] = null;
        }
    }

    public void playSound() {
        if (multiplier == 1) {
            sfxManager.playOkCombo();
        } else if (multiplier == 2) {
            sfxManager.playGoodCombo();
        } else {
            sfxManager.playGreatCombo();
        }
    }

    public void DestroyMatches()
    {
        for(int i = 0; i < xSize; i++)
        {
            for(int j = 0; j < ySize; j++)
            {
                if(allOrbs[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(DecreaseRowCo());
    }

    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        for(int i = 0; i < xSize; i++)
        {
            for(int j = 0; j < ySize; j++)
            {
                if(allOrbs[i, j] == null)
                {
                    nullCount++;
                }
                else if(nullCount > 0)
                {
                    allOrbs[i, j].GetComponent<Orb>().row -= nullCount;
                    allOrbs[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()
    {
        for(int i = 0; i < xSize; i++)
        {
            for(int j = 0; j < ySize; j++)
            {
                if(allOrbs[i, j] == null)
                {
                    Vector2 tempPosition = new Vector2(i, ySize + yOffset);
                    int orbToUse = Random.Range(0, orbs.Length);
                    GameObject orb = Instantiate(orbs[orbToUse], tempPosition, Quaternion.identity);
                    allOrbs[i, j] = orb;
                    orb.GetComponent<Orb>().col = i;
                    orb.GetComponent<Orb>().row = j;
                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for(int i = 0; i < xSize; i++)
        {
            for(int j = 0; j < ySize; j++)
            {
                if(allOrbs[i, j] != null)
                {
                    if(allOrbs[i, j].GetComponent<Orb>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        findMatches.FindAllMatches();
        yield return new WaitForSeconds(0.2f);
        if(MatchesOnBoard()) {
            multiplier++;
            DestroyMatches();
        } else {
            boardState = BoardState.stable;
            multiplier = 1;
            swapToDefault();
        }
    }

    public void backToMainGame(bool passed) {
        GameObject.Find("GameManager").GetComponent<GameManager>().setDateCondition(passed);
        SceneManager.LoadScene("PhoneUIDemo");
    }
}
