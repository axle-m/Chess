using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Tile
{

    static readonly Dictionary<int, char> file_codes = new Dictionary<int, char>
    {
          { 0, 'a' }
        , { 1, 'b' }
        , { 2, 'c' }
        , { 3, 'd' }
        , { 4, 'e' }
        , { 5, 'f' }
        , { 6, 'g' }
        , { 7, 'h' }
    };
    
    public readonly int index;

    private int num;
    private string name;
    private char curPiece;

    private char pieceType;
    private char pieceColor;

    private bool isLegalMove;

    public Tile(int num, string name, int index)
    {

        this.index = index;
        this.num = num;
        this.name = name;

        changeCurPiece(name[0]);

        isLegalMove = false;
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

    public void setLegalMove(bool b)
    {
        isLegalMove = b;
    }

    public bool getLegalMove()
    {
        return isLegalMove;
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
        return indexToName();
    }

    public char getPieceType()
    {
        return pieceType;
    }

    public char getPieceColor()
    {
        return pieceColor;
    }

    public string indexToName()
    {
        return file_codes[index % 8] + (index / 8 + 1).ToString();
    }

    public string toString()
    {
        return $"{num} {name} {curPiece} {pieceType} {pieceColor}";
    }
}

