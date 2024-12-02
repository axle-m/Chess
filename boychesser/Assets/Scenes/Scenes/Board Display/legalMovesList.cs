using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class LegalMovesList
{

    static readonly int[] knight_moves = { 15, 17, 10, 6, -15, -17, -10, -6 };
    static readonly int[] king_moves = { 1, -1, 8, -8, 9, 7, -9, -7 };
    static readonly int[] cardinal_moves = { 1, -1, 8, -8 };
    static readonly int[] diagonal_moves = { 9, 7, -9, -7 };

    static readonly int[][] numSquaresToEdge;

    String[] moves = new String[220];

    Tile[] tiles;
    

    void getCardinalMoves(Tile tile)
    {
        char type = tile.getPieceType();
        int index = tile.getIndex();

        foreach(int step in cardinal_moves)
        {

            char moveAxis = step == 8 ? 'v' : 'h';

           
            while (!(isLeftEdge(index, step)) ||  || !(isVertEdge(index, step) && moveAxis == 'v'))
            {
                int destinationIndex = index + step;

                if (tiles[destinationIndex].getPieceColor() != tiles[index].getPieceColor())
                {
                    string s = tiles[destinationIndex].getPieceType() == '0'
                        ? type + tiles[index].getName() + tiles[destinationIndex].getName()
                        : type + tiles[index].getName() + 'x' + tiles[destinationIndex].getName();

                    moves[MoveHasher.ChessMoveToHash(s)] = s;
                }
                else if (tiles[destinationIndex].getPieceType() != '0') break;
            }
        }
    }

    void getDiagonalMoves(Tile tile)
    {
        char type = tile.getPieceType();
        int index = tile.getIndex();

        foreach (int step in diagonal_moves)
        {
            int moveAxis = step == 7 ? 'l' : 'r';

            while (!(isRightEdge(index, step) && moveAxis == 'r') || !(isLeftEdge(index, step) && moveAxis == 'l'))
            {
                int destinationIndex = index + step;

                if (tiles[destinationIndex].getPieceColor() != tiles[index].getPieceColor())
                {
                    string s = tiles[destinationIndex].getPieceType() == '0'
                        ? type + tiles[index].getName() + tiles[destinationIndex].getName()
                        : type + tiles[index].getName() + 'x' + tiles[destinationIndex].getName();

                    moves[MoveHasher.ChessMoveToHash(s)] = s;
                }
                else if (tiles[destinationIndex].getPieceType() != '0') break;
            }
        }
    }

    bool isLeftEdge(int index, int step)
    {
        return index + step % 8 == 0;
    }

    bool isRightEdge(int index, int step)
    {
        return index + step % 8 == 7;
    }

    bool isVertEdge(int index, int step)
    {
        return (index + step < 0 || index + step > 63);
    }

    bool isHorizEdge(int index, int step)
    {
        return isRightEdge(index, step) || isLeftEdge(index, step);
    }



    //TODO
    bool putsInCheck(int destinationIndex, Tile[] tiles)
    {
        return false;
    }

    public String[] getLegalMoves(Tile[] tiles, Fen fen)
    {
        this.tiles = tiles;

        for (int start = 0; start < 64; start++)
        {
            char piece = tiles[start].getPieceType();

            if (piece == '0') continue;
            if (tiles[start].getPieceColor() != char.Parse(fen.getActiveColor())) continue;
            if (piece == 'q' || piece == 'r') generateSlidingMoves(tiles[start], piece);

        }
        

        //TODO
        foreach(string s in moves)
        {
            if(s != null) Debug.Log(s);
        }

        return moves;
    }
}
