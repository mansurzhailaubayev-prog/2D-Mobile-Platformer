using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int coins = 0;
    public TextMeshProUGUI coinsText;

    private void Awake()
    {
        instance = this;
    }

    public void AddCoin()
    {
        coins++;
        if (coinsText != null) coinsText.text = "Coins: " + coins;
    }
}