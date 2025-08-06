// Create: EnemyDamage.cs
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damageAmount = 10f;
    public float damageInterval = 1f; // Damage every second while in contact
    
    private float lastDamageTime = 0f;
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= damageInterval)
            {
                PlayerStats playerStats = other.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(damageAmount);
                    lastDamageTime = Time.time;
                }
            }
        }
    }
}
