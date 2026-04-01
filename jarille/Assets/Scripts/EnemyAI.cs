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

    public EnemyCombat combatData;

    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        wanderTimer = wanderInterval;
    }

    void Update()
    {

        if (CombatManager.Instance != null &&
    (CombatManager.Instance.isInCombat || CombatManager.Instance.isPaused))
        {
            movement = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
            return;
        }
        if (CombatManager.Instance != null && CombatManager.Instance.isInCombat)
        {
            movement = Vector2.zero;
            return;
        }
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
        if (DialogueManager.Instance != null && DialogueManager.Instance.isDialogueActive)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        if (CombatManager.Instance != null && CombatManager.Instance.isInCombat)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        Vector2 nextPos = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        
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

    bool hasTriggered = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CombatManager.Instance.StartCombat(this, combatData);
        }
    }

    //void StartCombat()
    ///{
    //   CombatManager.Instance.StartCombat();
    //  gameObject.SetActive(false);
    //  }
}