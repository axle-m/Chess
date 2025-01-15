using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class Testing : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Testing");

        PrecomputeMoveData.precomputedMoveData();
        Debug.Log("precomputed move data");

        Fen pos1 = new Fen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        Debug.Log(pos1);

        //Fen pos2 = new Fen("");

        Debug.Log(ChessBot.playBestMove(pos1.ToString()));
    }

    void update()
    {

    }
}