using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChessBot : MonoBehaviour
{
    public Board board;
    public LegalMovesList legalMovesList;

    void Start()
    {
        randomMove();
    }

    void Update()
    {
    }

    // Selects a random legal move and logs it
    void randomMove()
    {
        String[] currentLegalMoves = (string[])legalMovesList.getLegalMoves(board.curFen);
        System.Random rnd = new System.Random();
        string move = currentLegalMoves[rnd.Next(currentLegalMoves.Length)];
        Debug.Log(move);
    }

    void moveAttempt(int scorer, bool isWhite)
    {
        String[] currentLegalMoves = (string[])legalMovesList.getLegalMoves(board.curFen);
        string bestMove = null;

        if (isWhite)
        {
            for (int i = 0; i < currentLegalMoves.Length; i++)
            {
                string move = currentLegalMoves[i];
                if (bestMove == null || getCurrentScoring(move) > getCurrentScoring(bestMove))
                {
                    bestMove = move;
                }
            }
        }
        else // if bot is black
        {
            for (int i = 0; i < currentLegalMoves.Length; i++)
            {
                string move = currentLegalMoves[i];
                if (bestMove == null || getCurrentScoring(move) < getCurrentScoring(bestMove))
                {
                    bestMove = move;
                }
            }
        }

        Debug.Log($"Best move: {bestMove}");
    }

    // Gets the scoring for a specific board position
    public int getCurrentScoring(string boardPosition)
    {
        return Scorer.getPositionScore(boardPosition); 
    }
}
