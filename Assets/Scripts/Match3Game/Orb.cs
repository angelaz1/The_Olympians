using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public int col, row;
    public int targetX, targetY;
    private Board board;
    private GameObject otherOrb;
    private Vector2 firstTouchPos, finalTouchPos;
    private Vector2 tempPosition;
    public float swipeAngle = 0;

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
        targetX = col;
        targetY = row;
        // Horizontal Movement
        if(Mathf.Abs(targetX - transform.position.x) > 0.1f)
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.4f);
        }
        else
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allOrbs[col, row] = this.gameObject;
        }

        // Vertical Movement
        if(Mathf.Abs(targetY - transform.position.y) > 0.1f)
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.4f);
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allOrbs[col, row] = this.gameObject;
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
        swipeAngle = Mathf.Atan2(finalTouchPos.y - firstTouchPos.y, finalTouchPos.x - firstTouchPos.x) * 180 / Mathf.PI;
        MoveOrbs();
    }

    void MoveOrbs()
    {
        if(swipeAngle > -45 && swipeAngle <= 45 && col < board.xSize)
        {
            // Right Swipe
            otherOrb = board.allOrbs[col + 1, row];
            otherOrb.GetComponent<Orb>().col -= 1;
            col += 1;
        }
        else if(swipeAngle > 45 && swipeAngle <= 135 && row < board.ySize)
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
        else if((swipeAngle > -45 || swipeAngle <= 45) && row > 0)
        {
            // Down Swipe
            otherOrb = board.allOrbs[col, row - 1];
            otherOrb.GetComponent<Orb>().row += 1;
            row -= 1;
        }
    }
}
