using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChessBot : MonoBehaviour
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
    void moveAttempt2(int scorer, bool white){
        String[] currentLegalMoves = (string[])legalMovesList.getLegalMoves(board.curFen); //this is the line above
        // list<string> currentLegalMoves = legalMovesList.getLegalMoves(board.curFen);    do this if line above does NOT work
        string bestMove = '';
        string move1 = '';
        if(white){ //if bot is white
            for(int i = 0; i < legalMovesList-1; i++){
        move1 = legalMovesList[i];
            if(bestMove.equals('')){
                move1 = bestMove //im lazy, read the comments for black
            }
            else if(move1.getCurrentScoring > bestMove.getCurrentScoring){
                move1 = bestMove;
            }
        }
        }
        if(!white){ //if bot is black
         for(int i = 1; i < legalMovesList; i++){
            move1 = legalMovesList[i];  
            if(bestMove.equals('')){    //checks if this is first move being analyzed
                move1 = bestMove
            }
            else if(move1.getCurrentScoring < bestMove.getCurrentScoring){//checks if the score position is the best for black
                move1 = bestMove;
            }
        }
        }
    }
    public int getCurrentScoring(){ 
        return Scorer.getPositionScore(); //hopefully gives us position of board, gabe and kai need to do this
    }

}
