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

    }

    void Update()
    {
    }

    // Selects a random legal move and logs it
    void randomMove(LegalMovesList legalMovesList){
        String[] currentLegalMoves = (string[])legalMovesList.getLegalMoves(board.curFen);
        System.Random rnd = new System.Random();
        string move = currentLegalMoves[rnd.Next(currentLegalMoves.Length)];
        Debug.Log(move);
    }

    //I think this code can be ignored, sorry Max
    // void moveAttempt(int scorer, bool isWhite){
    //     String[] currentLegalMoves = (string[])legalMovesList.getLegalMoves(board.curFen);
    //     string bestMove = null;

    //     if (isWhite){
    //         for (int i = 0; i < currentLegalMoves.Length; i++){
    //             string move = currentLegalMoves[i];
    //             if (bestMove == null || getCurrentScoring(move) > getCurrentScoring(bestMove)){
    //                 bestMove = move;
    //             }
    //         }
    //     }
    //     else{ // if bot is black
    //         for (int i = 0; i < currentLegalMoves.Length; i++){
    //             string move = currentLegalMoves[i];
    //             if (bestMove == null || getCurrentScoring(move) < getCurrentScoring(bestMove)){
    //                 bestMove = move;
    //             }
    //         }
    //     }
    //     Debug.Log($"Best move: {bestMove}");
    // }

    string bestPositionMove(LegalMovesList legalMovesList){
        //Make a 2d array
        //Inside each array, first index is the move, the second index is the value
        //Store them as strings, String.GetNumbericalValue to get the second value

        //List<string[]> ints = new List<string[]>();
        //int val = int.Parse(ints[index][1]);
        double highestValue = 0;
        int index = 0;
        string[] currentLegalMoves = (string[])legalMovesList.getLegalMoves(board.curFen);
        List<string[]> movesAndValueList = new List<string[]>();
        string fenString = board.curFen.ToString();
        for(int i = 0; i < currentLegalMoves.Length; i++){
            Fen move = new Fen(Fen.move(fenString, currentLegalMoves[i]));
            double moveValue = getPositionScoring(move);
            if(moveValue > highestValue){
                highestValue = moveValue;
                index = i;
            }
            string[] movesAndValueArray = {currentLegalMoves[i], moveValue.ToString()};
            movesAndValueList.Add(movesAndValueArray);
        }
        return currentLegalMoves[index];

        //Goes through all legal moves, Applies scorer, stores move, returns move with highest value


        //This returns the best possible move based on position however more can be done
        //We can use the 2d array (movesAndValueList) choose between the best 1-3 or 1-5 
        //It should still favor the best move  the probability could be something like ⌄⌄⌄
        //(1st - 70%, 2nd - 15%, 3rd - 5%, 4th - 5%, 5th - 5%)?
        //We could keep track of those indicies or sort the arrays to figure this out

        //Question: Should this return a string (move played) or fen (curFen + move played)?
    }

    string bestPieceMove(LegalMovesList legalMovesList){
        double highestValue = 0;
        int index = 0;
        string[] currentLegalMoves = (string[])legalMovesList.getLegalMoves(board.curFen);
        List<string[]> movesAndValueList = new List<string[]>();
        string fenString = board.curFen.ToString();
        for(int i = 0; i < currentLegalMoves.Length; i++){
            Fen move = new Fen(Fen.move(fenString, currentLegalMoves[i]));
            double moveValue = getPieceScoring(move);
            if(moveValue > highestValue){
                highestValue = moveValue;
                index = i;
            }
            string[] movesAndValueArray = {currentLegalMoves[i], moveValue.ToString()};
            movesAndValueList.Add(movesAndValueArray);
        }
        return currentLegalMoves[index];
        //This is a copy-paste of the position mover just different scorer used
    }


    public double getPositionScoring(Fen fen){
        return Scorer.getPositionScore(fen); 
    }

    public double getPieceScoring(Fen fen){
        return Scorer.getPieceScore(fen); 
    }
}