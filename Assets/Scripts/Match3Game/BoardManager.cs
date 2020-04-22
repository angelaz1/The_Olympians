using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardManager : MonoBehaviour
{
    public static BoardManager bm;
    public List<Sprite> orbSprites;
    public GameObject tile;
    public int xSize, ySize;
    public bool isShifting;

    private GameObject[,] tiles;

    void Start()
    {
        bm = GetComponent<BoardManager>(); 

        Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;
        CreateBoard(offset.x, offset.y);
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

    private IEnumerator ShiftTilesDown(int x, int yStart, float shiftDelay = .03f) {
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

    public void backToMainGame() {
        // REPLACE LATER:
        GameObject.Find("GameManager").GetComponent<GameManager>().advanceCheckpoint();
        SceneManager.LoadScene("PhoneUIDemo");
    }
}
