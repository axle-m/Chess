using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class Testing : MonoBehaviour
{
    void Start()
    {
        Debug.Log(MoveHasher.ChessMoveToHash("exd4"));
        Debug.Log(MoveHasher.ChessMoveToHash("Nexf4"));
        Debug.Log(MoveHasher.ChessMoveToHash("e4"));
        Debug.Log(MoveHasher.ChessMoveToHash("e5"));
        Debug.Log(MoveHasher.ChessMoveToHash("Nf3"));
        Debug.Log(MoveHasher.ChessMoveToHash("Nc6"));
        Debug.Log(MoveHasher.ChessMoveToHash("Bb5"));
        Debug.Log(MoveHasher.ChessMoveToHash("a6"));
        Debug.Log(MoveHasher.ChessMoveToHash("Ba4"));
        Debug.Log(MoveHasher.ChessMoveToHash("Nf6"));
        Debug.Log(MoveHasher.ChessMoveToHash("O-O"));
        Debug.Log(MoveHasher.ChessMoveToHash("O-O-O"));
        Debug.Log(MoveHasher.ChessMoveToHash("Qb7"));
    }

    void update()
    {

    }
}