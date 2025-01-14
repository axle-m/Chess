using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChessBot
{
    
    /* a couple of things:
     * methods in this class should be static
     * the bot takes in a board and an array of legal moves
     * there should be a method bestMove(String[] moves, String board) that returns the best move
     * 
     * Also you're currently passing in a LegalMovesList object, and then using it to create an array of legal moves. You should be using a string array. Refer to randomMove for how it should be implemented
     * although i did modify the other method signatures to take in a string array instead of a LegalMovesList object and a fen string to describe a board
     */

    void Start()
    {

    }

    void Update()
    {
    }

    // Selects a random legal move and logs it
    public static string playBestMove(String[] moves, String fenString) {
        String move = GetBestMove(moves, fenString);
        Debug.Log($"Bot move: {move}");   
        return move;  
    }

    public static string GetBestMove(String[] currentLegalMoves, String fenString)
    {
        double highestValue = -1000.0;
        int index = -1;

        // Create the combined scoring function by using both piece and position scoring
        for (int i = 0; i < currentLegalMoves.Length; i++)
        {
            Fen moveFen = new Fen(Fen.move(fenString, currentLegalMoves[i]));
            double score = combinedScoring(moveFen);  // Directly use the combinedScoring

            if (score > highestValue)
            {
                highestValue = score;
                index = i;
            }
        }

        return index >= 0 ? currentLegalMoves[index] : null;
    }

    public static double combinedScoring(Fen fen) {
        double materialScore = getPieceScoring(fen);
        double positionScore = getPositionScoring(fen);
        return materialScore + positionScore; 
    }
    public static double getPositionScoring(Fen fen) {
        return Scorer.getPositionScore(fen);
    }

    public static double getPieceScoring(Fen fen) {
        return Scorer.getPieceScore(fen);
    }
}