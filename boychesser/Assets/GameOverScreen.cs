using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Text Moves;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Setup(int moves) {
        gameObject.SetActive(true);
        Moves.text = moves.ToString() + "MOVES";
    }
}
