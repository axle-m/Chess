using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Tile
{

    public readonly int index;

    private int num;
    private string name;
    private char curPiece;

    private char pieceType;
    private char pieceColor;

    public Tile(int num, string name, int index)
    {

        this.index = index;
        this.num = num;
        this.name = name;
        curPiece = '0';
        pieceType = '0';


    }

    public bool hasPiece()
    {
        return !Char.IsDigit(curPiece);
    }

    public void changeCurPiece(char newPiece)
    {
        curPiece = newPiece;
        if (curPiece == '0')
        {
            pieceType = '0';
            pieceColor = '0';
        }
        else
        {
            pieceType = Char.ToLower(curPiece);
            pieceColor = Char.IsUpper(curPiece) ? 'w' : 'b';
        }
    }

    public char getCurPiece()
    {
        return curPiece;
    }

    public int getNum()
    {
        return num;
    }
    public string getName()
    {
        return name;
    }

    public char getPieceType()
    {
        return pieceType;
    }

    public char getPieceColor()
    {
        return pieceColor;
    }

    public int nameToIndex(string s)
    {

        int file = s[0] - 'a';
        int rank = s[1] - '1';

        return rank * 8 + file;
    }
}
