using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Platformer Data/Player Stats")]
public class PlayerStats : ScriptableObject
{
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public int maxHealth = 3;
    public int coinValue = 10;
}