using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.UIElements;
public class Scorer : Board {
    // readonly int qScore = 9, rScore = 5, nScore = 3, bScore = 3, pScore = 1;
    // char[] wPieces = new char[] { 'Q', 'R', 'R', 'B', 'B', 'N', 'N', 'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P' };
    // char[] bPieces = new char[] { 'q', 'r', 'r', 'b', 'b', 'n', 'n', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p' };
    //https://www.freecodecamp.org/news/simple-chess-ai-step-by-step-1d55a9266977/

    static double[,] whiteKingPos = new double[,] {{2, 3, 1, 0, 0, 1, 3, 2},
                                        {2, 2, 0, 0, 0, 0, 2, 2}, 
                                        {-1, -2, -2, -2, -2, -2, -2, -1}, 
                                        {-2, -3, -3, -4, -4, -3, -3, -2},
                                        {-3, -4, -4, -5, -5, -4, -4, -3},
                                        {-3, -4, -4, -5, -5, -4, -4, -3}, 
                                        {-3, -4, -4, -5, -5, -4, -4, -3},
                                        {-3, -4, -4, -5, -5, -4, -4, -3}};
    
    static double[,] blackKingPos = new double[,] {{-3, -4, -4, -5, -5, -4, -4, -3},
                                            {-3, -4, -4, -5, -5, -4, -4, -3},
                                            {-3, -4, -4, -5, -5, -4, -4, -3}, 
                                            {-3, -4, -4, -5, -5, -4, -4, -3},
                                            {-2, -3, -3, -4, -4, -3, -3, -2},
                                            {-1, -2, -2, -2, -2, -2, -2, -1},
                                            {2, 2, 0, 0, 0, 0, 2, 2},
                                            {2, 3, 1, 0, 0, 1, 3, 2},};
    
    static double[,] whiteQueenPos = new double[,] {{-2, -1, -1, -0.5, -0.5, -1, -1, -2},
                                            {-1, 0, 0.5, 0, 0, 0, 0, -1},
                                            {-1, 0.5, 0.5, 0.5, 0.5, 0.5, 0, -1},
                                            {0, 0, 0.5, 0.5, 0.5, 0.5, 0, -0.5},
                                            {-0.5, 0, 0.5, 0.5, 0.5, 0.5, 0, -0.5},
                                            {-1, 0, 0.5, 0.5, 0.5, 0.5, 0, -1},
                                            {-1, 0, 0, 0, 0, 0, 0, -1},
                                            {-2, -1, -1, -0.5, -0.5, -1, -1, -2}};

    static double[,] blackQueenPos = new double[,] {{-2, -1, -1, -0.5, -0.5, -1, -1, -2},
                                             {-1, 0, 0, 0, 0, 0, 0, -1},
                                             {-1, 0, 0.5, 0.5, 0.5, 0.5, 0, -1},
                                             {-0.5, 0, 0.5, 0.5, 0.5, 0.5, 0, -0.5},
                                             {0, 0, 0.5, 0.5, 0.5, 0.5, 0, -0.5},
                                             {-1, 0.5, 0.5, 0.5, 0.5, 0.5, 0, -1},
                                             {-1, 0, 0.5, 0, 0, 0, 0, -1},
                                             {-2, -1, -1, -0.5, -0.5, -1, -1, -2}};


    static double[,] whiteRookPos = new double[,] {{0, 0, 0, 0.5, 0.5, 0, 0,0},
                                            {-0.5, 0, 0, 0, 0, 0, 0, -0.5},
                                            {-0.5, 0, 0, 0, 0, 0, 0, -0.5},
                                            {-0.5, 0, 0, 0, 0, 0, 0, -0.5},
                                            {-0.5, 0, 0, 0, 0, 0, 0, -0.5},
                                            {-0.5, 0, 0, 0, 0, 0, 0, -0.5},
                                            {0.5, 1, 1, 1, 1, 1, 1, 0.5},
                                            {0, 0, 0, 0, 0, 0, 0, 0}};

