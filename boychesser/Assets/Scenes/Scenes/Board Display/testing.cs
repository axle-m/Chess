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

        Fen pos1 = new Fen("8/8/8/3k4/3n1K2/8/2R1Q3/8 b - - 0 1");
        Debug.Log(pos1);
        Fen pos2 = new Fen("8/8/8/3k1K2/BR6/8/8/8 b - - 0 1");
        Fen pos3 = new Fen("8/8/5n2/3k1K2/6r1/8/8/3Q4 w - - 0 1");
        Fen pos4 = new Fen("4k3/8/8/4q3/2N5/8/8/3K4 w - - 0 1");

        double time = Time.realtimeSinceStartup;
        Debug.Log(ChessBot.combinedScoring(pos1));
        Debug.Log(ChessBot.GetBestMove(pos1.ToString()));
        Debug.Log("evaluation took " + (Time.realtimeSinceStartup - time).ToString());

        time = Time.realtimeSinceStartup;
        Debug.Log(ChessBot.combinedScoring(pos2));
        Debug.Log(ChessBot.GetBestMove(pos2.ToString()));
        Debug.Log("evaluation took " + (Time.realtimeSinceStartup - time).ToString());

        time = Time.realtimeSinceStartup;
        Debug.Log(ChessBot.combinedScoring(pos3));
        Debug.Log(ChessBot.GetBestMove(pos3.ToString()));
        Debug.Log("evaluation took " + (Time.realtimeSinceStartup - time).ToString());

        time = Time.realtimeSinceStartup;
        Debug.Log(ChessBot.combinedScoring(pos4));
        Debug.Log(ChessBot.GetBestMove(pos4.ToString()));
        Debug.Log("evaluation took " + (Time.realtimeSinceStartup - time).ToString());

    }

    void update()
    {

    }
}