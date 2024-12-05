using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveHasher
{
    private static int ChessMoveToHash(String attMove)
    {
        //dictionary for piece file and rank mapping
        var movePatterns = new Dictionary<char, int>
        {
            { 'P', 0 }, { 'Q', 1 }, { 'R', 2 }, { 'B', 3 }, { 'N', 4 }, { 'K', 5 },
            { 'a', 0 }, { 'b', 1 }, { 'c', 2 }, { 'd', 3 }, { 'e', 4 }, { 'f', 5 }, { 'g', 6 }, { 'h', 7 },
            { '1', 0 }, { '2', 1 }, { '3', 2 }, { '4', 3 }, { '5', 4 }, { '6', 5 }, { '7', 6 }, { '8', 7 }
        };

        int fileFromHashValue = 0, RankFromHashValue = 0, pieceCode = 0, fileToCode = 0, rankToCode = 0;

        //castling
        if (attMove == "O-O")
        {
            return 218;
        }
        else if (attMove == "O-O-O")
        {
            return 219;
        }

        char[] move = attMove.ToCharArray();

        try
        {
            pieceCode = movePatterns[move[0]];
            fileFromHashValue = movePatterns[move[1]];
            RankFromHashValue = movePatterns[move[2]];
            fileToCode = movePatterns[move[3]];
            rankToCode = movePatterns[move[4]];
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug.Log(move.ToString());
        }

        int hashCode = (pieceCode * 4096) + (fileToCode * 512) + (rankToCode * 64) + (fileFromHashValue * 8) + (RankFromHashValue);

        //maximum number of legal moves in a given chess position is cited as 218
        //add 2 for castling moves
        //while this is not exact, it is highly unlikely to occur and not an exact limit, therefore a good upper bound
        hashCode %= 218;

        return hashCode;
    }

    public static int hash(string move, String[] moves)
    {
        int code = ChessMoveToHash(move);

        while (moves[code] != null && !moves[code].Equals(move))
        {
            code++;
            code %= 218;
        }


        return code;
    }
}
