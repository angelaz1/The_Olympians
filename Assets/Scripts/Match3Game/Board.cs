using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mister Taft Creates' YouTube Match-3 Tutorial
// https://www.youtube.com/playlist?list=PL4vbr3u7UKWrxEz75MqmTDd899cYAvQ_B

public enum BoardState {moving, stable};

public class Board : MonoBehaviour
{
    public BoardState boardState = BoardState.stable;
    public int xSize, ySize;
    public int offset;
    public GameObject[] orbs;
    public GameObject [,] allOrbs;
    private FindMatches findMatches;

    // Start is called before the first frame update
    void Start()
    {
        findMatches = FindObjectOfType<FindMatches>();
        allOrbs = new GameObject[xSize, ySize];
        SetUp();
    }

    private void SetUp()
    {
        for(int i = 0; i < xSize; i++)
        {
            for(int j = 0; j < ySize; j++)
            {
                Vector2 tempPosition = new Vector2(i, j + offset);
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
            }
        }
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
            allOrbs[col, row] = null;
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
        yield return new WaitForSeconds(0.4f);
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
                    Vector2 tempPosition = new Vector2(i, j + offset);
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
        yield return new WaitForSeconds(0.5f);
        while(MatchesOnBoard())
        {
            yield return new WaitForSeconds(0.5f);
            DestroyMatches();
        }
        yield return new WaitForSeconds(0.5f);
        boardState = BoardState.stable;
    }
}
