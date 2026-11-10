using System.Collections;
using UnityEngine;

public class ChargerEnemy : Hazard
{
    public enum State { Idle, Preparing, Charging, Cooldown }
    [Header("Current State")]
    public State currentState = State.Idle;

    [Header("Settings")]
    [SerializeField] private float sightRange = 7f;
    [SerializeField] private float windUpTime = 0.7f;
    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private float chargeDuration = 0.5f;
    [SerializeField] private float cooldownTime = 2f;

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
        if (player == null || currentState != State.Idle) return;

        if (Vector2.Distance(transform.position, player.position) <= sightRange)
        {
            StartCoroutine(ChargeSequence());
        }
    }

    private IEnumerator ChargeSequence()
    {
        currentState = State.Preparing;
        chargeDirection = (player.position - transform.position).normalized;
        if (sr != null) sr.color = Color.red; // Предупреждающая вспышка

        yield return new WaitForSeconds(windUpTime);

        currentState = State.Charging;
        float timer = 0f;
        while (timer < chargeDuration)
        {
            transform.Translate(chargeDirection * chargeSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        if (sr != null) sr.color = Color.white;
        currentState = State.Cooldown;
        yield return new WaitForSeconds(cooldownTime);

        currentState = State.Idle;
    }
}