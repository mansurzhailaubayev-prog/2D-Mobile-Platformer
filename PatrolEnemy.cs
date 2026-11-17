using UnityEngine;

public class PatrolEnemy : Hazard
{
    public enum State { Patrol, Chase, Attack }
    public State currentState = State.Patrol;

    [Header("Data Asset")]
    [SerializeField] private EnemyStats stats;
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
        if (player == null || stats == null) return;

        bool canSee = CanSeePlayer();

        switch (currentState)
        {
            case State.Patrol:
                PatrolLogic();
                if (canSee) currentState = State.Chase;
                break;
            case State.Chase:
                ChaseLogic();
                if (!canSee && Vector2.Distance(transform.position, player.position) > stats.sightRange * 1.2f)
                    currentState = State.Patrol;
                else if (Vector2.Distance(transform.position, player.position) <= stats.attackRange)
                    currentState = State.Attack;
                break;
            case State.Attack:
                if (Vector2.Distance(transform.position, player.position) > stats.attackRange)
                    currentState = State.Chase;
                break;
        }
    }

    private bool CanSeePlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist > stats.sightRange) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist, obstacleMask | playerMask);
        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    private void PatrolLogic()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * stats.moveSpeed * Time.deltaTime);
            if (transform.position.x >= startPos.x + stats.moveDistance) movingRight = false;
        }
        else
        {
            transform.Translate(Vector2.left * stats.moveSpeed * Time.deltaTime);
            if (transform.position.x <= startPos.x - stats.moveDistance) movingRight = true;
        }
    }

    private void ChaseLogic()
    {
        Vector2 target = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, stats.chaseSpeed * Time.deltaTime);
    }
}