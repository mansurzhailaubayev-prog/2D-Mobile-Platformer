using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float cooldown = 0.3f;
    public Transform firePoint;

    private float nextShot;
    private SpriteRenderer sr;
    private AudioSource audioSource;
    public AudioClip shootClip;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Shoot()
    {
        if (Time.time < nextShot || bulletPrefab == null) return;
        nextShot = Time.time + cooldown;

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        Quaternion rotation = (sr != null && sr.flipX) ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;

        Instantiate(bulletPrefab, spawnPos, rotation);

        if (audioSource != null && shootClip != null)
        {
            audioSource.PlayOneShot(shootClip);
        }
    }
}