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

    /////////////////////////
    public class Move
    {
        public int row, col;
    };

    //static char player = 'x', ai = 'o';

    // This function returns true if there are moves
    // remaining on the board. It returns false if
    // there are no moves left to play.
    static Boolean isMovesLeft(TicTacToeState[,] board)
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (board[i, j] == TicTacToeState.none)
                    return true;
        return false;
    }

    public static int evaluate(TicTacToeState[,] b)
    {
        // Checking for Rows for X or O victory.
        for (int row = 0; row < 3; row++)
        {
            if (b[row, 0] == b[row, 1] &&
                b[row, 1] == b[row, 2])
            {
                if (b[row, 0] == TicTacToeState.circle) //SWITCH
                    return +10;
                else if (b[row, 0] == TicTacToeState.cross) //SWITCHED
                    return -10;
            }
        }

        // Checking for Columns for X or O victory.
        for (int col = 0; col < 3; col++)
        {
            if (b[0, col] == b[1, col] &&
                b[1, col] == b[2, col])
            {
                if (b[0, col] == TicTacToeState.circle) //SWITCH
                    return +10;

                else if (b[0, col] == TicTacToeState.cross) //SWITCH
                    return -10;
            }
        }

        // Checking for Diagonals for X or O victory.
        if (b[0, 0] == b[1, 1] && b[1, 1] == b[2, 2])
        {
            if (b[0, 0] == TicTacToeState.circle) //SWITCH
                return +10;
            else if (b[0, 0] == TicTacToeState.cross) //SWITCH
                return -10;
        }

        if (b[0, 2] == b[1, 1] && b[1, 1] == b[2, 0])
        {
            if (b[0, 2] == TicTacToeState.circle) //SWITCH
                return +10;
            else if (b[0, 2] == TicTacToeState.cross) //SWITCH
                return -10;
        }

        // Else if none of them have won then return 0
        return 0;
    }

    // This is the minimax function. It considers all
    // the possible ways the game can go and returns
    // the value of the board
    public static int minimax(TicTacToeState[,] board,
                       int depth, Boolean isMax)
    {
        int score = evaluate(board);

        // If Maximizer has won the game
        // return his/her evaluated score
        if (score == 10)
            return score;

        // If Minimizer has won the game
        // return his/her evaluated score
        if (score == -10)
            return score;

        // If there are no more moves and
        // no winner then it is a tie
        if (isMovesLeft(board) == false)
            return 0;

        // If this maximizer's move
        if (isMax)
        {
            int best = -1000;

            // Traverse all cells
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Check if cell is empty
                    if (board[i, j] == TicTacToeState.none)
                    {
                        // Make the move
                        board[i, j] = TicTacToeState.circle; //SWITCHED

                        // Call minimax recursively and choose
                        // the maximum value
                        best = Math.Max(best, minimax(board,
                                        depth + 1, !isMax));

                        // Undo the move
                        board[i, j] = TicTacToeState.none;
                    }
                }
            }
            return best;
        }

        // If this minimizer's move
        else
        {
            int best = 1000;

            // Traverse all cells
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Check if cell is empty
                    if (board[i, j] == TicTacToeState.none)
                    {
                        // Make the move
                        board[i, j] = TicTacToeState.cross; //SWITCH

                        // Call minimax recursively and choose
                        // the minimum value
                        best = Math.Min(best, minimax(board,
                                        depth + 1, !isMax));

                        // Undo the move
                        board[i, j] = TicTacToeState.none;
                    }
                }
            }
            return best;
        }
    }

    // This will return the best possible
    // move for the player
    public static Move findBestMove(TicTacToeState[,] board)
    {
        int bestVal = -1000;
        Move bestMove = new Move();
        bestMove.row = -1;
        bestMove.col = -1;
        //int[] bestMoveArray;

        // Traverse all cells, evaluate minimax function
        // for all empty cells. And return the cell
        // with optimal value.
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // Check if cell is empty
                if (board[i, j] == TicTacToeState.none)
                {
                    // Make the move
                    board[i, j] = TicTacToeState.circle; //SWITCHED

                    // compute evaluation function for this
                    // move.
                    int moveVal = minimax(board, 0, false);

                    // Undo the move
                    board[i, j] = TicTacToeState.none;

                    // If the value of the current move is
                    // more than the best value, then update
                    // best/
                    if (moveVal > bestVal)
                    {
                        bestMove.row = i;
                        bestMove.col = j;
                        bestVal = moveVal;
                    }
                }
            }
        }
        //bestMoveArray = new int[] { bestMove.row, bestMove.col };
        //TicTacToeAI.tempBestRow = bestMove.row;
        //tempBestCol=
        //Debug.Log(bestMove.col +"  "+ bestMove.row);
        return bestMove;
    }

    /////////////////////////
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
        tempRow=findBestMove(boardState).row;
        tempCol= findBestMove(boardState).col;
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

        if (evaluate(b) == 10)
        {
            onPlayerWin.Invoke(1);
            Debug.Log("winnercheck" + " 1");
            return 1; //represents ai/circle
        }
        else if (evaluate(b) == -10)
        {
            onPlayerWin.Invoke(2);
            Debug.Log("winnercheck" + " 2");

            return 2; //represents human/player/cross
        }
        else if (isMovesLeft(b) == false)
        {
            onPlayerWin.Invoke(-1);
            Debug.Log("winnercheck" + " 0");

            return -1; //represents draw/no winner
        }

        else {
            Debug.Log("winnercheck" + " -1");
            return 0;
        }//represents ongoing game
    }

}



