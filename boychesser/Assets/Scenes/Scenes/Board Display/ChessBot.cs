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
    void randomMove(){
        String[] currentLegalMoves = (string[])legalMovesList.getLegalMoves(board.curFen);
        System.Random rnd = new System.Random();
        string move = currentLegalMoves[rnd.Next(currentLegalMoves.Length)];
        Debug.Log(move);
    }

    void moveAttempt(int scorer, bool isWhite){
        String[] currentLegalMoves = (string[])legalMovesList.getLegalMoves(board.curFen);
        string bestMove = null;

        if (isWhite){
            for (int i = 0; i < currentLegalMoves.Length; i++){
                string move = currentLegalMoves[i];
                if (bestMove == null || getCurrentScoring(move) > getCurrentScoring(bestMove)){
                    bestMove = move;
                }
            }
        }
        else{ // if bot is black
            for (int i = 0; i < currentLegalMoves.Length; i++){
                string move = currentLegalMoves[i];
                if (bestMove == null || getCurrentScoring(move) < getCurrentScoring(bestMove)){
                    bestMove = move;
                }
            }
        }
        Debug.Log($"Best move: {bestMove}");
    }

    void positionMoveAttempt(LegalMovesList legalMovesList){
        //Make a 2d array
        // In the inner array, first index is the move, the second index is the value
        //Store them as strings, String.GetNumbericalValue

        //List<string[]> ints = new List<string[]>();
        //int val = int.Parse(ints[index][1]);
        string[] currentLegalMoves = (string[])legalMovesList.getLegalMoves(board.curFen);
        List<string[]> movesAndValueList = new List<string[]>();
        string fenString = board.curFen.ToString();
        for(int i = 0; i < currentLegalMoves.Length; i++){
            //scoring = fen object
            //move = fen string
            string[] movesAndValueArray = {currentLegalMoves[i], getCurrentScoring(Fen.move(fenString, currentLegalMoves[i]))};
            movesAndValueList.Add(movesAndValueArray);
        }
    }

    // Gets the scoring for a specific board position
    public double getCurrentScoring(Fen fen){
        return Scorer.getPositionScore(fen); 
    }
}