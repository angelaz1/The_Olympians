using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mister Taft Creates' YouTube Match-3 Tutorial
// https://www.youtube.com/playlist?list=PL4vbr3u7UKWrxEz75MqmTDd899cYAvQ_B

public class FindMatches : MonoBehaviour
{
    private Board board;
    public List<GameObject> currMatches = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        board = GameObject.Find("BoardManager").GetComponent<Board>();
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    public IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(0.0f);
        for(int i = 0; i < board.xSize; i++)
        {
            for(int j = 0; j < board.ySize; j++)
            {
                GameObject currOrb = board.allOrbs[i, j];
                if(currOrb != null)
                {
                    if(i > 0 && i < board.xSize - 1)
                    {
                        GameObject leftOrb = board.allOrbs[i - 1, j];
                        GameObject rightOrb = board.allOrbs[i + 1, j];
                        if(leftOrb != null && rightOrb != null)
                        {
                            if(leftOrb.tag == currOrb.tag && rightOrb.tag == currOrb.tag)
                            {
                                if(!currMatches.Contains(leftOrb))
                                {
                                    currMatches.Add(leftOrb);
                                }
                                if(!currMatches.Contains(rightOrb))
                                {
                                    currMatches.Add(rightOrb);
                                }
                                if(!currMatches.Contains(currOrb))
                                {
                                    currMatches.Add(currOrb);
                                }
                                leftOrb.GetComponent<Orb>().isMatched = true;
                                rightOrb.GetComponent<Orb>().isMatched = true;
                                currOrb.GetComponent<Orb>().isMatched = true;
                            }
                        }
                    }

                    if(j > 0 && j < board.ySize - 1)
                    {
                        GameObject upOrb = board.allOrbs[i, j + 1];
                        GameObject downOrb = board.allOrbs[i, j - 1];
                        if(upOrb != null && downOrb != null)
                        {
                            if(upOrb.tag == currOrb.tag && downOrb.tag == currOrb.tag)
                            {
                                if(!currMatches.Contains(upOrb))
                                {
                                    currMatches.Add(upOrb);
                                }
                                if(!currMatches.Contains(downOrb))
                                {
                                    currMatches.Add(downOrb);
                                }
                                upOrb.GetComponent<Orb>().isMatched = true;
                                downOrb.GetComponent<Orb>().isMatched = true;
                                currOrb.GetComponent<Orb>().isMatched = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
