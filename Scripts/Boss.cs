using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;
    public Image hpBar;
    public GameObject bossBulletPrefab;
    public Transform firePoint;

    private float attackTimer = 0f;
    public float attackInterval = 1.8f;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHPBar();
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            Attack();
            attackTimer = 0f;
        }
    }

    void Attack()
    {
        if (bossBulletPrefab != null && firePoint != null)
        {
            Instantiate(bossBulletPrefab, firePoint.position, Quaternion.identity);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHPBar();

        if (currentHealth <= 0)
        {
            SceneController.instance?.LoadNextLevel();
            Destroy(gameObject);
        }
    }

    void UpdateHPBar()
    {
        if (hpBar != null)
            hpBar.fillAmount = (float)currentHealth / maxHealth;
    }
}