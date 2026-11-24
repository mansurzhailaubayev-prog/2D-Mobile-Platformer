using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.instance?.AddCoin();
            AudioManager.Instance?.PlayPickup();
            Destroy(gameObject);
        }
    }
}