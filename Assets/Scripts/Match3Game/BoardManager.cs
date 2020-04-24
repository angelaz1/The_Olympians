using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public static BoardManager bm;
    public List<Sprite> orbSprites;
    public GameObject tile;
    public int xSize, ySize;
    public bool isShifting;
    public int scoreReq;
    private int score = 0;
    public int moveLimit;
    private int movesMade = 0;
    private int multiplier = 1;

    public GameObject MovesText;
    public GameObject ScoreText;
    public GameObject ComboText;
    public GameObject GoalText;

    private GameObject[,] tiles;

    void Start()
    {
        bm = GetComponent<BoardManager>(); 

        Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;
        CreateBoard(offset.x, offset.y);

        MovesText.GetComponent<Text>().text = movesMade + " / " + moveLimit;
        ScoreText.GetComponent<Text>().text = "" + score;
        ComboText.GetComponent<Text>().text = "x" + multiplier;
        GoalText.GetComponent<Text>().text = "" + scoreReq;
    }

    void Update()
    {
        MovesText.GetComponent<Text>().text = movesMade + " / " + moveLimit;
        ScoreText.GetComponent<Text>().text = "" + score;
        ComboText.GetComponent<Text>().text = "x" + multiplier;
    }

    private void CreateBoard (float xOffset, float yOffset) {
        tiles = new GameObject[xSize, ySize];    

        float startX = transform.position.x;     
        float startY = transform.position.y;

        Sprite[] previousLeft = new Sprite[ySize];
        Sprite previousBelow = null;
        for (int x = 0; x < xSize; x++) {
            for (int y = 0; y < ySize; y++) {
                GameObject newTile = Instantiate(tile, 
                    new Vector3(startX + (xOffset * (x - ((xSize - 1) / 2.0f))), startY + (yOffset * (y - ((ySize - 1) / 2.0f))), 0), 
                    tile.transform.rotation);
                tiles[x, y] = newTile;
                newTile.transform.parent = transform;

                List<Sprite> possibleOrbs = new List<Sprite>();
                possibleOrbs.AddRange(orbSprites); 
                possibleOrbs.Remove(previousLeft[y]); 
                possibleOrbs.Remove(previousBelow);

                Sprite newSprite = possibleOrbs[Random.Range(0, possibleOrbs.Count)];
                newTile.GetComponent<SpriteRenderer>().sprite = newSprite;

                previousLeft[y] = newSprite;
                previousBelow = newSprite;
            }
        }
    }

    public IEnumerator FindNullTiles() {
        for (int x = 0; x < xSize; x++) {
            for (int y = 0; y < ySize; y++) {
                if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == null) {
                    score += 100 * multiplier;
                    yield return StartCoroutine(ShiftTilesDown(x, y));
                    break;
                }
            }
        }

        for (int x = 0; x < xSize; x++) {
            for (int y = 0; y < ySize; y++) {
                tiles[x, y].GetComponent<Tile>().ClearAllMatches();
            }
        }
    }

    private IEnumerator ShiftTilesDown(int x, int yStart, float shiftDelay = .1f) {
        isShifting = true;
        List<SpriteRenderer> renders = new List<SpriteRenderer>();
        int nullCount = 0;

        for (int y = yStart; y < ySize; y++) { 
            SpriteRenderer render = tiles[x, y].GetComponent<SpriteRenderer>();
            if (render.sprite == null) { 
                nullCount++;
            }
            renders.Add(render);
        }

        for (int i = 0; i < nullCount; i++) { 
            yield return new WaitForSeconds(shiftDelay);
            for (int k = 0; k < renders.Count - 1; k++) {
                renders[k].sprite = renders[k + 1].sprite;
                renders[k + 1].sprite = GetNewSprite(x, ySize - 1);
            }
        }

        isShifting = false;
        // Multiplier not working CORRECTLY yet, but does "work"
        AddMultiplier();
        Debug.Log(multiplier);
    }

    private Sprite GetNewSprite(int x, int y) {
        List<Sprite> possibleOrbs = new List<Sprite>();
        possibleOrbs.AddRange(orbSprites);

        if (x > 0) {
            possibleOrbs.Remove(tiles[x - 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (x < xSize - 1) {
            possibleOrbs.Remove(tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (y > 0) {
            possibleOrbs.Remove(tiles[x, y - 1].GetComponent<SpriteRenderer>().sprite);
        }

        return possibleOrbs[Random.Range(0, possibleOrbs.Count)];
    }

    public void MakeMove()
    {
        movesMade++;
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

    public void AddMultiplier()
    {
        if(multiplier < 3)
        {
            multiplier++;
        }
        StopCoroutine(ResetMultiplier());
		StartCoroutine(ResetMultiplier());
    }

    private IEnumerator ResetMultiplier()
    {
        while(multiplier > 1)
        {
            yield return new WaitForSeconds(3.0f);
            multiplier--;
        }
    }

    public void backToMainGame(bool passed) {
        // REPLACE LATER:
        GameObject.Find("GameManager").GetComponent<GameManager>().setDateCondition(passed);
        SceneManager.LoadScene("PhoneUIDemo");
    }
}
