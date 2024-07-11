using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class testing : MonoBehaviour
{

    public GameObject lightCol;
    public GameObject darkCol;
    public GameObject selectedCol;


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
    //"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    
    public const string startFen = "8/8/8/8/3Q4/8/8/8 w KQkq - 0 1";
    Fen curFen = null;

    //tiles
    public const float tileSize = 1.0f;
    public Tile[] tiles = new Tile[64];
    string[] ranks = new string[] { "a", "b", "c", "d", "e", "f", "g", "h" };
    string[] files = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" };
    Tile selectedTile = null;
    Tile targetTile = null;

    void Start()
    {
        curFen = new Fen(startFen);

        createBoard();
        legalMovesList();
        displayBoard();
        placePieces();
    }

    private void Update()
    {
        move();
    }

    void move()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            {

                var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0f;              

                //identify target tile
                if(mouseWorldPos.x > -4.0f && mouseWorldPos.x < 4.0f && mouseWorldPos.y > -4.0f && mouseWorldPos.y < 4.0f)
                {
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
                            if(selectedTile.getCurPiece() == '0')
                            {
                                selectedTile = null;
                            }
                        } 

                        //if a tile is selected, move the piece from the selected tile to the target tile
                        else if (selectedTile != null)
                        {
                            targetTile = tiles[file + rank * 8];
                            Debug.Log(targetTile.getName());

                            targetTile.changeCurPiece(selectedTile.getCurPiece());
                            selectedTile.changeCurPiece('0');                          

                            //reset selected and target tiles
                            selectedTile = null;
                            targetTile = null;

                            //updaete fen
                            
                            curFen = new Fen(createNewFen(null));
                            //replace pieces

                            GameObject[] pieces = GameObject.FindGameObjectsWithTag("piece");
                            foreach (GameObject obj in pieces)
                            {
                                Destroy(obj);
                            }

                            placePieces();
                            Debug.Log(curFen.ToString());
                        }
                    }
                }
            }
        }
    }
    //TODO: implement putsInCheck

    bool putsInCheck()
    {
        return false;
    }



     List<string>[] legalMovesList()
    {
        List<string>[] legalMovesList = new List<string>[6];

        for (int j = 0; j < 6; j++)
        {
            legalMovesList[j] = new List<string>();
        }

        for (int i = 0; i < 64; i++)
        {
            bool exhaustedMoves = false;
            bool isLeftEdge = i % 8 == 0;
            bool isRightEdge = i % 8 == 7;
            bool isTopEdge = i > 55;
            bool isBottomEdge = i < 8;

            if (tiles[i].hasPiece())
            {
                switch (tiles[i].getPieceType())
                {
                    case 'k':

                        while (!exhaustedMoves)
                        {

                            if (!isLeftEdge)
                            {
                                //move up left
                                if (i + 7 < 63 && tiles[i + 7].getPieceType() == '0' && !putsInCheck())
                                {
                                    legalMovesList[0].Add("K" + tiles[i + 7].getName());
                                    tiles[i].setLegalMove(true);
                                }
                                else if (i + 7 < 63 && tiles[i + 7].getPieceColor() != tiles[i].getPieceColor() && !putsInCheck())
                                {
                                    legalMovesList[0].Add("Kx" + tiles[i + 7].getName());
                                    tiles[i].setLegalMove(true);
                                }

                                //move down left
                                if (i - 7 > 0 && tiles[i - 7].getPieceType() == '0' && !putsInCheck())
                                {
                                    legalMovesList[0].Add("K" + tiles[i - 7].getName());
                                    tiles[i].setLegalMove(true);
                                }
                                else if (i - 7 > 0 && tiles[i - 7].getPieceColor() != tiles[i].getPieceColor() && !putsInCheck())
                                {
                                    legalMovesList[0].Add("Kx" + tiles[i - 7].getName());
                                    tiles[i].setLegalMove(true);
                                }

                                //move left
                                if (i - 1 > 0 && tiles[i - 1].getPieceType() == '0' && !putsInCheck())
                                {
                                    legalMovesList[0].Add("K" + tiles[i - 1].getName());
                                    tiles[i].setLegalMove(true);
                                }
                                else if (i - 1 > 0 && tiles[i + 1].getPieceColor() != tiles[i].getPieceColor() && !putsInCheck())
                                {
                                    legalMovesList[0].Add("Kx" + tiles[i - 1].getName());
                                    tiles[i].setLegalMove(true);
                                }
                            }

                            if (!isRightEdge)
                            {
                                //move up right
                                if (i + 9 < 63 && tiles[i + 9].getPieceType() == '0' && !putsInCheck())
                                {
                                    legalMovesList[0].Add("K" + tiles[i + 9].getName());
                                    tiles[i].setLegalMove(true);
                                }
                                else if (i + 9 < 63 && tiles[i + 9].getPieceColor() != tiles[i].getPieceColor() && !putsInCheck())
                                {
                                    legalMovesList[0].Add("Kx" + tiles[i + 9].getName());
                                    tiles[i].setLegalMove(true);
                                }

                                //move down right
                                if (i - 8 > 0 && tiles[i - 9].getPieceType() == '0' && !putsInCheck())
                                {
                                    legalMovesList[0].Add("K" + tiles[i - 8].getName());
                                    tiles[i].setLegalMove(true);
                                }
                                else if (i - 8 > 0 && tiles[i - 8].getPieceColor() != tiles[i].getPieceColor() && !putsInCheck())
                                {
                                    legalMovesList[0].Add("Kx" + tiles[i - 8].getName());
                                    tiles[i].setLegalMove(true);
                                }

                                //move right
                                if (i + 1 < 63 && tiles[i + 1].getPieceType() == '0' && !putsInCheck())
                                {
                                    legalMovesList[0].Add("K" + tiles[i - 7].getName());
                                    tiles[i].setLegalMove(true);
                                }
                                else if (i + 1 < 63 && tiles[i + 1].getPieceColor() != tiles[i].getPieceColor() && !putsInCheck())
                                {
                                    legalMovesList[0].Add("Kx" + tiles[i + 1].getName());
                                    tiles[i].setLegalMove(true);
                                }
                            }


                            //move up
                            if (i + 8 < 63 && tiles[i + 8].getPieceType() == '0' && !putsInCheck())
                            {
                                legalMovesList[0].Add("K" + tiles[i + 8].getName());
                                tiles[i].setLegalMove(true);
                            }
                            else if (i + 8 < 63 && tiles[i + 8].getPieceColor() != tiles[i].getPieceColor() && !putsInCheck())
                            {
                                legalMovesList[0].Add("Kx" + tiles[i + 8].getName());
                                tiles[i].setLegalMove(true);
                            }

                            //move down
                            if (i - 8 > 0 && tiles[i - 8].getPieceType() == '0' && !putsInCheck())
                            {
                                legalMovesList[0].Add("K" + tiles[i - 8].getName());
                                tiles[i].setLegalMove(true);
                            }
                            else if (i - 8 > 0 && tiles[i - 8].getPieceColor() != tiles[i].getPieceColor() && !putsInCheck())
                            {
                                legalMovesList[0].Add("Kx" + tiles[i - 8].getName());
                                tiles[i].setLegalMove(true);
                            }

                            else
                            {
                                exhaustedMoves = true;
                            }   
                        }

                        break;



                    case 'q':
                        
                        //vertical moves

                        //move up
                        int tempIndex = i;
                        while (tempIndex + 8 < 63)
                        {
                            if (tiles[tempIndex + 8].getPieceType() == '0')
                            {
                                legalMovesList[1].Add("Q" + tiles[tempIndex + 8].getName());
                                tiles[i].setLegalMove(true);
                            }
                            else if (tiles[tempIndex + 8].getPieceColor() != tiles[i].getPieceColor())
                            {
                                legalMovesList[1].Add("Qx" + tiles[tempIndex + 8].getName());
                                tiles[i].setLegalMove(true);
                                break;
                            }
                            else
                            {
                                break;
                            }
                            tempIndex += 8;
                        }

                        //move down
                        tempIndex = i;
                        while (tempIndex - 8 > 0)
                        {
                            if (tiles[tempIndex - 8].getPieceType() == '0')
                            {
                                legalMovesList[1].Add("Q" + tiles[tempIndex - 8].getName());
                                tiles[i].setLegalMove(true);
                            }
                            else if (tiles[tempIndex - 8].getPieceColor() != tiles[i].getPieceColor())
                            {
                                legalMovesList[1].Add("Qx" + tiles[tempIndex - 8].getName());
                                tiles[i].setLegalMove(true);
                                break;
                            }
                            else
                            {
                                break;  
                            }
                            tempIndex -= 8;
                        }

                        //horizontal moves

                        //move right
                        tempIndex = i;
                        while (tempIndex % 8 != 7)
                        {
                            if (tiles[tempIndex + 1].getPieceType() == '0')
                            {
                                legalMovesList[1].Add("Q" + tiles[tempIndex + 1].getName());
                                tiles[i].setLegalMove(true);
                            }
                            else if (tiles[tempIndex + 1].getPieceColor() != tiles[i].getPieceColor())
                            {
                                legalMovesList[1].Add("Qx" + tiles[tempIndex + 1].getName());
                                tiles[i].setLegalMove(true);
                                break;
                            }
                            else
                            {
                                break;
                            }
                            tempIndex += 1;
                        }     
                        
                        //move left
                        tempIndex = i;
                        while(tempIndex % 8 != 0)
                        {
                            if (tiles[tempIndex - 1].getPieceType() == '0')
                            {
                                legalMovesList[1].Add("Q" + tiles[tempIndex - 1].getName());
                                tiles[i].setLegalMove(true);
                            }
                            else if (tiles[tempIndex - 1].getPieceColor() != tiles[i].getPieceColor())
                            {
                                legalMovesList[1].Add("Qx" + tiles[tempIndex - 1].getName());
                                tiles[i].setLegalMove(true);
                                break;
                            }
                            else
                            {
                                break;
                            }
                            tempIndex -= 1;
                        }

                        // diagonal moves

                        // move up right
                        tempIndex = i;
                        while ((tempIndex % 8 != 7) && (tempIndex + 9 <= 63))
                        {
                            if (tiles[tempIndex + 9].getPieceType() == '0')
                            {
                                legalMovesList[1].Add("Q" + tiles[tempIndex + 9].getName());
                                tiles[i].setLegalMove(true);
                            }
                            else if (tiles[tempIndex + 9].getPieceColor() != tiles[i].getPieceColor())
                            {
                                legalMovesList[1].Add("Qx" + tiles[tempIndex + 9].getName());
                                tiles[i].setLegalMove(true);
                                break;
                            }
                            else
                            {
                                break;
                            }
                            tempIndex += 9;
                        }

                        // move up left
                        tempIndex = i;
                        while ((tempIndex % 8 != 0) && (tempIndex + 7 <= 63))
                        {
                            if (tiles[tempIndex + 7].getPieceType() == '0')
                            {
                                legalMovesList[1].Add("Q" + tiles[tempIndex + 7].getName());
                                tiles[i].setLegalMove(true);
                            }
                            else if (tiles[tempIndex + 7].getPieceColor() != tiles[i].getPieceColor())
                            {
                                legalMovesList[1].Add("Qx" + tiles[tempIndex + 7].getName());
                                tiles[i].setLegalMove(true);
                                break;
                            }
                            else
                            {
                                break;
                            }
                            tempIndex += 7;
                        }

                        // move down right
                        tempIndex = i;
                        while ((tempIndex % 8 != 7) && (tempIndex - 7 >= 0))
                        {
                            if (tiles[tempIndex - 7].getPieceType() == '0')
                            {
                                legalMovesList[1].Add("Q" + tiles[tempIndex - 7].getName());
                                tiles[i].setLegalMove(true);
                            }
                            else if (tiles[tempIndex - 7].getPieceColor() != tiles[i].getPieceColor())
                            {
                                legalMovesList[1].Add("Qx" + tiles[tempIndex - 7].getName());
                                tiles[i].setLegalMove(true);
                                break;
                            }
                            else
                            {
                                break;
                            }
                            tempIndex -= 7;
                        }

                        // move down left
                        tempIndex = i;
                        while ((tempIndex % 8 != 0) && (tempIndex - 9 >= 0))
                        {
                            if (tiles[tempIndex - 9].getPieceType() == '0')
                            {
                                legalMovesList[1].Add("Q" + tiles[tempIndex - 9].getName());
                                tiles[i].setLegalMove(true);
                            }
                            else if (tiles[tempIndex - 9].getPieceColor() != tiles[i].getPieceColor())
                            {
                                legalMovesList[1].Add("Qx" + tiles[tempIndex - 9].getName());
                                tiles[i].setLegalMove(true);
                                break;
                            }
                            else
                            {
                                break;
                            }
                            tempIndex -= 9;
                        }

                        break;



                    case 'r':

                        //vertical moves

                        //move up
                        tempIndex = i;
                        while (tempIndex + 8 < 63)
                        {
                            if (tiles[tempIndex + 8].getPieceType() == '0')
                            {
                                legalMovesList[2].Add("R" + tiles[tempIndex + 8].getName());
                                tiles[i].setLegalMove(true);
                            }
                            else if (tiles[tempIndex + 8].getPieceColor() != tiles[i].getPieceColor())
                            {
                                legalMovesList[2].Add("Rx" + tiles[tempIndex + 8].getName());
                                tiles[i].setLegalMove(true);
                                break;
                            }
                            else
                            {
                                break;
                            }
                            tempIndex += 8;
                        }

                        //move down
                        tempIndex = i;
                        while (tempIndex - 8 > 0)
                        {
                            if (tiles[tempIndex - 8].getPieceType() == '0')
                            {
                                legalMovesList[2].Add("R" + tiles[tempIndex - 8].getName());
                                tiles[i].setLegalMove(true);
                            }
                            else if (tiles[tempIndex - 8].getPieceColor() != tiles[i].getPieceColor())
                            {
                                legalMovesList[2].Add("Rx" + tiles[tempIndex - 8].getName());
                                tiles[i].setLegalMove(true);
                                break;
                            }
                            else
                            {
                                break;
                            }
                            tempIndex -= 8;
                        }

                        //horizontal moves

                        //move right
                        tempIndex = i;
                        while (tempIndex % 8 != 7)
                        {
                            if (tiles[tempIndex + 1].getPieceType() == '0')
                            {
                                legalMovesList[2].Add("R" + tiles[tempIndex + 1].getName());
                                tiles[i].setLegalMove(true);
                            }
                            else if (tiles[tempIndex + 1].getPieceColor() != tiles[i].getPieceColor())
                            {
                                legalMovesList[2].Add("Rx" + tiles[tempIndex + 1].getName());
                                tiles[i].setLegalMove(true);
                                break;
                            }
                            else
                            {
                                break;
                            }
                            tempIndex += 1;
                        }

                        //move left
                        while (tempIndex % 8 != 0)
                        {
                            if (tiles[tempIndex - 1].getPieceType() == '0')
                            {
                                legalMovesList[2].Add("R" + tiles[tempIndex - 1].getName());
                                tiles[i].setLegalMove(true);
                            }
                            else if (tiles[tempIndex - 1].getPieceColor() != tiles[i].getPieceColor())
                            {
                                legalMovesList[2].Add("Rx" + tiles[tempIndex - 1].getName());
                                tiles[i].setLegalMove(true);
                                break;
                            }
                            else
                            {
                                break;
                            }
                            tempIndex -= 1;
                        }

                        break;




                    case 'b':
                        
                       //move up right
                        tempIndex = i;
                        while ((tempIndex % 8 != 7) && (tempIndex + 9 <= 63))
                        {
                            if (tiles[tempIndex + 9].getPieceType() == '0')
                            {
                                legalMovesList[3].Add("B" + tiles[tempIndex + 9].getName());
                                tiles[i].setLegalMove(true);
                            }
                            else if (tiles[tempIndex + 9].getPieceColor() != tiles[i].getPieceColor())
                            {
                                legalMovesList[3].Add("Bx" + tiles[tempIndex + 9].getName());
                                tiles[i].setLegalMove(true);
                                break;
                            }
                            else
                            {
                                break;
                            }
                            tempIndex += 9;
                        }

                        // move up left
                        tempIndex = i;
                        while ((tempIndex % 8 != 0) && (tempIndex + 7 <= 63))
                        {
                            if (tiles[tempIndex + 7].getPieceType() == '0')
                            {
                                legalMovesList[3].Add("B" + tiles[tempIndex + 7].getName());
                                tiles[i].setLegalMove(true);
                            }
                            else if (tiles[tempIndex + 7].getPieceColor() != tiles[i].getPieceColor())
                            {
                                legalMovesList[3].Add("Bx" + tiles[tempIndex + 7].getName());
                                tiles[i].setLegalMove(true);
                                break;
                            }
                            else
                            {
                                break;
                            }
                            tempIndex += 7;
                        }

                        // move down right
                        tempIndex = i;
                        while ((tempIndex % 8 != 7) && (tempIndex - 7 >= 0))
                        {
                            if (tiles[tempIndex - 7].getPieceType() == '0')
                            {
                                legalMovesList[3].Add("B" + tiles[tempIndex - 7].getName());
                                tiles[i].setLegalMove(true);
                            }
                            else if (tiles[tempIndex - 7].getPieceColor() != tiles[i].getPieceColor())
                            {
                                legalMovesList[3].Add("Bx" + tiles[tempIndex - 7].getName());
                                tiles[i].setLegalMove(true);
                                break;
                            }
                            else
                            {
                                break;
                            }
                            tempIndex -= 7;
                        }

                        // move down left
                        tempIndex = i;
                        while ((tempIndex % 8 != 0) && (tempIndex - 9 >= 0))
                        {
                            if (tiles[tempIndex - 9].getPieceType() == '0')
                            {
                                legalMovesList[3].Add("B" + tiles[tempIndex - 9].getName());
                                tiles[i].setLegalMove(true);
                            }
                            else if (tiles[tempIndex - 9].getPieceColor() != tiles[i].getPieceColor())
                            {
                                legalMovesList[3].Add("Bx" + tiles[tempIndex - 9].getName());
                                tiles[i].setLegalMove(true);
                                break;
                            }
                            else
                            {
                                break;
                            }
                            tempIndex -= 9;
                        }

                        break;

                    case 'n':
                        break;
                    case 'p':
                        break;
                }
            }
        }

        Debug.Log("Legal moves list created");
        return legalMovesList;
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

    void createBoard()
    {

        int tileNum = 0;

        for(int file = 0; file < 8; file++)
        {
            for (int rank  = 0; rank < 8; rank++)
            {
               
                string tileName = ranks[rank] + files[file];
                Tile t = new Tile(tileNum, tileName);
                tiles[tileNum] = t;
                tileNum++;

            }
        }
    }
    void displayBoard()
    {

        foreach (Tile t in tiles)
        {
            bool isLightSquare = (t.getNum() % 8 + (int)t.getNum() / 8) % 2 != 0;

            GameObject squareColor = (isLightSquare) ? lightCol : darkCol;

            if (tiles[t.getNum()].getLegalMove()) squareColor = selectedCol;

            GameObject tile = Instantiate(squareColor, new Vector3((t.getNum() % 8) * tileSize - 4.0f, (t.getNum() / 8) * tileSize - 4.0f), Quaternion.identity);
        }
    }
}