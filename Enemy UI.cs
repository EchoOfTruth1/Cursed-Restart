using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Wandering,
    Chasing,
    Attacking
}

public class EnemyUI : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float detectionRange = 2f;
    public float attackRange = 1f;
    public float attackDamage = 100f;
    public float attackCooldown = 1f;

    [SerializeField] private float _health = 100f;
    public float Health => _health;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        MoveLeft();
        DetectPlayer();
        AttackPlayer();
    }

    private void MoveLeft()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    private void DetectPlayer()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < detectionRange)
        {
            Debug.Log("Player detected!");
            // Optional: Switch to chasing state or behavior
        }
    }

    private void AttackPlayer()
    {
        // Future: Implement attack logic based on distance and cooldown
    }
}
