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
        Fen pos2 = new Fen("rn2k2r/pb1qbppp/1p2pn2/3p4/2B2B2/2N1PN2/PPP2PPP/R2Q1RK1 w kq - 0 1");
        Fen pos3 = new Fen("rn2k2r/pb1qbppp/1p2pn2/3p4/2B2B2/2N1PN2/PPP2PPP/R2Q1RK1 w kq - 0 1");

        //Fen pos2 = new Fen("");

        double time = Time.realtimeSinceStartup;
        Debug.Log(ChessBot.combinedScoring(pos1));
        Debug.Log("evaluation took " + (Time.realtimeSinceStartup - time).ToString());

        time = Time.realtimeSinceStartup;
        Debug.Log(ChessBot.combinedScoring(pos2));
        Debug.Log("evaluation took " + (Time.realtimeSinceStartup - time).ToString());

        time = Time.realtimeSinceStartup;
        Debug.Log(ChessBot.combinedScoring(pos3));
        Debug.Log("evaluation took " + (Time.realtimeSinceStartup - time).ToString());
    }

    void update()
    {

    }
}