using System.Collections;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public TextMeshProUGUI hpText;

    private bool isInvincible = false;
    private SpriteRenderer sr;

    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        UpdateUI();

        if (currentHealth <= 0)
        {
            AudioManager.Instance?.PlayDeath();
            GameOver();
        }
        else
        {
            AudioManager.Instance?.PlayHit();
            StartCoroutine(InvincibilityRoutine());
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateUI();

        AudioManager.Instance?.PlayPickup();
    }

    private void UpdateUI()
    {
        if (hpText != null)
        {
            hpText.text = "HP: " + currentHealth + "/" + maxHealth;
        }
    }

    IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        for (int i = 0; i < 10; i++)
        {
            if (sr != null) sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.15f);
        }

        if (sr != null) sr.enabled = true;
        isInvincible = false;
    }

    void GameOver()
    {
        if (SceneController.instance != null)
        {
            SceneController.instance.RestartCurrentLevel();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
        }
    }
}