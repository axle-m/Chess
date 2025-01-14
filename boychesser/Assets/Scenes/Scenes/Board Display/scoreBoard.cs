using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class scoreBoard : Board
{
    public TMP_Text whiteScoreBoard;
    public TMP_Text blackScoreBoard;
    public double whiteScore;
    public double blackScore;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        whiteScoreBoard.text = "White Score: " + whiteScore;
        blackScoreBoard.text = "Black Score: " + blackScore;
    }

    // Update is called once per frame
    void Update()
    {
        if(curFen.getActiveColor() == "w"){
            whiteScore = Scorer.getPieceScore(curFen);
            } 
        else{
            blackScore = Scorer.getPieceScore(curFen);
            }
    }
}
