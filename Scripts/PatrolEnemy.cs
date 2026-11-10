using UnityEngine;

public class PatrolEnemy : Hazard
{
    public enum State { Patrol, Chase, Attack }
    [Header("Current State")]
    public State currentState = State.Patrol;

    [Header("Movement & Distance")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float chaseSpeed = 4.5f;
    [SerializeField] private float moveDistance = 4f;

    [Header("Detection & Attack")]
    [SerializeField] private float sightRange = 6f;
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask playerMask;

    private Vector3 startPos;
    private bool movingRight = true;
    private Transform player;

    private void Start()
    {
        startPos = transform.position;
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
    }

    private void Update()
    {
        if (player == null) return;

        bool canSee = CanSeePlayer();

        switch (currentState)
        {
            case State.Patrol:
                PatrolLogic();
                if (canSee) currentState = State.Chase;
                break;

            case State.Chase:
                ChaseLogic();
                if (!canSee && Vector2.Distance(transform.position, player.position) > sightRange * 1.2f)
                {
                    currentState = State.Patrol;
                }
                else if (Vector2.Distance(transform.position, player.position) <= attackRange)
                {
                    currentState = State.Attack;
                }
                break;

            case State.Attack:
                if (Vector2.Distance(transform.position, player.position) > attackRange)
                {
                    currentState = State.Chase;
                }
                break;
        }
    }

    private bool CanSeePlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist > sightRange) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist, obstacleMask | playerMask);
        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    private void PatrolLogic()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= startPos.x + moveDistance) movingRight = false;
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= startPos.x - moveDistance) movingRight = true;
        }
    }

    private void ChaseLogic()
    {
        Vector2 target = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, chaseSpeed * Time.deltaTime);
    }
}