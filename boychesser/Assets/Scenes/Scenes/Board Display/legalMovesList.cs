using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using static PrecomputeMoveData;
using Unity.VisualScripting;

public class LegalMovesList
{

    public static readonly int[] knight_moves = { 15, 17, 10, 6, -15, -17, -10, -6 };
    public static readonly int[] king_moves = { 1, -1, 8, -8, 9, 7, -9, -7 };
    public static readonly int[] pawn_captures = { 7, 9 };

    static String[] legalMoves;

    //TODO
    static bool putsInCheck(int destinationIndex, Tile[] tiles)
    {
        return false;
    }
    //generate a list of legal moves in a fen string array
    public static string[] getLegalMoves(Fen fen)
    {

        Tile[] tiles = fen.fenToTiles();    

        List<string> moves = new List<string>();
        //iterate over all 64 tiles of chess board
        for (int start = 0; start < 64; start++)
        {
            //get the piece on current tile
            char piece = tiles[start].getPieceType();

            //
            if (tiles[start].getPieceColor() != fen.getActiveColor()[0]) continue;


            switch (piece)
            {
                case 'q': case 'b': case 'r':
                    generateSlidingMoves(tiles[start], tiles, piece, moves);
                    break;
                case 'n':

                    foreach (int move in knight_moves)
                    {
                        int destinationIndex = start + move;

                        //bounds check
                        if (destinationIndex >= 0 && destinationIndex < 64)
                        {
                            //wraparound check
                            int targetFile = destinationIndex % 8;
                            int curFile = start % 8;

                            if (Math.Abs(targetFile - curFile) == 2 || Math.Abs(targetFile - curFile) == 1)
                            {
                                if (tiles[destinationIndex].getPieceType() == '0' || (tiles[destinationIndex].getPieceColor() != tiles[start].getPieceColor()))
                                {
                                    string s = "N" + tiles[start].getName() + tiles[destinationIndex].getName();
                                    moves.Add(s);
                                    tiles[destinationIndex].setLegalMove(true);


                                }
                            }
                        }
                    }
                    break;
                case 'k':
                    foreach (int move in king_moves)
                    {
                        int destinationIndex = start + move;

                        //bounds check
                        if (destinationIndex >= 0 && destinationIndex < 64)
                        {
                            //wraparound check
                            int targetFile = destinationIndex % 8;
                            int curFile = start % 8;

                            if (Math.Abs(curFile - targetFile) <= 1)
                            {
                                if ((tiles[destinationIndex].getPieceType() == '0' || tiles[destinationIndex].getPieceColor() != tiles[start].getPieceColor()) && !putsInCheck(destinationIndex, tiles))
                                {
                                    string s = "K" + tiles[start].getName() + tiles[destinationIndex].getName();
                                    moves.Add(s);
                                    tiles[destinationIndex].setLegalMove(true);
                                }
                            }
                        }
                    }

                    //castling

                    string availability = fen.getCastlingAvailability();

                    foreach (char c in availability)
                    {
                        if (c == 'K' && tiles[5].getPieceType() == '0' && tiles[6].getPieceType() == '0' && !putsInCheck(5, tiles) && !putsInCheck(6, tiles))
                        {
                            if (fen.getActiveColor().Equals("b")) break;
                            string s = "K" + tiles[start].getName() + tiles[6].getName();
                            moves.Add(s);
                            tiles[6].setLegalMove(true);

                        }
                        if (c == 'Q' && tiles[1].getPieceType() == '0' && tiles[2].getPieceType() == '0' && tiles[3].getPieceType() == '0' && !putsInCheck(2, tiles) && !putsInCheck(3, tiles))
                        {
                            if (fen.getActiveColor().Equals("b")) break;
                            string s = "K" + tiles[start].getName() + tiles[2].getName();
                            moves.Add(s);
                            tiles[2].setLegalMove(true);

                        }
                        if (c == 'k' && tiles[61].getPieceType() == '0' && tiles[62].getPieceType() == '0' && !putsInCheck(61, tiles) && !putsInCheck(62, tiles))
                        {
                            if (fen.getActiveColor().Equals("w")) break;
                            string s = "K" + tiles[start].getName() + tiles[62].getName();
                            moves.Add(s);
                            tiles[62].setLegalMove(true);

                        }
                        if (c == 'q' && tiles[57].getPieceType() == '0' && tiles[58].getPieceType() == '0' && tiles[59].getPieceType() == '0' && !putsInCheck(58, tiles) && !putsInCheck(59, tiles))
                        {
                            if (fen.getActiveColor().Equals("w")) break;
                            string s = "K" + tiles[start].getName() + tiles[58].getName();
                            moves.Add(s);
                            tiles[58].setLegalMove(true);
                        }
                    }

                    break;

                case 'p':

                    int direction = tiles[start].getPieceColor() == 'w' ? 1 : -1;
                    int forward = start + 8 * direction;

                    // Single forward move
                    if (IsWithinBounds(forward) && tiles[forward].getPieceType() == '0')
                    {

                        string s = "P" + tiles[start].getName() + tiles[forward].getName();
                        moves.Add(s);
                        tiles[forward].setLegalMove(true);


                        // Double forward move (only from starting position)
                        if (direction == 1 && start / 8 == 1 && tiles[forward + 8].getPieceType() == '0')
                        {
                            s = "P" + tiles[start].getName() + tiles[forward + 8].getName();
                            moves.Add(s);
                            tiles[forward + 8].setLegalMove(true);

                        }
                        else if (direction == -1 && start / 8 == 6 && tiles[forward - 8].getPieceType() == '0')
                        {
                            s = "P" + tiles[start].getName() + tiles[forward - 8].getName();
                            moves.Add(s);
                            tiles[forward - 8].setLegalMove(true);

                        }
                    }

                    // Diagonal captures

                    foreach(int n in pawn_captures)
                    {
                        int destinationIndex = start + n * direction;

                        if (!IsWithinBounds(destinationIndex) || Math.Abs(destinationIndex % 8 - start % 8) != 1) continue;

                        if (tiles[destinationIndex].getPieceColor() != tiles[start].getPieceColor() && tiles[destinationIndex].getPieceType() != '0')
                        {
                            string s = "P" + tiles[start].getName() + tiles[destinationIndex].getName();
                            moves.Add(s);
                            tiles[destinationIndex].setLegalMove(true);
                        }

                        if (tiles[destinationIndex].getName().Equals(fen.getEnPassant()))
                        {
                            string s = "P" + tiles[start].getName() + tiles[destinationIndex].getName();
                            moves.Add(s);
                            tiles[destinationIndex].setLegalMove(true);
                        }
                    }

                    //TODO
                    break;
                default:
                    continue;
            }
        }

        legalMoves = orderMoves(moves, tiles);

        return legalMoves;

    }

