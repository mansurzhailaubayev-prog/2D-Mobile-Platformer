using UnityEngine;

public class Hazard : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckHit(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckHit(other.gameObject);
    }

    private void CheckHit(GameObject obj)
    {
        if (obj.CompareTag("Player"))
        {
            Health playerHealth = obj.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }

            if (!CompareTag("Ground"))
            {
                Destroy(gameObject);
            }
        }
    }
}