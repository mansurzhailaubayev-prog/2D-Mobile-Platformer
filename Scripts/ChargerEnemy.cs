using System.Collections;
using UnityEngine;

public class ChargerEnemy : Hazard
{
    public enum State { Idle, Preparing, Charging, Cooldown }
    public State currentState = State.Idle;

    [Header("Data Asset")]
    [SerializeField] private EnemyStats stats;

    private Transform player;
    private Vector2 chargeDirection;
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
            StartCoroutine(ChargeSequence());
        }
    }

    private IEnumerator ChargeSequence()
    {
        currentState = State.Preparing;
        chargeDirection = (player.position - transform.position).normalized;
        if (sr != null) sr.color = Color.red;
        AudioManager.Instance?.PlayEnemyWindUp();

        yield return new WaitForSeconds(stats.windUpTime);

        currentState = State.Charging;
        float timer = 0f;
        while (timer < stats.chargeDuration)
        {
            transform.Translate(chargeDirection * stats.chargeSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        if (sr != null) sr.color = Color.white;
        currentState = State.Cooldown;
        yield return new WaitForSeconds(stats.cooldownTime);

        currentState = State.Idle;
    }
}