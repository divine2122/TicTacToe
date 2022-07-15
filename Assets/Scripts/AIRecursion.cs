using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Text;

public class AIRecursion : MonoBehaviour
{
    public class Move
    {
        public int row, col;
    };


    public static bool isMovesLeft(TicTacToeState[,] board)
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

        return 0;
    }


    public static int minimax(TicTacToeState[,] board,
                       int depth, bool isMax)
    {
        int score = evaluate(board);

        // If Maximizer has won the game
        if (score == 10)
            return score;

        // If Minimizer has won the game
        if (score == -10)
            return score;

        if (isMovesLeft(board) == false)
            return 0;

        if (isMax)
        {
            int best = -1000;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Check if cell is empty
                    if (board[i, j] == TicTacToeState.none)
                    {
                        // Make move
                        board[i, j] = TicTacToeState.circle; //SWITCHED

                        // Call finds max val recursively
                        best = Math.Max(best, minimax(board,
                                        depth + 1, !isMax));

                        // Undo move
                        board[i, j] = TicTacToeState.none;
                    }
                }
            }
            return best;
        }

        //if not maximizer
        else
        {
            int best = 1000;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == TicTacToeState.none)
                    {
                        board[i, j] = TicTacToeState.cross; //SWITCH
                        best = Math.Min(best, minimax(board,
                                        depth + 1, !isMax));
                        // Undo move
                        board[i, j] = TicTacToeState.none;
                    }
                }
            }
            return best;
        }
    }

    // Returns the best possible ai move
    public static Move findBestMove(TicTacToeState[,] board)
    {
        int bestVal = -1000;
        Move bestMove = new Move();
        bestMove.row = -1;
        bestMove.col = -1;

        // Traverse all cells and returns the best position
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == TicTacToeState.none)
                {
                    // Makes move
                    board[i, j] = TicTacToeState.circle; //SWITCHED

                    int moveVal = minimax(board, 0, false);

                    // Undo the move
                    board[i, j] = TicTacToeState.none;

                    // If current move is higher then update
                    if (moveVal > bestVal)
                    {
                        bestMove.row = i;
                        bestMove.col = j;
                        bestVal = moveVal;
                    }
                }
            }
        }
        return bestMove;
    }
}
