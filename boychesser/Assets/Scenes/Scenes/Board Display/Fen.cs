using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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
        //split fen string into 6 parts with delimiter of a space char
        //ai did this and idk anything about fen strings so not sure if this is accurate
        //first part is the board
        //second part is the active color (w or b)
        //third part is the castling availability (e.g. KQkq)
        //fourth part is the en passant target square (e.g. e6)
        //fifth part is the halfmove clock (number of halfmoves since the last pawn move)
        //sixth part is the fullmove number (number of moves since the start of the game)

        // throw an exception if the number of parts is not 6, which means the FEN string is invalid.
        string[] parts = fen.Split(' ');
        if (parts.Length != 6)
        {
            throw new ArgumentException("Invalid FEN string");
        }
        //assigns each piece of the fen string to a variable
        //active color is who's turn it is I think 
            //yes this is correct -alex

        Board = parts[0];
        ActiveColor = parts[1];
        CastlingAvailability = parts[2];
        EnPassantTarget = parts[3];
        HalfmoveClock = int.Parse(parts[4]);
        FullmoveNumber = int.Parse(parts[5]);
    }

    public string getCastlingAvailability()
    {
        return CastlingAvailability;
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

        string board = parts[0];
        string activeColor = parts[1];
        string castlingRights = parts[2];
        string enPassant = parts[3];
        int halfmoveClock = int.Parse(parts[4]);
        int fullmoveNumber = int.Parse(parts[5]);

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

        // Parse the move
        char piece = move[0];

        piece = activeColor.Equals("w") ? Char.ToUpper(piece) : Char.ToLower(piece);

        string from, to;

        from = move.Substring(1, 2);
        to = move.Substring(3, 2);

        move = normaliseMove(move);

        if (move.Equals("O-O") || move.Equals("O-O-O"))
        {
            int row = activeColor.Equals("w") ? 7 : 0;
            int kingCol = 4;
            int rookCol = move.Equals("O-O") ? 7 : 0;

            boardArray[row, kingCol] = '1';
            boardArray[row, rookCol] = '1';
            boardArray[row, kingCol + (move.Equals("O-O") ? 2 : -2)] = activeColor.Equals("w") ? 'K' : 'k';
            boardArray[row, kingCol + (move.Equals("O-O") ? 1 : -1)] = activeColor.Equals("w") ? 'R' : 'r';

            char side = move.Equals("O-O") ? 'k' : 'q';
            char sideCol = activeColor.Equals("w") ? char.ToUpper(side) : side;
            castlingRights = castlingRights.Remove(castlingRights.IndexOf(sideCol), 1);

            if (castlingRights.Equals(""))
            {
                castlingRights = "-";
            }

            Debug.Log(castlingRights);
        }

        else
        {
            // Map positions to 2D indices
            int fromRow = 8 - int.Parse(from[1].ToString());
            int fromCol = from[0] - 'a';
            int toRow = 8 - int.Parse(to[1].ToString());
            int toCol = to[0] - 'a';

            // Update board for the move
            boardArray[fromRow, fromCol] = '1'; // Empty the source square
            boardArray[toRow, toCol] = piece; // Place the piece on the destination square

            if (Char.ToLower(piece) == 'p' && to == enPassant)
            {
                int captureRow = activeColor.Equals("w") ? toRow + 1 : toRow - 1;
                boardArray[captureRow, toCol] = '1'; // Remove captured pawn
            }

            enPassant = "-";
            if (Char.ToLower(piece) == 'p' && Math.Abs(fromRow - toRow) == 2)
            {
                int enPassantRow = activeColor.Equals("b") ? fromRow - 1 : fromRow + 1;
                enPassant = $"{(char)('a' + fromCol)}{8 - enPassantRow}";
            }
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
        activeColor = activeColor.Equals("w") ? "b" : "w";
        Debug.Log(activeColor);

        // Update halfmove clock
        if (Char.ToLower(piece) == 'p')
        {
            halfmoveClock = 0; // Reset halfmove clock on pawn move or capture
        }

        else
        {
            halfmoveClock++;
        }

        // Update fullmove number
        if (activeColor.Equals("w"))
        {
            fullmoveNumber++;
        }

        //Debug.Log(enPassant);

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

    private static string normaliseMove(string move)    //could also use this to add pgn compatibility
    {

        Debug.Log(move);

        if (char.ToLower(move[0]) == 'k')
        {
            if(move == "Ke1g1" || move == "Ke8g8")
            {
                return "O-O";
            }
            else if(move == "Ke1c1" || move == "Ke8c8")
            {
                return "O-O-O";
            }
        }

        return move;    //return literal move for now, will implement other normalisations later
    }
}