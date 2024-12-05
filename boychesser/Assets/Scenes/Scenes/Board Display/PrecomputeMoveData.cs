using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrecomputeMoveData
{
    public static readonly int[] cardinal_moves = { 8, -8, 1, -1, 9, 7, -9, -7 };

    public static readonly int[][] numSquaresToEdge = new int[64][];

    public static void precomputedMoveData()
    {
        for (int file = 0; file < 8; file++)
        {
            for(int rank = 0; rank < 8; rank++)
            {
                int squareIndex = rank * 8 + file;
                
                int north = 7 - rank;
                int south = rank;
                int east = 7 - file;
                int west = file;

                int northEast = Mathf.Min(north, east);
                int northWest = Mathf.Min(north, west);
                int southEast = Mathf.Min(south, east);
                int southWest = Mathf.Min(south, west);

                numSquaresToEdge[squareIndex] = new int[] { north, south, east, west, northEast, northWest, southEast, southWest };

                //Debug.Log("Square: " + squareIndex + " N: " + north + " S: " + south + " E: " + east + " W: " + west + " NE: " + northEast + " NW: " + northWest + " SE: " + southEast + " SW: " + southWest);
            }
        }
    }
    

    //might refactor again to follow this structure, unsure
    public struct move { public readonly int StartSquare; public readonly int TargetSquare; public readonly int moveType; }

}
