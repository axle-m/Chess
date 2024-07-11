using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class MoveHasher : MonoBehaviour
{
    public static int ChessMoveToHash(string move)
    {
        //dictionary for piece file and rank mapping
        var movePatterns = new Dictionary<char, int>
        {
            { 'P', 0 }, { 'Q', 1 }, { 'R', 2 }, { 'B', 3 }, { 'N', 4 }, { 'K', 5 },
            { 'a', 0 }, { 'b', 1 }, { 'c', 2 }, { 'd', 3 }, { 'e', 4 }, { 'f', 5 }, { 'g', 6 }, { 'h', 7 },
            { '1', 0 }, { '2', 1 }, { '3', 2 }, { '4', 3 }, { '5', 4 }, { '6', 5 }, { '7', 6 }, { '8', 7 }
        };

        //castling
        if (move == "O-O")
        {
            return 208;
        }
        else if (move == "O-O-O")
        {
            return 209;
        }

        //remove x if move is a capture
        if (move.Contains('x'))
        {
            move = move.Replace("x", "");

        }

        

        string piece = Char.IsUpper(move[0]) ? move[0].ToString() : "";

        int fileFromHashValue = 0, RankFromHashValue = 0;

        if (move.Length >= 4)
        {
            string fileFrom = Char.IsUpper(move[0]) ? move[1].ToString() : move[0].ToString();
            int fileFromCode = fileFrom.Length > 0 ? movePatterns[fileFrom[0]] : 0;
            fileFromHashValue = fileFromCode;
            move = move.Remove(1);
        }
        if (move.Length == 4)
        {
            string rankFrom = Char.IsUpper(move[0]) ? move[2].ToString() : move[1].ToString();
            int rankFromCode = rankFrom.Length > 0 ? int.Parse(rankFrom) - 1 : 0;
            RankFromHashValue = rankFromCode;
            move.Remove(1);
        }
        string to = Char.IsUpper(move[0]) ? move.Substring(1) : move.Substring(0);

        int pieceCode = piece.Length > 0 ? movePatterns[piece[0]] : 0;
        int rankToCode = movePatterns[to[0]];
        int fileToCode = int.Parse(to[1].ToString()) - 1;

        int hashCode = (pieceCode * 4096) + (fileToCode * 512) + (rankToCode * 64) + (fileFromHashValue * 8) + (RankFromHashValue);

        //maximum number of legal moves in a given chess position is cited as 218
        //while this is not exact, and highly unlikely, it is a good upper bound
        hashCode %= 218;

        return hashCode;
    }
}
