using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header("Data & References")]
    [SerializeField] private EnemyStats stats;
    public Image hpBar;
    public GameObject bossBulletPrefab;
    public Transform firePoint;

    private int currentHealth;
    private float attackTimer = 0f;
    private Transform player;

    void Start()
    {
        currentHealth = stats != null ? stats.maxHealth : 10;

        if (hpBar == null)
        {
            GameObject barObj = GameObject.Find("BossHPBar");
            if (barObj != null)
            {
                hpBar = barObj.GetComponent<Image>();
            }
        }

        UpdateHPBar();

        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (stats == null) return;

        attackTimer += Time.deltaTime;
        if (attackTimer >= stats.cooldownTime)
        {
            Attack();
            attackTimer = 0f;
        }
    }

    void Attack()
    {
        if (bossBulletPrefab != null && firePoint != null)
        {
            Vector2 shootDirection = Vector2.left;
            if (player != null)
            {
                shootDirection = (player.position - firePoint.position).normalized;
            }

            GameObject bullet = Instantiate(bossBulletPrefab, firePoint.position, Quaternion.identity);
            ProjectileBullet bulletScript = bullet.GetComponent<ProjectileBullet>();
            if (bulletScript != null)
            {
                bulletScript.Setup(shootDirection, stats);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHPBar();

        if (currentHealth <= 0)
        {
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.UnlockLevel(4);
            }
            SceneController.instance?.LoadNextLevel();
            Destroy(gameObject);
        }
    }

    void UpdateHPBar()
    {
        if (hpBar != null && stats != null)
            hpBar.fillAmount = (float)currentHealth / stats.maxHealth;
    }
}