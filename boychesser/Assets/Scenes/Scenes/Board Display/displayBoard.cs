using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class displayBoard : MonoBehaviour
{

    public GameObject lightCol;
    public GameObject darkCol;


    //pieces
    public GameObject w_k;
    public GameObject w_q;
    public GameObject w_r;
    public GameObject w_b;
    public GameObject w_n;
    public GameObject w_p;
    public GameObject b_k;
    public GameObject b_q;
    public GameObject b_r;
    public GameObject b_b;
    public GameObject b_n;
    public GameObject b_p;


    //fen
    public const string startFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    Fen curFen = null;

    //tiles
    public const float tileSize = 1.0f;
    Tile[] tiles = new Tile[64];



    void Start()
    {
        curFen = new Fen(startFen);

        CreateGraphicalBoard();
        placePieces();
    }

    private void Update()
    {
       
    }

    void placePieces()
    {
        string[] rows = curFen.getBoard().Split('/');

        int tile = 0;

        for (int i = rows.Length - 1; i >= 0; i--)
        {
            string row = rows[i];

            for (int j = 0; j < row.Length; j++)
            {
                switch (row[j])
                {
                    case 'K':
                        GameObject KingW = Instantiate(w_k, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'Q':
                        GameObject QueenW = Instantiate(w_q, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'R':
                        GameObject RookW = Instantiate(w_r, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'B':
                        GameObject BishopW = Instantiate(w_b, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'N':
                        GameObject KnightW = Instantiate(w_n, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'P':
                        GameObject PawnW = Instantiate(w_p, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'k':
                        GameObject KingB = Instantiate(b_k, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'q':
                        GameObject QueenB = Instantiate(b_q, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'r':
                        GameObject RookB = Instantiate(b_r, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'b':
                        GameObject BishopB = Instantiate(b_b, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'n':
                        GameObject KnightB = Instantiate(b_n, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'p':
                        GameObject PawnB = Instantiate(b_p, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    default:
                        tile += (int)Char.GetNumericValue(row[j]);
                        break;
                }
            }
        }
    }

    void CreateGraphicalBoard()
    {

        int tileNum = 0;

        for(int file = 0; file < 8; file++)
        {
            for (int rank  = 0; rank < 8; rank++)
            {
                bool isLightSquare = (file+rank) % 2 != 0;

                GameObject squareColor = (isLightSquare) ? lightCol : darkCol;

                GameObject tile = Instantiate(squareColor, new Vector3(file * tileSize - 4.0f, rank * tileSize - 4.0f), Quaternion.identity);

                Tile t = new Tile(tileNum);
                tiles[tileNum] = t;
                tileNum++;
            }
        }
    }
}

class Fen
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

    public string getBoard()
    {
        return Board;
    }

    public override string ToString()
    {
        return $"{Board} {ActiveColor} {CastlingAvailability} {EnPassantTarget} {HalfmoveClock} {FullmoveNumber}";
    }
}

class Tile
{
    private int num;
    private char curPiece;

    public Tile(int num)
    {
        this.num = num;
        curPiece = '0';
    }

    public bool hasPiece()
    {
        return !Char.IsDigit(curPiece);
    }

    public void changeCurPiece(char newPiece)
    {
        curPiece = newPiece;
    }
    
    public char getCurPiece()
    {
        return curPiece;
    }

    public int getNum()
    {
        return num;
    }
}