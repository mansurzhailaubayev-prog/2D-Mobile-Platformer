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
    private Transform player;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHPBar();

        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
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
            // Если игрок найден, рассчитываем вектор прицеливания в него, иначе стреляем влево
            Vector2 shootDirection = Vector2.left;
            if (player != null)
            {
                shootDirection = (player.position - firePoint.position).normalized;
            }

            GameObject bullet = Instantiate(bossBulletPrefab, firePoint.position, Quaternion.identity);

            // Запускаем пулю через новый класс ProjectileBullet
            ProjectileBullet bulletScript = bullet.GetComponent<ProjectileBullet>();
            if (bulletScript != null)
            {
                bulletScript.Setup(shootDirection);
            }
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