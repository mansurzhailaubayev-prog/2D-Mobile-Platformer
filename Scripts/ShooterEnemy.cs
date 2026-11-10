using System.Collections;
using UnityEngine;

public class ShooterEnemy : Hazard
{
    public enum State { Idle, Aiming, Cooldown }
    [Header("Current State")]
    public State currentState = State.Idle;

    [Header("Settings")]
    [SerializeField] private float sightRange = 8f;
    [SerializeField] private float windUpTime = 0.6f;
    [SerializeField] private float cooldownTime = 1.5f;
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
        if (player == null || currentState != State.Idle) return;

        if (Vector2.Distance(transform.position, player.position) <= sightRange)
        {
            StartCoroutine(ShootSequence());
        }
    }

    private IEnumerator ShootSequence()
    {
        currentState = State.Aiming;
        if (sr != null) sr.color = Color.yellow; // Сигнал прицеливания (wind-up)

        yield return new WaitForSeconds(windUpTime);

        if (bulletPrefab != null && firePoint != null && player != null)
        {
            Vector2 shootDirection = (player.position - firePoint.position).normalized;

            // Разворот врага к игроку
            if (shootDirection.x < 0)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else if (shootDirection.x > 0)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            ProjectileBullet bulletScript = bullet.GetComponent<ProjectileBullet>();
            if (bulletScript != null)
            {
                bulletScript.Setup(shootDirection);
            }
        }

        if (sr != null) sr.color = Color.white;
        currentState = State.Cooldown;

        yield return new WaitForSeconds(cooldownTime);
        currentState = State.Idle;
    }
}