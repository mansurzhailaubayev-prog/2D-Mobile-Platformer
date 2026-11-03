using System.Collections;
using UnityEngine;
using TMPro; // Подключаем TextMeshPro для вывода текста жизней

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public TextMeshProUGUI hpText; // Поле для текста HP на Canvas

    private bool isInvincible = false;
    private SpriteRenderer sr;
    private AudioSource audioSource;

    public AudioClip hurtClip;
    public AudioClip healClip;

    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        UpdateUI();

        if (audioSource != null && hurtClip != null)
            audioSource.PlayOneShot(hurtClip);

        if (currentHealth <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateUI();

        if (audioSource != null && healClip != null)
            audioSource.PlayOneShot(healClip);
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
        // Перезагрузка сцены через единый контроллер
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