using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Platformer Data/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Movement & Dash")]
    public float moveSpeed = 3f;
    public float chaseSpeed = 4.5f;
    public float moveDistance = 4f;
    public float chargeSpeed = 10f;
    public float chargeDuration = 0.5f;

    [Header("Combat & Detection")]
    public int maxHealth = 10;
    public int damage = 1;
    public float sightRange = 7f;
    public float attackRange = 1.2f;
    public float windUpTime = 0.6f;
    public float cooldownTime = 1.5f;
    public float bulletSpeed = 8f;
    public float bulletLifeTime = 4f;
}