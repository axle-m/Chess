using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
public class Scorer : Board {
    // readonly int qScore = 9, rScore = 5, nScore = 3, bScore = 3, pScore = 1;
    // char[] wPieces = new char[] { 'Q', 'R', 'R', 'B', 'B', 'N', 'N', 'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P' };
    // char[] bPieces = new char[] { 'q', 'r', 'r', 'b', 'b', 'n', 'n', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p' };

    int[] whiteKingPos = new int[] {2, 3, 1, 0, 0, 1, 3, 2,
                                    2, 2, 0, 0, 0, 0, 2, 2,
                                    -1, -2, -2, -2, -2, -2, -2, -1,
                                    -2, -3};


    static readonly Dictionary<char, int> piece_values = new Dictionary<char, int>
    {
        { 'k', 256 },   //give king arbitrarily high value to ensure any position in which a side can capture a king is always chosen
        { 'q', 9 }, 
        { 'r', 5 }, 
        { 'b', 3 },     //bishop and knight are both 3, may change bishop to be a little higher later for accuracy
        { 'n', 3 }, 
        { 'p', 1 }
    };

    public int getPieceScore(string curFen){
        //yalls methods are dogshit, this is good - Max
        //This only takes account the amount of pieces each player has

        //my man ur code is the ass one u can do this in like 20 total lines - alex
        //also can we do something about all the extraneous commented code its hurting my eyes
        //i didnt test my code either but it works trust

        int score = 0;

        string board = curFen.Split(' ')[0];
        char[] boardCharArray = board.ToCharArray();
        foreach(char c in boardCharArray)
        {
            if (piece_values.ContainsKey(char.ToLower(c)))
            {
                int pieceScore = piece_values[c];
                if (!char.IsUpper(c)) pieceScore *= -1;

                score += pieceScore;
            }
        }

        return score;

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
    }

    

    public int getPositionScore(string curFen){
        //Not Completed, This code will take into account the placement of the piece
        //This website has the points for each position: https://www.freecodecamp.org/news/simple-chess-ai-step-by-step-1d55a9266977/
        int whiteScore = 0;
        int blackScore = 0;
        string[] parts = curFen.Split(' ');
        string[] board = parts[0].Split('/');
        for(int i = 0; i < board.Length; i++){
            for(int j = 0; j < board[i].Length; j++){
                if(char.IsLetter(board[i][j])){
                    
                }
            }
        }
        return 0;
    }
        
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

    public void blackPosition(int[] array){
        int[] black = new int[array.Length];
        for(int i = 0; i < array.Length; i++){
            for(int i = 0; i < array.Length; i++){
                black[i] = array[i] * -1;
            }
        }
    }
}