    static double[,] blackRookPos = new double[,] {{0, 0, 0, 0, 0, 0, 0, 0},
                                            {0.5, 1, 1, 1, 1, 1, 1, 0.5},
                                            {-0.5, 0, 0, 0, 0, 0, 0, -0.5},
                                            {-0.5, 0, 0, 0, 0, 0, 0, -0.5},
                                            {-0.5, 0, 0, 0, 0, 0, 0, -0.5},
                                            {-0.5, 0, 0, 0, 0, 0, 0, -0.5},
                                            {-0.5, 0, 0, 0, 0, 0, 0, -0.5},
                                            {0, 0, 0, 0.5, 0.5, 0, 0,0}};

    static double[,] whiteBishopPos = new double[,] {{-2, -1, -1, -1, -1, -1, -1, -2},
                                              {-1, 0.5, 0, 0, 0, 0, 0.5, -1},
                                              {-1, 1, 1, 1, 1, 1, 1, -1},
                                              {-1, 0, 1, 1, 1, 1, 0, -1},
                                              {-1, 0.5, 0.5, 1, 1, 0.5, 0.5, -1},
                                              {-1, 0, 0.5, 1, 1, 0.5, 0, -1},
                                              {-1, 0, 0, 0, 0, 0, 0, -1},
                                              {-2, -1, -1, -1, -1, -1, -1, -2}};

    static double[,] blackBishopPos = new double[,] {{-2, -1, -1, -1, -1, -1, -1, -2},
                                              {-1, 0, 0, 0, 0, 0, 0, -1},
                                              {-1, 0, 0.5, 1, 1, 0.5, 0, -1},
                                              {-1, 0.5, 0.5, 1, 1, 0.5, 0.5, -1},
                                              {-1, 0, 1, 1, 1, 1, 0, -1},
                                              {-1, 1, 1, 1, 1, 1, 1, -1},
                                              {-1, 0.5, 0, 0, 0, 0, 0.5, -1},
                                              {-2, -1, -1, -1, -1, -1, -1, -2}};

    static double[,] whiteKnightPos = new double[,] {{-5, -4, -3, -3, -3, -3, -4, -5},
                                              {-4, -2, 0, 0.5, 0.5, 0, -2, -4},
                                              {-3, 0.5, 1.5, 2, 2, 1.5, 0, -3},
                                              {-3, 0.5, 1.5, 2, 2, 1.5, 0.5, -3},
                                              {-3, 0.5, 1.5, 2, 2, 1.5, 0.5, -1},
                                              {-3, 0, 1, 1.5, 1.5, 1, 0, -3},
                                              {-4, -2, 0, 0, 0, 0, -2, -4},
                                              {-5, -4, -3, -3, -3, -3, -4, -5}};

    static double[,] blackKnightPos = new double[,] {{-5, -4, -3, -3, -3, -3, -4, -5},
                                              {-4, -2, 0, 0, 0, 0, -2, -4},
                                              {-3, 0, 1, 1.5, 1.5, 1, 0, -3},
                                              {-3, 0.5, 1.5, 2, 2, 1.5, 0.5, -1},
                                              {-3, 0.5, 1.5, 2, 2, 1.5, 0.5, -3},
                                              {-3, 0.5, 1.5, 2, 2, 1.5, 0, -3},
                                              {-4, -2, 0, 0.5, 0.5, 0, -2, -4},
                                              {-5, -4, -3, -3, -3, -3, -4, -5}};

    static double[,] whitePawnPos = new double[,] {{0, 0, 0, 0, 0, 0, 0, 0},
                                            {0.5, 1, 1, -2, -2, 1, 1, 0.5},
                                            {0.5, -0.5, -1, 0, 0, -1, -0.5, 0.5},
                                            {0, 0, 0, 2, 2, 0, 0, 0},
                                            {0.5, 0.5, 1, 2.5, 2.5, 1, 0.5, 0.5},
                                            {1, 1, 2, 3, 3, 2, 1, 1},
                                            {5, 5, 5, 5, 5, 5, 5, 5},
                                            {0, 0, 0, 0, 0, 0, 0, 0}};
                                            
    static double[,] blackPawnPos = new double[,] {{0, 0, 0, 0, 0, 0, 0, 0},
                                            {5, 5, 5, 5, 5, 5, 5, 5},
                                            {1, 1, 2, 3, 3, 2, 1, 1},
                                            {0.5, 0.5, 1, 2.5, 2.5, 1, 0.5, 0.5},
                                            {0, 0, 0, 2, 2, 0, 0, 0},
                                            {0.5, -0.5, -1, 0, 0, -1, -0.5, 0.5},
                                            {0.5, 1, 1, -2, -2, 1, 1, 0.5},
                                            {0, 0, 0, 0, 0, 0, 0, 0}};



