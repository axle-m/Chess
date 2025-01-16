using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
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

    public static double Negamax(Fen fen, int depth)
    {
        // Base case: terminal state or depth is 0
        if (depth == 0)
        {
            return -combinedScoring(fen);
        }

        double max = Double.NegativeInfinity;

        string[] moves = LegalMovesList.getLegalMoves(fen);
        foreach(string move in moves)
        {
            Fen newBoardPos = new Fen(Fen.move(fen.ToString(), move));
            double score = -Negamax(newBoardPos, depth - 1);
            if (score > max)
            {
                max = score;
            }
        }
        return max;
    }
    
    public static string GetBestMove(String fenString)
    {
        Fen f = new Fen(fenString);
        int active = f.getActiveColor().Equals("w") ? 1 : -1;
        string[] moves = LegalMovesList.getLegalMoves(f);
        List<string[]> evalMoves = new List<string[]>();
        foreach (string move in moves)
        {
            Fen newBoardPos = new Fen(Fen.move(f.ToString(), move));
            double eval = Negamax(newBoardPos, depth);
            evalMoves.Add(new string[] { move, eval.ToString() });
        }

        int bestMoveIndex = 0;
        for(int i = 0; i < evalMoves.Count; i++)
        {
            if (Double.Parse(evalMoves[i][1]) > Double.Parse(evalMoves[bestMoveIndex][1])){
                bestMoveIndex = i;
            }
        }
        double currentPosEval = combinedScoring(f);
        //Debug.Log("Current board state: " + currentPosEval);
        //Debug.Log($"Best move: {moves[bestMoveIndex]} with score {evalMoves[bestMoveIndex][1]}");
        return moves[bestMoveIndex];
    }

    public static double combinedScoring(Fen fen) {
        double materialScore = getPieceScoring(fen);
        //double positionScore = getPositionScoring(fen);
        return materialScore;// + positionScore; 
    }
    public static double getPositionScoring(Fen fen) {
        return Scorer.getPositionScore(fen);
    }

    public static double getPieceScoring(Fen fen) {
        return Scorer.getPieceScore(fen);
    }
}