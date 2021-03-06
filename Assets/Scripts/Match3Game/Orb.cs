﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mister Taft Creates' YouTube Match-3 Tutorial
// https://www.youtube.com/playlist?list=PL4vbr3u7UKWrxEz75MqmTDd899cYAvQ_B

public class Orb : MonoBehaviour
{
    public int col, row;
    private float targetX, targetY;
    public bool isMatched = false;
    private FindMatches findMatches;
    private Board board;
    private GameObject otherOrb;
    private Vector2 firstTouchPos, finalTouchPos;
    private Vector2 tempPosition;
    private float swipeAngle = 0.0f;
    private float swipeResist = 1.0f;
    private float yOffset = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        // targetX = (int)transform.position.x;
        // targetY = (int)transform.position.y;
        // col = targetX;
        // row = targetY;
    }

    // Update is called once per frame
    void Update()
    {
        if(isMatched)
        {
            Animator orbAnimator = GetComponent<Animator>();
            orbAnimator.SetTrigger("Disappear");
            board.swapToHappy();
            board.playSound();
        }

        targetX = col;
        targetY = row + yOffset;
        // Horizontal Movement
        if(Mathf.Abs(targetX - transform.position.x) > 0.1f)
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.4f);
            if(board.allOrbs[col, row] != this.gameObject)
            {
                board.allOrbs[col, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        }
        else
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
        }

        // Vertical Movement
        if(Mathf.Abs(targetY - transform.position.y) > 0.1f)
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.4f);
            if(board.allOrbs[col, row] != this.gameObject)
            {
                board.allOrbs[col, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }
    }

    private void OnMouseDown()
    {
        if(board.boardState == BoardState.stable)
        {
            SpriteRenderer orbSprite = GetComponent<SpriteRenderer>();
            orbSprite.color = new Color(0.8f, 0.8f, 0.8f, 1.0f);
            firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseUp()
    {
        SpriteRenderer orbSprite = GetComponent<SpriteRenderer>();
        orbSprite.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        if(board.boardState == BoardState.stable)
        {
            finalTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    void CalculateAngle()
    {
        if(Mathf.Abs(finalTouchPos.y - firstTouchPos.y) > swipeResist || Mathf.Abs(finalTouchPos.x - firstTouchPos.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(finalTouchPos.y - firstTouchPos.y, finalTouchPos.x - firstTouchPos.x) * 180 / Mathf.PI;
            MoveOrbs();
            board.boardState = BoardState.moving;
        }
        else
        {
            board.boardState = BoardState.stable;
        }
    }

    void MoveOrbs()
    {
        board.AddMove();
        if(swipeAngle > -45 && swipeAngle <= 45 && col < board.xSize - 1)
        {
            // Right Swipe
            otherOrb = board.allOrbs[col + 1, row];
            otherOrb.GetComponent<Orb>().col -= 1;
            col += 1;
        }
        else if(swipeAngle > 45 && swipeAngle <= 135 && row < board.ySize - 1)
        {
            // Up Swipe
            otherOrb = board.allOrbs[col, row + 1];
            otherOrb.GetComponent<Orb>().row -= 1;
            row += 1;
        }
        else if((swipeAngle > 135 || swipeAngle <= -135) && col > 0)
        {
            // Left Swipe
            otherOrb = board.allOrbs[col - 1, row];
            otherOrb.GetComponent<Orb>().col += 1;
            col -= 1;
        }
        else if(swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            // Down Swipe
            otherOrb = board.allOrbs[col, row - 1];
            otherOrb.GetComponent<Orb>().row += 1;
            row -= 1;
        }
        StartCoroutine(ClearMatches());
    }

    void FindMatches()
    {
        if(col > 0 && col < board.xSize - 1)
        {
            GameObject leftOrb1 = board.allOrbs[col - 1, row];
            GameObject rightOrb1 = board.allOrbs[col + 1, row];
            if(leftOrb1 != null && rightOrb1 != null)
            {
                if (leftOrb1.tag == this.gameObject.tag && rightOrb1.tag == this.gameObject.tag)
                {
                    leftOrb1.GetComponent<Orb>().isMatched = true;
                    rightOrb1.GetComponent<Orb>().isMatched = true;
                    isMatched = true;
                }
            }
        }

        if(row > 0 && row < board.ySize - 1)
        {
            GameObject upOrb1 = board.allOrbs[col, row + 1];
            GameObject downOrb1 = board.allOrbs[col, row - 1];
            if(upOrb1 != null && downOrb1 != null)
            {
                if (upOrb1.tag == this.gameObject.tag && downOrb1.tag == this.gameObject.tag)
                {
                    upOrb1.GetComponent<Orb>().isMatched = true;
                    downOrb1.GetComponent<Orb>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }

    public IEnumerator ClearMatches()
    {
        yield return new WaitForSeconds(0.0f);
        
        if(otherOrb != null)
        {
            board.DestroyMatches();
            otherOrb = null;
        }
    }
}
