using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrecomputeMoveData
{
    public static readonly int[] knight_moves = { 15, 17, 10, 6, -15, -17, -10, -6 };
    public static readonly int[] king_moves = { 1, -1, 8, -8, 9, 7, -9, -7 };
    public static readonly int[] cardinal_moves = { 1, -1, 8, -8 };
    public static readonly int[] diagonal_moves = { 9, 7, -9, -7 };

    public static readonly int[][] numSquaresToEdge;

    static void precomputedMoveData()
    {
        for (int file = 0; file < 8; file++)
        {
            for(int rank = 0; rank < 8; rank++)
            {
                int north = 7 - rank;
                int south = rank;
                int east = 7 - file;
                int west = file;

                int squareIndex = rank * 8 + file;

                int northEast = Mathf.Min(north, east);
                int northWest = Mathf.Min(north, west);
                int southEast = Mathf.Min(south, east);
                int southWest = Mathf.Min(south, west);

                numSquaresToEdge[squareIndex] = new int[] { north, south, east, west, northEast, northWest, southEast, southWest };
            }
        }
    }
}