    static readonly Dictionary<char, int> piece_values = new Dictionary<char, int>
    {
        { 'k', 256 },   //give king arbitrarily high value to ensure any position in which a side can capture a king is always chosen
        { 'q', 9 }, 
        { 'r', 5 }, 
        { 'b', 3 },     //bishop and knight are both 3, may change bishop to be a little higher later for accuracy
        { 'n', 3 }, 
        { 'p', 1 }
    };

    static readonly Dictionary<char, double[,]> piecePosition = new Dictionary<char, double[,]>
    {


        { 'k', blackKingPos },   
        { 'q', blackQueenPos }, 
        { 'r', blackRookPos }, 
        { 'b', blackBishopPos },     
        { 'n', blackKnightPos }, 
        { 'p', blackPawnPos },
        { 'K', whiteKingPos },   
        { 'Q', whiteQueenPos }, 
        { 'R', whiteRookPos }, 
        { 'B', whiteBishopPos },     
        { 'N', whiteKnightPos }, 
        { 'P', whitePawnPos }
    };    
    public static bool isEndgame(Fen f) { //figures out if it's the endgame or not
    // Count the number of pieces on the board for each color
    int whitePieceCount = 0;
    int blackPieceCount = 0;

    string board = f.ToString().Split('/')[0];  // Board is the first part of the FEN
    foreach (char c in board) {
        if (char.IsUpper(c)) {
            whitePieceCount++;
        } else if (char.IsLower(c)) {
            blackPieceCount++;
        }
    }

    // If both players have fewer than 12 pieces, it's likely an endgame
    if (whitePieceCount < 12 || blackPieceCount < 12) {
        return true;
    }

    return false; // If neither condition is met, it's not considered an endgame
}
  public static double getPieceScore(Fen f) {
    double whiteScore = 0;
    double blackScore = 0;

    string board = f.ToString().Split('/')[0];
    char[] boardCharArray = board.ToCharArray();

    foreach (char c in boardCharArray) {
        if (piece_values.ContainsKey(char.ToLower(c))) {
            int pieceScore = piece_values[c];
            if (char.IsUpper(c)) {
                whiteScore += pieceScore;
            } else {
                blackScore += pieceScore;
            }
        }
    }

    // Example: Adjust score for endgame/early game.
    if (isEndgame(f)) {
        // In the endgame, kings and pawns are more important
        whiteScore += calculateKingSafety(f, "w");
        blackScore += calculateKingSafety(f, "b");
    }

    return (f.getActiveColor() == "w") ? whiteScore - blackScore : blackScore - whiteScore;
}

public static double getPositionScore(Fen f) {
    double whiteScore = 0;
    double blackScore = 0;

    string board = f.ToString().Split('/')[0];
    char[] boardCharArray = board.ToCharArray();

    for (int i = 0; i < boardCharArray.Length; i++) {
        if (piece_values.ContainsKey(boardCharArray[i])) {
            char piece = boardCharArray[i];
            double[,] positionValues = piecePosition[piece];

            int rank = i / 8;
            int file = i % 8;

            if (char.IsUpper(piece)) {
                // For white pieces, consider their position
                whiteScore += piece_values[piece] + positionValues[rank, file];
            } else {
                // For black pieces, consider their position
                blackScore += piece_values[piece] + positionValues[rank, file];
            }

            // Pawn structure evaluation (e.g., isolated or passed pawns)
            if (piece == 'p' || piece == 'P') {
                whiteScore += evaluatePawnStructure(f, "w");
                blackScore += evaluatePawnStructure(f, "b");
            }
        }
    }

    return (f.getActiveColor() == "w") ? whiteScore - blackScore : blackScore - whiteScore;
}

// Helper function to calculate the king's position
private static int getKingPosition(Fen f, string color) {
    string board = f.ToString();
    char kingChar = color == "w" ? 'K' : 'k';
    
    // Find the king's position on the board (it's in the first rank of the FEN)
    for (int i = 0; i < board.Length; i++) {
        if (board[i] == kingChar) {
            return i; // return the index of the king's position in the FEN string
        }
    }
    return -1; // King not found, handle this case as needed
}

private static double calculateKingSafety(Fen f, string color) {
    int kingPos = getKingPosition(f, color);
    double safetyScore = 0;

    if (kingPos == -1) {
        return safetyScore; // Return 0 if king not found, though this should not happen
    }

    int[] dangerousSquares = new int[] { // Example of squares that can be considered dangerous
        kingPos - 8, kingPos + 8, kingPos - 1, kingPos + 1, // Direct squares around the king
        kingPos - 7, kingPos + 7, kingPos - 9, kingPos + 9 // Diagonal squares around the king
    };

    foreach (var square in dangerousSquares) {
        if (square >= 0 && square < 64 && f.getBoard()[square] != '0') {
            // Penalize if there are opposing pieces nearby
            char piece = f.getBoard()[square];
            if ((char.IsLower(piece) && color == "w") || (char.IsUpper(piece) && color == "b")) {
                safetyScore -= 0.5; // Penalize king for being near an enemy piece
            }
        }
    }

    return safetyScore;
}

// Helper function to evaluate pawn structure (isolated, passed, etc.)
private static double evaluatePawnStructure(Fen f, string color) {
    double pawnStructureScore = 0;
    string board = f.ToString();
    
    // Check for isolated pawns
    for (int i = 0; i < board.Length; i++) {
        if (board[i] == 'p' || board[i] == 'P') {
            int rank = i / 8;
            int file = i % 8;

            bool isIsolated = true;
            if (color == "w" && i > 0 && i < board.Length - 1) {
                if ((board[i - 1] == 'P' || board[i + 1] == 'P')) {
                    isIsolated = false;
                }
            }
            if (color == "b" && i > 0 && i < board.Length - 1) {
                if ((board[i - 1] == 'p' || board[i + 1] == 'p')) {
                    isIsolated = false;
                }
            }

            if (isIsolated) {
                pawnStructureScore -= 0.5; // Isolated pawns should be penalized
            }

            // Check for passed pawns: no opposing pawns can be on the same or adjacent files
            bool isPassed = true;
            for (int j = 0; j < 8; j++) {
                if (color == "w" && (board[j * 8 + rank] == 'p' || board[j * 8 + rank] == 'P')) {
                    isPassed = false;
                }
            }
            if (isPassed) {
                pawnStructureScore += 0.5; // Passed pawns should be rewarded
            }
        }
    }

    return pawnStructureScore;
}


