using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectionRange = 5f;

    public Transform player;

    private Rigidbody2D rb;
    private Vector2 movement;

    private float wanderTimer;
    private float wanderInterval = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        wanderTimer = wanderInterval;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            Wander();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        movement = direction;
    }

    void Wander()
    {
        wanderTimer -= Time.deltaTime;

        if (wanderTimer <= 0)
        {
            movement = Random.insideUnitCircle.normalized;
            wanderTimer = wanderInterval;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCombat();
        }
    }

    void StartCombat()
    {
        CombatManager.Instance.StartCombat();
        gameObject.SetActive(false);
    }
}