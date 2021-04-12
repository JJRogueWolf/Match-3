using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fruits : MonoBehaviour
{
    public string fruitName;

    private Board board;

    private int column;
    private int row;
    private int previousColumn;
    private int previousRow;
    private int targetX;
    private int targetY;

    public bool isMatched = false;

    private Vector2 FirstPosition;
    private Vector2 FinalPosition;
    private float swipeAngle = 0;
    private float swipeResist = 0.02f;

    private GameObject tragetedGameobject;
    private Vector2 tempPosition;

    // Start is called before the first frame update
    void Start()
    {
        board = GameObject.Find("Board").GetComponent<Board>();
        Debug.Log(board);
        targetX = (int) transform.position.x;
        targetY = (int) transform.position.y;
        column = targetX;
        row = targetY;
        previousColumn = column;
        previousRow = row;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMatches();
        if(isMatched){
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.color = new Color(1f,1f,1f,0.5f);
        }

        targetX = column;
        targetY = row;
        if(Mathf.Abs(targetX - transform.position.x) > .1){
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.1f);
        } else {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.fruits[column, row] = this.gameObject;
        }
        if(Mathf.Abs(targetY - transform.position.y) > .1){
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.1f);
        } else {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.fruits[column, row] = this.gameObject;
        }

    }

    private void OnMouseDown() {
        FirstPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp() {
        FinalPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        calculateAngle();
    }

    private void calculateAngle(){
        if(!isMatched){
            if(Mathf.Abs(FinalPosition.x - FirstPosition.x) > swipeResist && Mathf.Abs(FinalPosition.y - FirstPosition.y) > swipeResist){
                swipeAngle = Mathf.Atan2(FinalPosition.y - FirstPosition.y, FinalPosition.x - FirstPosition.x) * 180 / Mathf.PI;
                MoveFruit();
            }
        }
    }

    private void MoveFruit(){
        if((swipeAngle > 45 && swipeAngle <= 135) && row < board.height - 1){
            //Move Up
            tragetedGameobject = board.fruits[column, row + 1];
            if(!tragetedGameobject.GetComponent<fruits>().isMatched){
                tragetedGameobject.GetComponent<fruits>().row -= 1;
                row += 1;
            }
        } else if((swipeAngle > -45 && swipeAngle <= 45) && column < board.width - 1){
            //Move Right
            tragetedGameobject = board.fruits[column + 1, row];
            if(!tragetedGameobject.GetComponent<fruits>().isMatched){
                tragetedGameobject.GetComponent<fruits>().column -= 1;
                column += 1;
            }
        } else if((swipeAngle > 135 || swipeAngle <= -135) && column > 0){
            //Move Left
            tragetedGameobject = board.fruits[column - 1, row];
            if(!tragetedGameobject.GetComponent<fruits>().isMatched){
                tragetedGameobject.GetComponent<fruits>().column += 1;
                column -= 1;
            }
        } else if((swipeAngle > -135 && swipeAngle <= -45) && row > 0){
            //Move Down
            tragetedGameobject = board.fruits[column, row - 1];
            if(!tragetedGameobject.GetComponent<fruits>().isMatched){
                tragetedGameobject.GetComponent<fruits>().row += 1;
                row -= 1;
            }
        }
        StartCoroutine(IsMatched());
    }

    IEnumerator IsMatched(){
        yield return new WaitForSeconds(0.2f);
        if(tragetedGameobject != null){
            if(!tragetedGameobject.GetComponent<fruits>().isMatched && !isMatched){
                tragetedGameobject.GetComponent<fruits>().row = row;
                tragetedGameobject.GetComponent<fruits>().column = column;
                tragetedGameobject.GetComponent<fruits>().previousRow = row;
                tragetedGameobject.GetComponent<fruits>().previousColumn = column;
                row = previousRow;
                column = previousColumn;
            }
            tragetedGameobject = null;
        }
    }

    private void CheckMatches(){
        if (column > 0 && column < board.width - 1){
            fruits leftFruit = board.fruits[column - 1, row].GetComponent<fruits>();
            fruits rightFruit = board.fruits[column + 1, row].GetComponent<fruits>();
            if (leftFruit.fruitName == this.fruitName && rightFruit.fruitName == this.fruitName){
                leftFruit.isMatched = true;
                rightFruit.isMatched = true;
                this.isMatched = true;
            }
        }
        if (row > 0 && row < board.height - 1){
            fruits downFruit = board.fruits[column, row - 1].GetComponent<fruits>();
            fruits upFruit = board.fruits[column, row + 1].GetComponent<fruits>();
            if (downFruit.fruitName == this.fruitName && upFruit.fruitName == this.fruitName){
                downFruit.isMatched = true;
                upFruit.isMatched = true;
                this.isMatched = true;
            }
        }
    }
}
