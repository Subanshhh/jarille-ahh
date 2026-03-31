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

    public LayerMask groundLayer;

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
        Vector2 nextPos = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        // Check if next position is still on ground
        Collider2D groundCheck = Physics2D.OverlapCircle(nextPos, 0.2f, groundLayer);

        if (groundCheck != null)
        {
            rb.linearVelocity = movement * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
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
            CombatManager.Instance.StartCombat(this);
        }
    }

    //void StartCombat()
    ///{
     //   CombatManager.Instance.StartCombat();
      //  gameObject.SetActive(false);
  //  }
}