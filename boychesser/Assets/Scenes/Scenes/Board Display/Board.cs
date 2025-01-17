using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class Board : MonoBehaviour
{

    public GameObject lightCol;
    public GameObject darkCol;
    public GameObject legalCol;


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

    string[] pieceTypeIndex = new string[] { "k", "q", "r", "b", "n", "p" };

    //fen
    //"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

    public const string START_FEIN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

    public static Fen curFen;

    //tiles
    public const float tileSize = 1.0f;
    public static Tile[] tiles = new Tile[64];

    readonly string[] ranks = new string[] { "a", "b", "c", "d", "e", "f", "g", "h" };
    readonly string[] files = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" };

    Tile selectedTile = null;
    private string botColor = "b";
    public int moves = 0;
    public GameOverScreen GameOverScreen;
    
    void Start()
    {
        
        //randomly select bot color
        System.Random random = new System.Random();
        //botColor = random.Next(0, 2) == 0 ? "w" : "b";

        curFen = new Fen(START_FEIN);
        
        CreateGraphicalBoard();
        placePieces();
        PrecomputeMoveData.precomputedMoveData();
    }

    private void Update()
    {
        if (curFen.winConditions().Equals("continue")) {
            Debug.Log("Active: " + curFen.getActiveColor());
            Debug.Log("Bot: " + botColor);

            if (curFen.getActiveColor().Equals(botColor))
            {
                String[] moves = LegalMovesList.getLegalMoves(curFen);
                curFen = new Fen(Fen.move(curFen.ToString(), ChessBot.playBestMove(curFen.ToString())));
                placePieces();
            }
            else tryMove();
            moves++;
        }
        else{
            GameOverScreen.Setup(moves);
        }
        
    }

    void tryMove()
    {
        String[] moves = LegalMovesList.getLegalMoves(curFen);
        string move = getAttemptedMove();
        if(move != null)
        {
            if(search(move, moves))
            {

                curFen = new Fen(Fen.move(curFen.ToString(), move));

                placePieces();
            }
            else
            {
                Debug.Log(move + " Illegal move");
            }
        }
    }

    string getAttemptedMove()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            {

                var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0f;

                //identify target tile
                if (mouseWorldPos.x > -4.0f && mouseWorldPos.x < 4.0f && mouseWorldPos.y > -4.0f && mouseWorldPos.y < 4.0f)
                {

                    //convert mouse position to file and rank
                    int file = Mathf.FloorToInt(0.5f + (mouseWorldPos.x + 4.0f) / tileSize);
                    int rank = Mathf.FloorToInt(0.5f + (mouseWorldPos.y + 4.0f) / tileSize);

                    if (file >= 0 && file < 8 && rank >= 0 && rank < 8)
                    {

                        //if no tile is selected, select the tile
                        if (selectedTile == null)
                        {
                            selectedTile = tiles[file + rank * 8];
                            Debug.Log(selectedTile.getCurPiece());

                            //reset selected tile if no piece is on the tile
                            if (selectedTile.getCurPiece() == '0')
                            {
                                selectedTile = null;
                            }
                        }

                        //if a tile is selected, return the attempted move
                        else if (selectedTile != null)
                        {
                            string toReturn = Char.ToUpper(selectedTile.getPieceType()) + selectedTile.getName() + tiles[file + rank * 8].getName();
                            selectedTile = null;
                            return toReturn;

                        }
                    }
                }
            }
        }

        return null;
    } //getAttemptedMove();

    public bool search(string move, string[] moves)
    {
        foreach(string s in moves)
        {
            if (s == move) return true;
        }
        return false;
    }

    void placePieces()
    {
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("piece");
        foreach (GameObject obj in pieces)
        {
            Destroy(obj);
        }

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
                        KingW.tag = "piece";
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'Q':
                        GameObject QueenW = Instantiate(w_q, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        QueenW.tag = "piece";
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'R':
                        GameObject RookW = Instantiate(w_r, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        RookW.tag = "piece";
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'B':
                        GameObject BishopW = Instantiate(w_b, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        BishopW.tag = "piece";
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'N':
                        GameObject KnightW = Instantiate(w_n, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        KnightW.tag = "piece";
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'P':
                        GameObject PawnW = Instantiate(w_p, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        PawnW.tag = "piece";
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'k':
                        GameObject KingB = Instantiate(b_k, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        KingB.tag = "piece";
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'q':
                        GameObject QueenB = Instantiate(b_q, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        QueenB.tag = "piece";
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'r':
                        GameObject RookB = Instantiate(b_r, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        RookB.tag = "piece";
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'b':
                        GameObject BishopB = Instantiate(b_b, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        BishopB.tag = "piece";
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'n':
                        GameObject KnightB = Instantiate(b_n, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        KnightB.tag = "piece";
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    case 'p':
                        GameObject PawnB = Instantiate(b_p, new Vector3((tile % 8) * tileSize - 4.0f, (int)(tile / 8) * tileSize - 4.0f, -1), Quaternion.identity);
                        PawnB.tag = "piece";
                        tiles[tile].changeCurPiece(row[j]);
                        tile++;
                        break;
                    default:
                        for(int k = 0; k < (int)Char.GetNumericValue(row[j]); k++)
                        {
                            tiles[tile + k].changeCurPiece('0');
                           
                        }
                        tile += (int)Char.GetNumericValue(row[j]);
                        break;
                }
            }
        }
    }

    void CreateGraphicalBoard()
    {

        int tileNum = 0;

        for (int file = 0; file < 8; file++)
        {
            for (int rank = 0; rank < 8; rank++)
            {
                bool isLightSquare = (file + rank) % 2 != 0;

                GameObject squareColor = (isLightSquare) ? lightCol : darkCol;

                GameObject tile = Instantiate(squareColor, new Vector3(file * tileSize - 4.0f, rank * tileSize - 4.0f), Quaternion.identity);
                tile.tag = "tile";


                string tileName = ranks[rank] + files[file];
                Tile t = new Tile(tileNum, tileName, tileNum);
                tiles[tileNum] = t;
                tileNum++;
            }
        }
    }
}
