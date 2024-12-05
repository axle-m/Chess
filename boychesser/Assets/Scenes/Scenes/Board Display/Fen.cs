using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using static PrecomputeMoveData;

public class Fen
{
    public string Board { get; private set; }
    public string ActiveColor { get; private set; }
    public string CastlingAvailability { get; private set; }
    public string EnPassantTarget { get; private set; }
    public int HalfmoveClock { get; private set; }
    public int FullmoveNumber { get; private set; }

    public Fen(string fen)
    {
        ParseFen(fen);
    }

    private void ParseFen(string fen)
    {
        string[] parts = fen.Split(' ');
        if (parts.Length != 6)
        {
            throw new ArgumentException("Invalid FEN string");
        }

        Board = parts[0];
        ActiveColor = parts[1];
        CastlingAvailability = parts[2];
        EnPassantTarget = parts[3];
        HalfmoveClock = int.Parse(parts[4]);
        FullmoveNumber = int.Parse(parts[5]);
    }

    public string getEnPassant()
    {
        return EnPassantTarget;
    }

    public string getBoard()
    {
        return Board;
    }

    public string getActiveColor()
    {
        return ActiveColor;
    }

    public override string ToString()
    {
        return $"{Board} {ActiveColor} {CastlingAvailability} {EnPassantTarget} {HalfmoveClock} {FullmoveNumber}";
    }

    public static string move(string fen, string move)
    {
        // Parse the FEN string
        var parts = fen.Split(' ');
        if (parts.Length != 6)
            throw new ArgumentException("Invalid FEN format");

        string board = parts[0];
        string activeColor = parts[1];
        string castlingRights = parts[2];
        string enPassant = parts[3];
        int halfmoveClock = int.Parse(parts[4]);
        int fullmoveNumber = int.Parse(parts[5]);

        // Parse the move
        string piece = move.Substring(0, 1);

        piece = activeColor == "w" ? piece.ToUpper() : piece.ToLower();

        string from = move.Substring(1, 2);
        string to = move.Substring(3, 2);

        // Convert board to a 2D array for easier manipulation
        var rows = board.Split('/');
        char[,] boardArray = new char[8, 8];
        for (int i = 0; i < 8; i++)
        {
            int col = 0;
            foreach (var ch in rows[i])
            {
                if (char.IsDigit(ch))
                {
                    int emptySquares = ch - '0';
                    for (int j = 0; j < emptySquares; j++)
                        boardArray[i, col++] = '1'; // Represent empty squares
                }
                else
                {
                    boardArray[i, col++] = ch;
                }
            }
        }

        // Map positions to 2D indices
        int fromRow = 8 - int.Parse(from[1].ToString());
        int fromCol = from[0] - 'a';
        int toRow = 8 - int.Parse(to[1].ToString());
        int toCol = to[0] - 'a';

        // Update board for the move
        boardArray[fromRow, fromCol] = '1'; // Empty the source square
        boardArray[toRow, toCol] = piece[0]; // Place the piece on the destination square

        if (piece.ToLower() == "p" && to == enPassant)
        {
            int captureRow = activeColor == "w" ? toRow + 1 : toRow - 1;
            boardArray[captureRow, toCol] = '1'; // Remove captured pawn
        }

        // Rebuild the board string
        string newBoard = string.Join("/", Enumerable.Range(0, 8).Select(r =>
        {
            string row = "";
            int emptyCount = 0;
            for (int c = 0; c < 8; c++)
            {
                if (boardArray[r, c] == '1')
                {
                    emptyCount++;
                }
                else
                {
                    if (emptyCount > 0)
                    {
                        row += emptyCount.ToString();
                        emptyCount = 0;
                    }
                    row += boardArray[r, c];
                }
            }
            if (emptyCount > 0) row += emptyCount.ToString();
            return row;
        }));

        // Update active color
        activeColor = activeColor == "w" ? "b" : "w";

        enPassant = "-";
        if (piece.ToLower() == "p" && Math.Abs(fromRow - toRow) == 2)
        {
            int enPassantRow = activeColor == "b" ? fromRow - 1 : fromRow + 1;
            enPassant = $"{(char)('a' + fromCol)}{8 - enPassantRow}";
        }

        // Update halfmove clock
        if (piece.ToLower() == "p" || boardArray[toRow, toCol] != '1')
        {
            halfmoveClock = 0; // Reset halfmove clock on pawn move or capture
        }
        else
        {
            halfmoveClock++;
        }

        // Update fullmove number
        if (activeColor == "w")
        {
            fullmoveNumber++;
        }

        // Assemble the new FEN
        return $"{newBoard} {activeColor} {castlingRights} {enPassant} {halfmoveClock} {fullmoveNumber}";
    }

    public Tile[] fenToTiles()
    {
        Tile[] tiles = new Tile[64];

        string[] rows = Board.Split('/');

        int index = 0;

        for (int i = rows.Length - 1; i >= 0; i--)
        {
            string row = rows[i];

            for (int j = 0; j < row.Length; j++)
            {
                if (Char.IsDigit(rows[i][j]))
                {
                    index += int.Parse(rows[i][j].ToString());
                    continue;
                }

                Tile tile = new Tile(index, rows[i][j].ToString(), index);
                tiles[index] = tile;

                index++;
            }
        }

        for (int i = 0; i < 64; i++)
        {
            if (tiles[i] == null)
            {
                tiles[i] = new Tile(i, "0", i);
            }
        }

        return tiles;
    }
}