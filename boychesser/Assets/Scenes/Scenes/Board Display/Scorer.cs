using System.Collections;
public class Scorer : Board{
    // readonly int qScore = 9, rScore = 5, nScore = 3, bScore = 3, pScore = 1;
    // char[] wPieces = new char[] { 'Q', 'R', 'R', 'B', 'B', 'N', 'N', 'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P' };
    // char[] bPieces = new char[] { 'q', 'r', 'r', 'b', 'b', 'n', 'n', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p' };
    public int getBasicScore(string curFen){  //yalls methods are dogshit, this is good
        int whiteScore = 0;
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
        }
        return whiteScore - blackScore;
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
}

