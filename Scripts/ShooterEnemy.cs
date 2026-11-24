using System.Collections;
using UnityEngine;

public class ShooterEnemy : Hazard
{
    public enum State { Idle, Aiming, Cooldown }
    public State currentState = State.Idle;

    [Header("Data & References")]
    [SerializeField] private EnemyStats stats;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    private Transform player;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
    }

    private void Update()
    {
        if (player == null || stats == null || currentState != State.Idle) return;

        if (Vector2.Distance(transform.position, player.position) <= stats.sightRange)
        {
            StartCoroutine(ShootSequence());
        }
    }

    private IEnumerator ShootSequence()
    {
        currentState = State.Aiming;
        if (sr != null) sr.color = Color.yellow;
        AudioManager.Instance?.PlayEnemyWindUp();

        yield return new WaitForSeconds(stats.windUpTime);

        if (bulletPrefab != null && firePoint != null && player != null)
        {
            Vector2 shootDirection = (player.position - firePoint.position).normalized;

            if (shootDirection.x < 0)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else if (shootDirection.x > 0)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            ProjectileBullet bulletScript = bullet.GetComponent<ProjectileBullet>();
            if (bulletScript != null)
            {
                bulletScript.Setup(shootDirection, stats);
            }
        }

        if (sr != null) sr.color = Color.white;
        currentState = State.Cooldown;

        yield return new WaitForSeconds(stats.cooldownTime);
        currentState = State.Idle;
    }
}