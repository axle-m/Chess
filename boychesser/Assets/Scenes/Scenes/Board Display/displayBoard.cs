using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class DisplayBoard : MonoBehaviour
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

    string[] pieceTypeIndex = new string[] { "k", "q", "r", "b", "n", "p" };

    //fen
    //"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

    public const string startFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    Fen curFen;

    //tiles
    public const float tileSize = 1.0f;
    public Tile[] tiles = new Tile[64];
    readonly  string[] ranks = new string[] { "a", "b", "c", "d", "e", "f", "g", "h" };
    readonly string[] files = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" };
    Tile selectedTile = null;
    Tile targetTile = null;

    void Start()
    {
        curFen = new Fen(startFen);

        CreateGraphicalBoard();
        placePieces();
    }

    private void Update()
    {
        move();
    }

    void move()
    {

        LegalMovesList legalMovesList = new LegalMovesList();
        string[] moves = legalMovesList.getLegalMoves(tiles, curFen);

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

                        //if a tile is selected, move the piece from the selected tile to the target tile
                        else if (selectedTile != null)
                        {
                            targetTile = tiles[file + rank * 8];

                            string attemptedMove = Char.ToUpper(selectedTile.getPieceType()) + targetTile.getName();
                            if (targetTile.getCurPiece() != '0')
                            {
                                attemptedMove = Char.ToUpper(selectedTile.getPieceType()) + "x" + targetTile.getName();
                            }

                            Debug.Log("Attempted move: " + attemptedMove);

                            if (isLegalMove(attemptedMove, moves))
                            {

                                targetTile.changeCurPiece(selectedTile.getCurPiece());
                                selectedTile.changeCurPiece('0');

                                //reset selected and target tiles
                                selectedTile = null;
                                targetTile = null;

                                //update fen

                                curFen = new Fen(createNewFen(attemptedMove));

                                //replace pieces

                                GameObject[] pieces = GameObject.FindGameObjectsWithTag("piece");
                                foreach (GameObject obj in pieces)
                                {
                                    Destroy(obj);
                                }

                                placePieces();
                                Debug.Log(curFen.ToString());
                            } 
                            
                            else
                            {
                                selectedTile = null;
                                targetTile = null;

                                Debug.Log(attemptedMove + " is not a legal move");
                            }
                        }
                    }
                }
            }
        }
    }

    bool isLegalMove(string attemptedMove, string[] moves)
    {
        if (Char.ToLower(attemptedMove[0]) == 'p') attemptedMove = attemptedMove.Substring(1);
        return moves[MoveHasher.ChessMoveToHash(attemptedMove)] == attemptedMove;
    }

    string getNewBoardstate()
    {
        string boardFen = "";
        int emptyCount = 0;

        for (int rank = 7; rank >= 0; rank--)
        {
            for (int file = 0; file < 8; file++)
            {
                Tile currentTile = tiles[file + rank * 8];
                char piece = currentTile.getCurPiece();

                if (piece == '0')
                {
                    emptyCount++;
                }
                else
                {
                    if (emptyCount > 0)
                    {
                        boardFen += emptyCount.ToString();
                        emptyCount = 0;
                    }
                    boardFen += piece;
                }
            }

            if (emptyCount > 0)
            {
                boardFen += emptyCount.ToString();
                emptyCount = 0;
            }

            if (rank > 0)
            {
                boardFen += "/";
            }
        }

        return boardFen;
    }

    string createNewFen(string moveType)
    {

        // Extract the other parts of the previous FEN
        string curFenString = curFen.ToString();
        string[] fenParts = curFenString.Split(' ');
        string activeColor = fenParts[1] == "w" ? "b" : "w";
        string castlingAvailability = fenParts.Length > 2 ? fenParts[2] : "-";
        string enPassantTarget = fenParts.Length > 3 ? fenParts[3] : "-";
        string halfmoveClock = fenParts.Length > 4 ? fenParts[4] : "0";
        string fullmoveNumber = fenParts.Length > 5 ? fenParts[5] : "1";

        // Construct the full FEN string
        string newFenString = getNewBoardstate() + " " + activeColor + " " + castlingAvailability + " " + enPassantTarget + " " + halfmoveClock + " " + fullmoveNumber;
        return newFenString;
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


                string tileName = ranks[rank] + files[file];
                Tile t = new Tile(tileNum, tileName, tileNum);
                tiles[tileNum] = t;
                tileNum++;
            }
        }
    }
}
