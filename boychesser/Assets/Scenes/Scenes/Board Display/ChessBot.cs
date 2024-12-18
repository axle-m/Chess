using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChessBot
{
    public Board board;
    public LegalMovesList legalMovesList;

    // Start is called before the first frame update
    void Start()
    {
        randomMove();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void randomMove(){ 
        String[] currentLegalMoves = (string[])legalMovesList.getLegalMoves(board.curFen);
        System.Random rnd = new System.Random();
        String move = currentLegalMoves[rnd.Next(currentLegalMoves.Length)];
        Debug.Log(move);
    }
    void moveAttempt(int scorer, bool isWhite){
        String[] currentLegalMoves = (string[])legalMovesList.getLegalMoves(board.curFen); //this is the line above
        // list<string> currentLegalMoves = legalMovesList.getLegalMoves(board.curFen);    do this if line above does NOT work
        string bestMove = null;
        string move = null;
        if(isWhite){ //if bot is white
            for(int i = 0; i < currentLegalMoves.Length-1; i++){
            move = currentLegalMoves[i];
                if (string.IsNullOrEmpty(bestMove)){
                    move = bestMove; //im lazy, read the comments for black
                }
                else if(move.getCurrentScoring() > bestMove.getCurrentScoring()){
                move = bestMove;
                }
            }
        }
        else{ //if bot is black
         for(int i = 1; i < legalMovesList; i++){
            move = legalMovesList[i];  
            if(bestMove.Equals('')){    //checks if this is first move being analyzed
                    move = bestMove;
            }
            else if(move.getCurrentScoring < bestMove.getCurrentScoring){//checks if the score position is the best for black
                move = bestMove;
            }
        }
        }
    }
    public int getCurrentScoring(){ 
        return Scorer.getPositionScore(); //hopefully gives us position of board, gabe and kai need to do this
    }

}
