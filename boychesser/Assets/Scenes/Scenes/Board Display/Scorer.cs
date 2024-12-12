using System.Collections;
public class Scorer : Board{
    readonly int qScore = 9, rScore = 5, nScore = 3, bScore = 3, pScore = 1;
    char[] wPieces = new char[] { 'q', 'r', 'r', 'b', 'b', 'n', 'n', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p' };
    char[] bPieces = new char[] { 'q', 'r', 'r', 'b', 'b', 'n', 'n', 'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p' };
    public int getScore(string curFen)
    {
        int score = 0;
        char[] fenArr = curFen.ToCharArray();
        var fenList = new ArrayList();
        foreach (char c in fenArr)
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
        
        return 0;
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
}