using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OrbMove {None, Up, Down, Left, Right};

public class OrbManager : MonoBehaviour
{
    // Mister Taft Creates' Youtube Match-3 Game Tutorial
    // https://www.youtube.com/playlist?list=PL4vbr3u7UKWrxEz75MqmTDd899cYAvQ_B

    // The first orb's movement direction
    private OrbMove orbMove = OrbMove.None;
    private BoardScript board;
    private int firstOrbX, firstOrbY, finalOrbX, finalOrbY;
    private GameObject firstOrb;
    private GameObject finalOrb;
    private static bool firstClick = true;
    private Vector2 tempPositionFirst;
    private Vector2 tempPositionFinal;

    // Start is called before the first frame update
    void Start()
    {
        board = GameObject.Find("Board").GetComponent<BoardScript>();        
    }

    // Update is called once per frame
    void Update()
    {
        if(orbMove == OrbMove.Up || orbMove == OrbMove.Down)
        {
            tempPositionFirst = new Vector2(firstOrbX, finalOrbY);
            tempPositionFinal = new Vector2(finalOrbX, firstOrbY);
            if(Mathf.Abs(firstOrb.transform.position.y - finalOrbY) > 0.1f)
            {
                firstOrb.transform.position = Vector2.Lerp(firstOrb.transform.position, tempPositionFirst, 0.4f);
                finalOrb.transform.position = Vector2.Lerp(finalOrb.transform.position, tempPositionFinal, 0.4f); 
            }
            else
            {
                firstOrb.transform.position = tempPositionFirst;
                finalOrb.transform.position = tempPositionFinal;
                board.allOrbs[finalOrbX - board.startX, finalOrbY - board.startY] = firstOrb;
                board.allOrbs[firstOrbX - board.startX, firstOrbY - board.startY] = finalOrb;
                NewMove();
            }
        }
        if(orbMove == OrbMove.Left || orbMove == OrbMove.Right)
        {
            tempPositionFirst = new Vector2(finalOrbX, firstOrbY);
            tempPositionFinal = new Vector2(firstOrbX, finalOrbY);
            if(Mathf.Abs(firstOrb.transform.position.x - finalOrbX) > 0.1f)
            {
                firstOrb.transform.position = Vector2.Lerp(firstOrb.transform.position, tempPositionFirst, 0.4f);
                finalOrb.transform.position = Vector2.Lerp(finalOrb.transform.position, tempPositionFinal, 0.4f); 
            }
            else
            {
                firstOrb.transform.position = tempPositionFirst;
                finalOrb.transform.position = tempPositionFinal;
                board.allOrbs[finalOrbX - board.startX, finalOrbY - board.startY] = firstOrb;
                board.allOrbs[firstOrbX - board.startX, firstOrbY - board.startY] = finalOrb;
                NewMove();
            }
        }
    }

    public void OrbClicked(Vector2 orbPosition)
    {
        if(firstClick)
        {
            firstOrbX = (int)orbPosition.x;
            firstOrbY = (int)orbPosition.y; 
            firstOrb = board.allOrbs[firstOrbX - board.startX, firstOrbY - board.startY];
        }
        else
        {
            finalOrbX = (int)orbPosition.x;
            finalOrbY = (int)orbPosition.y;
            finalOrb = board.allOrbs[finalOrbX - board.startX, finalOrbY - board.startY];
            MoveOrbs();
        }
        firstClick = !firstClick;
    }

    private void MoveOrbs()
    {
        if(firstOrbX == finalOrbX && firstOrbY < finalOrbY)
        {
            // UP SWIPE
            orbMove = OrbMove.Up;
        }
        else if(firstOrbX == finalOrbX && firstOrbY > finalOrbY)
        {
            // DOWN SWIPE
            orbMove = OrbMove.Down;
        }
        else if(firstOrbX < finalOrbX && firstOrbY == finalOrbY)
        {
            // RIGHT SWIPE
            orbMove = OrbMove.Right;
        }
        else if(firstOrbX > finalOrbX && firstOrbY == finalOrbY)
        {
            // LEFT SWIPE
            orbMove = OrbMove.Left;
        }
        else
        {
            Debug.Log("Invalid Swipe");
        }
    }

    private void NewMove()
    {
        orbMove = OrbMove.None;
        firstClick = true;
        firstOrb = null;
        finalOrb = null;
        firstOrbX = 0;
        firstOrbY = 0; 
        finalOrbX = 0;
        finalOrbY = 0;
    }
}
