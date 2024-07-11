using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private int num;
    private string name;
    private char curPiece;

    private char pieceType;
    private char pieceColor;

    private bool isLegalMove;

    public Tile(int num, string name)
    {
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
        pieceType = Char.ToLower(curPiece);
        pieceColor = Char.IsUpper(curPiece) ? 'w' : 'b';
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

    public void setLegalMove(bool isLegalMove)
    {
        this.isLegalMove = isLegalMove;
    }

    public bool getLegalMove()
    {
        return isLegalMove;
    }

    public char getPieceType()
    {
        return pieceType;
    }

    public char getPieceColor()
    {
        return pieceColor;
    }
}
