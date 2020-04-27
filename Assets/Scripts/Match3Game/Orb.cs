using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public int col, row;
    private int targetX, targetY;
    public bool isMatched = false;
    private Board board;
    private GameObject otherOrb;
    private Vector2 firstTouchPos, finalTouchPos;
    private Vector2 tempPosition;
    private float swipeAngle = 0.0f;
    private float swipeResist = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        board = GameObject.Find("BoardManager").GetComponent<Board>();
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        col = targetX;
        row = targetY;
    }

    // Update is called once per frame
    void Update()
    {
        FindMatches();
        if(isMatched)
        {
            SpriteRenderer orbSprite = GetComponent<SpriteRenderer>();
            orbSprite.color = new Color(0.0f, 0.0f, 0.0f, 0.2f);
        }

        targetX = col;
        targetY = row;
        // Horizontal Movement
        if(Mathf.Abs(targetX - transform.position.x) > 0.1f)
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.4f);
            if(board.allOrbs[col, row] != this.gameObject)
            {
                board.allOrbs[col, row] = this.gameObject;
            }
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
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }
    }

    private void OnMouseDown()
    {
        firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        finalTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    void CalculateAngle()
    {
        if(Mathf.Abs(finalTouchPos.y - firstTouchPos.y) > swipeResist || Mathf.Abs(finalTouchPos.x - firstTouchPos.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(finalTouchPos.y - firstTouchPos.y, finalTouchPos.x - firstTouchPos.x) * 180 / Mathf.PI;
            MoveOrbs();
        }
    }

    void MoveOrbs()
    {
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
        yield return new WaitForSeconds(0.5f);
        if(otherOrb != null)
        {
            board.DestroyMatches();
            otherOrb = null;
        }
    }
}
