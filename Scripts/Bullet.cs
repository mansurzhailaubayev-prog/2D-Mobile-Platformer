using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 12f;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = transform.right * speed;
        }
        Destroy(gameObject, 3f); // Удаляем через 3 секунды, если ни во что не попала
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Попадание в Босса
        Boss boss = other.GetComponent<Boss>();
        if (boss != null)
        {
            boss.TakeDamage(1);
            Destroy(gameObject);
            return;
        }

        // Попадание в Врага или Землю/Препятствие
        if (other.CompareTag("Hazard") || other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}