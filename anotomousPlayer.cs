using UnityEngine;

public class AutonomousPlayer : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;

    [Header("Combat Settings")]
    public float attackDamage = 10f;
    public float attackCooldown = 1f;
    public float health = 100f;
    public float maxHealth = 100f;

    [Header("AI Settings")]
    public float wanderRadius = 10f;
    public float wanderTimer = 3f;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform target;
    private Vector2 wanderTarget;
    private float wanderCooldown;
    private float lastAttackTime;

    private PlayerStats stats;

    public enum PlayerState
    {
        Idle,
        Wandering,
        Chasing,
        Attacking
    }

    public PlayerState currentState = PlayerState.Idle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        stats = GetComponent<PlayerStats>();

        SetNewWanderTarget();
    }

    void Update()
    {
        FindNearestEnemy();
        HandleStateMachine();
        UpdateAnimations();
    }

    void HandleStateMachine()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                HandleIdle();
                break;
            case PlayerState.Wandering:
                HandleWandering();
                break;
            case PlayerState.Chasing:
                HandleChasing();
                break;
            case PlayerState.Attacking:
                HandleAttacking();
                break;
        }
    }

    void HandleIdle()
    {
        rb.velocity = Vector2.zero;

        if (target != null)
        {
            currentState = PlayerState.Chasing;
        }
        else if (Time.time >= wanderCooldown)
        {
            currentState = PlayerState.Wandering;
            SetNewWanderTarget();
        }
    }

    void HandleWandering()
    {
        MoveTowards(wanderTarget);

        if (target != null)
        {
            currentState = PlayerState.Chasing;
        }
        else if (Vector2.Distance(transform.position, wanderTarget) < 0.5f)
        {
            currentState = PlayerState.Idle;
            wanderCooldown = Time.time + wanderTimer;
        }
    }

    void HandleChasing()
    {
        if (target == null)
        {
            currentState = PlayerState.Idle;
            return;
        }

        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget <= attackRange)
        {
            currentState = PlayerState.Attacking;
        }
        else
        {
            MoveTowards(target.position);
        }
    }

    void HandleAttacking()
    {
        rb.velocity = Vector2.zero;

        if (target == null)
        {
            currentState = PlayerState.Idle;
            return;
        }

        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget > attackRange)
        {
            currentState = PlayerState.Chasing;
        }
        else if (Time.time >= lastAttackTime + attackCooldown)
        {
            PerformAttack();
        }
    }

    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float nearestDistance = detectionRange;
        Transform nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        target = nearestEnemy;
    }

    void MoveTowards(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        float currentSpeed = stats ? stats.GetCurrentMoveSpeed() : moveSpeed;
        rb.velocity = direction * currentSpeed;

        // Flip sprite based on movement direction
        if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    void PerformAttack()
    {
        lastAttackTime = Time.time;

        if (target != null)
        {
            float currentDamage = stats ? stats.GetCurrentAttackDamage() : attackDamage;
            // Deal damage to enemy here
            Debug.Log($"Attacking {target.name} for {currentDamage} damage!");
        }
    }

    void SetNewWanderTarget()
    {
        Vector2 randomDirection = Random.insideUnitCircle * wanderRadius;
        wanderTarget = (Vector2)transform.position + randomDirection;
    }

    void UpdateAnimations()
    {
        if (animator == null) return;

        animator.SetBool("isMoving", rb.velocity.magnitude > 0.1f);
        animator.SetBool("isAttacking", currentState == PlayerState.Attacking);
        animator.SetBool("enemyInRange", target != null);
    }

    void PlayerDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            // Handle player death
            Debug.Log("Player has died!");
            Destroy(gameObject);
        }
    }
}