    static private string[] orderMoves(List<string> moves, Tile[] tiles)
    {
        List<string[]> captures = new List<string[]>();
        List<string> nonCaptures = new List<string>();

        foreach (string move in moves)
        {
            int start = Tile.ToIndex(move.Substring(1, 2)); //Substring in c# takes in (index, length)
            int end = Tile.ToIndex(move.Substring(3, 2));

            if (tiles[end].getPieceType() != '0')
            {
                captures.Add(new string[] { move, (Scorer.piece_values[tiles[end].getPieceType()] * 100 - Scorer.piece_values[tiles[start].getPieceType()]).ToString() });
            }
            else
            {
                nonCaptures.Add(move);
            }
        }

        List<string> orderedMoves = new List<string>();

        foreach (string s in sortCaptures(captures))
        {
            orderedMoves.Add(s);
        }

        foreach (string s in nonCaptures)
        {
            orderedMoves.Add(s);
        }

        return orderedMoves.ToArray();
    }

    static private List<string> sortCaptures(List<string[]> captures)
    {
        List<string> sortedCaptures = new List<string>();

        List<string[]> sorted = captures.OrderBy(x => x[1]).ToList();

        foreach (string[] s in sorted)
        {
            sortedCaptures.Add(s[0]);
        }

        return sortedCaptures;
    }

    static private bool IsWithinBounds(int tileIndex)
    {
        return tileIndex >= 0 && tileIndex < 64;
    }

    static private bool isAtEdge(int tileIndex)
    {
        return tileIndex % 8 == 0 || tileIndex % 8 == 7;
    }

    static void generateSlidingMoves(Tile tile, Tile[] tiles, char piece, List<string> moves)
    {
        
        // Determine the directions in which the piece can move
        // dirIndex stores the move offsets, first 4 are cardinal last 4 are diagonal
        int startDirIndex = piece == 'b' ? 4 : 0;
        int endDirIndex = piece == 'r' ? 4 : 8;

        for(int dirIndex = startDirIndex; dirIndex < endDirIndex; dirIndex++)
        {

            // iterate over all possible moves in the direction
            for(int n = 0; n < PrecomputeMoveData.numSquaresToEdge[tile.index][dirIndex]; n++)
            {
                int destinationIndex = tile.index + PrecomputeMoveData.cardinal_moves[dirIndex] * (n + 1);

                if (!IsWithinBounds(destinationIndex)) continue;
                if (tiles[destinationIndex].getPieceColor() == tile.getPieceColor()) break;

                string s = char.ToUpper(piece) + tile.getName() + tiles[destinationIndex].getName();
                moves.Add(s);

                tiles[tile.index].setLegalMove(true);

                //break if no more legal moves in this direction

                if (isAtEdge(destinationIndex)) break;

                if (tiles[destinationIndex].getPieceType() != '0') break;
           }
        }
    }

    internal object getLegalMove()
    {
        throw new NotImplementedException();
    }

    internal object getLegalMoves(object fen)
    {
        throw new NotImplementedException();
    }
}