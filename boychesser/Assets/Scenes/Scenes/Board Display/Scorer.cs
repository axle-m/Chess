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

    public static double getPieceScore(Fen f){
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

        /*int whiteScore = 0;
        int blackScore = 0;
        int i = 0;
        while(curFen[i] != ' '){
            if(char.IsLetter(curFen[i])){
                if(curFen[i] == 'K'){
                    whiteScore +=90;
                    i++;
                }
                if(curFen[i] == 'Q'){
                    whiteScore +=9;
                    i++;
                }
                else if(curFen[i] == 'R'){
                    whiteScore +=5;
                    i++;
                }
                else if(curFen[i] == 'B'){
                    whiteScore +=3;
                    i++;
                }
                else if(curFen[i] == 'N'){
                    whiteScore +=3;
                    i++;
                }
                else if(curFen[i] == 'P'){
                    whiteScore +=1;
                }
                if(curFen[i] == 'k'){
                    whiteScore +=90;
                    i++;
                }
                else if(curFen[i] == 'q'){
                    blackScore +=9;
                    i++;
                }
                else if(curFen[i] == 'r'){
                    blackScore +=5;
                    i++;
                }
                else if(curFen[i] == 'b'){
                    blackScore +=3;
                    i++;
                }
                else if(curFen[i] == 'n'){
                    blackScore +=3;
                    i++;
                }
                else if(curFen[i] == 'p'){
                    blackScore +=1;
                    i++;
                }                                                                
            else{
                i++;
            }
            }
        
        return whiteScore - blackScore;*/
    

        //var fenList = new ArrayList();
        
        /* foreach (char c in fenArr)
        {
            if (char.IsLetter(c))
            {
                fenList.Add(c);
            }
        }
        foreach (char c in fenList)
        {
            if (char.IsUpper(c))
            {

            }
            else if (!char.IsUpper(c))
            {

            }
        }
        
        return 0;     THAT WAS KAI'S EDIT, BUT GABE AND MAX DIDN'T LIKE IT
        */ 
        
        /*
        int score = 0;
        if(ActiveColor != 'w' && ActiveColor != 'b')
        {
            throw new System.Exception("illegal fen string");
        }
        if(ActiveColor == 'w')
        {
            foreach (string c in curFen)
            {
                switch (c)
                {
                    case 'q':
                        score += qScore;
                        break;
                    case 'r':
                        score += rScore;
                        break;
                    case 'b':
                        score += bScore;
                        break;
                    case 'n':
                        score += nScore;
                        break;
                    case 'p':
                        score += pScore;
                        break;
                }
            }
            return ' ';
        }
        else if(ActiveColor == 'b')
        {
            return ' ';
        }
        else
        {
            return ' ';
        }
        */
}