    /* public static double getPieceScore(Fen f){
        //This only takes account the amount of pieces each player has
        double whiteScore = 0;
        double blackScore = 0;

        string board = f.ToString().Split('/')[0];
        
        char[] boardCharArray = board.ToCharArray();
        foreach(char c in boardCharArray){
            if (piece_values.ContainsKey(char.ToLower(c))){
                int pieceScore = piece_values[c];
                if(char.IsUpper(c)) {
                    whiteScore += pieceScore;
                }
                else{
                blackScore += pieceScore;
                }
            }
        }
        return (f.getActiveColor() == "w") ? whiteScore - blackScore : blackScore - whiteScore;
    }
        
    public static double getPositionScore(Fen f){
        //Takes into account position of pieces
        double whiteScore = 0;
        double blackScore = 0;

        string board = f.ToString().Split('/')[0];
        char[] boardCharArray = board.ToCharArray();
        for(int i = 0; i < boardCharArray.Length; i++){
            if (piece_values.ContainsKey(boardCharArray[i])){
                if(char.IsUpper(boardCharArray[i])){
                    double[,] positionValues = piecePosition[boardCharArray[i]];
                    whiteScore += piece_values[boardCharArray[i]] + positionValues[(int)i/8, i%8];
                }
                else{
                    double[,] positionValues = piecePosition[boardCharArray[i]];
                    blackScore += piece_values[boardCharArray[i]] + positionValues[(int)i/8, i%8];
                }
            }
        }
        return (f.getActiveColor() == "w") ? whiteScore - blackScore : blackScore - whiteScore;
    }
    */
        
}