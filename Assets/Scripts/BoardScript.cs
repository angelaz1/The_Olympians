using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScript : MonoBehaviour
{
    // Mister Taft Creates' Youtube Match-3 Game Tutorial
    // https://www.youtube.com/playlist?list=PL4vbr3u7UKWrxEz75MqmTDd899cYAvQ_B
    
    public int startX, startY;
    public int width;
    public int height;

    public GameObject[] orbs;
    public GameObject tilePrefab;
    private BackgroundTile[,] boardTiles;
    public GameObject[,] allOrbs;

    // Start is called before the first frame update
    void Start()
    {
        boardTiles = new BackgroundTile[width, height];
        allOrbs = new GameObject[width, height];
        SetupBoard();
    }

    private void SetupBoard()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                Vector2 pos = new Vector2(i + startX, j + startY);
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity);
                tile.transform.parent = this.transform;
                tile.name = "tile: (" + i  + ", " + j + ")";
                int getOrb = Random.Range(0, orbs.Length);
                GameObject orb = Instantiate(orbs[getOrb], pos, Quaternion.identity);
                orb.transform.parent = this.transform;
                orb.name = "orb: (" + i  + ", " + j + ")";
                allOrbs[i, j] = orb;
            }
        }
    }
}
