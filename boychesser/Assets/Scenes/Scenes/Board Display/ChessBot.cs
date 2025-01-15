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

    private static int depth = 3; // start at a low depth for now

    void Start()
    {

    }

    void Update()
    {
    }

    public static string playBestMove(String fenString) {
        String move = GetBestMove(fenString);
        Debug.Log($"Bot move: {move}");   
        return move;  
    }

    public static string[] recursiveMoves(Fen f, int depth)
    {
        List<string[]> moves = new List<string[]>(); // list of moves along with eval
        foreach (string move in LegalMovesList.getLegalMoves(f))
        {
            if (depth != 0) 
            {

                Fen newFen = new Fen(Fen.move(f.ToString(), move));
                recursiveMoves(newFen, depth - 1);
            }
            else
            {
                moves.Add(new string[] { move, combinedScoring(f).ToString() });
            }
        }

        //find best position out of provided
        double bestEval = -4096.0;
        int indexOfBest = 0;

        for(int i = 0; i < moves.Count; i++)
        {
            if (Convert.ToDouble(moves[i][1]) > bestEval)
            {
                bestEval = Convert.ToDouble(moves[i][1]);
            }
        }

        return moves[indexOfBest];
    }

    public static string GetBestMove(String fenString)
    {
       
        Fen f = new Fen(fenString);
        string move = recursiveMoves(f, depth)[0];

        return move;
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