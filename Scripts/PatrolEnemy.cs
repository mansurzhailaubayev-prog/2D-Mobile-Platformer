using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PatrolEnemy : Hazard
{
    public float speed = 3f;
    public float moveDistance = 4f;
    private Vector3 startPos;
    private bool movingRight = true;

    void Start() => startPos = transform.position;

    void Update()
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
}