using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Text;

public enum TicTacToeState { none, cross, circle }

[System.Serializable]
public class WinnerEvent : UnityEvent<int>
{


}
public class TicTacToeAI : MonoBehaviour
{

    int _aiLevel;

    TicTacToeState[,] boardState;

    [SerializeField]
    private bool _isPlayerTurn;

    [SerializeField]
    private int _gridSize = 3;

    [SerializeField]
    private TicTacToeState playerState = TicTacToeState.cross;
    TicTacToeState aiState = TicTacToeState.circle;

    [SerializeField]
    private GameObject _xPrefab;

    [SerializeField]
    private GameObject _oPrefab;

    public UnityEvent onGameStarted;

    //Call This event with the player number to denote the winner
    public WinnerEvent onPlayerWin;

    ClickTrigger[,] _triggers;

    //public int tempBestRow;
    //public int tempBestCol;
    public int tempRow;
    public int tempCol;

    public bool aiChoosing;

    private void Start()
    {
        StartAI(1);//added this here

    }
    private void Awake()
    {
        if (onPlayerWin == null)
        {
            onPlayerWin = new WinnerEvent();
        }
    }

    public void StartAI(int AILevel)
    {
        //Debug.Log("startai ran");
        _aiLevel = AILevel;
        StartGame();
    }

    public void RegisterTransform(int myCoordX, int myCoordY, ClickTrigger clickTrigger)
    {
        //Debug.Log("register transform ran"+myCoordX+myCoordY);
        //Debug.Log("registeringtransform"+myCoordX+" "+myCoordY);
        _triggers[myCoordX, myCoordY] = clickTrigger;
    }

    private void StartGame()
    { //Debug.Log("startgame ran");
        _triggers = new ClickTrigger[_gridSize, _gridSize];
        boardState = new TicTacToeState[_gridSize, _gridSize];
        //Debug.Log("testtt" + _triggers);
        onGameStarted.Invoke();
        Debug.Log("game has started");
    }

    public void PlayerSelects(int coordX, int coordY)
    {
        if (aiChoosing == false)
        {
            //Debug.Log("playerselect ran" + coordX + coordY+"    " + playerState);
            SetVisual(coordX, coordY, playerState);
            boardState[coordX, coordY] = playerState;
            Invoke("aiChooserLogic", 1); //Pause till ai takes turn. solely here to give feeling of playing against a person
            aiChoosing = true;

            Invoke("aiChoosingSetter", 1);//Time delay until player can make another move

            //Debug.Log(boardState);
            logArray();
            //Debug.Log(findBestMove(boardState));
            winnerCheck(boardState);
        }
    }

    public void aiChoosingSetter()
    {
        aiChoosing = false;
    }

    void logArray()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < boardState.GetLength(1); i++)
        {
            for (int j = 0; j < boardState.GetLength(0); j++)
            {
                sb.Append(boardState[i, j]);
                sb.Append(' ');
                sb.Append(i);
                sb.Append(j);
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());
    }

    public void aiChooserLogic()
    {
        tempRow= AIRecursion.findBestMove(boardState).row;
        tempCol= AIRecursion.findBestMove(boardState).col;
        //Debug.Log("row"+tempRow+" col: "+ tempCol);
        //Debug.Log("col"+tempCol);
        
        AiSelects(tempRow, tempCol);
        logArray();

    }

    public void AiSelects(int coordX, int coordY)
    {
        //Debug.Log("aiselects ran" + coordX + coordY + aiState);
        boardState[coordX, coordY] = aiState;

        SetVisual(coordX, coordY, aiState);
        winnerCheck(boardState);
    }

    private void SetVisual(int coordX, int coordY, TicTacToeState targetState)
    {

        //Debug.Log("setvisual rna" + _triggers[coordX, coordY]);
        _triggers[coordX,coordY].SetInputEndabled(false);
        Instantiate(
            targetState == TicTacToeState.circle ? _oPrefab : _xPrefab,
            _triggers[coordX, coordY].transform.position,
            Quaternion.identity
        );
    }



    public int winnerCheck(TicTacToeState[,] b)
    {

        if (AIRecursion.evaluate(b) == 10)
        {
            onPlayerWin.Invoke(1);
            return 1; //represents ai/circle
        }
        else if (AIRecursion.evaluate(b) == -10)
        {
            onPlayerWin.Invoke(2);
            return 2; //represents human/player/cross
        }
        else if (AIRecursion.isMovesLeft(b) == false)
        {
            onPlayerWin.Invoke(-1);
            return -1; //represents draw/no winner
        }

        else {
            return 0;
        }//represents ongoing game
    }

}



