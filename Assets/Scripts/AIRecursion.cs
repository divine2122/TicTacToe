using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Text;

public class AIRecursion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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



}
