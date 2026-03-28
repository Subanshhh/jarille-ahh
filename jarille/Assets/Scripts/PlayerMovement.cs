using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class PlayerMovement : MonoBehaviour
{
    // set uhh speed in inspector
    public float moveSpeed;

    // give access to the rigidbody for movement of vector2
    private Rigidbody2D rb;
    private Vector2 movement;

    //to detect if the floor is walkable or not

    public float checkDistance = 0.5f;
    public LayerMask groundLayer;

    // controls yeah
    private PlayerControls controls;
    
    // to track the position for the followers
    public List<Vector2> positionHistory = new List<Vector2>();
    public float recordDistance = 0.1f;

    private Animator animator;

    public float interactDistance = 1.5f;
    private Vector2 lastMoveDirection = Vector2.down;

    //at awake, the rigidbody and controls are setup
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();
        animator = GetComponent<Animator>();
    }

    // controls are enabled
    void OnEnable()
    {
        controls.Enable();
    }

    // only when controls should be disabled
    void OnDisable()
    {
        controls.Disable();
    }

    // the player's move action of vector 2 should be normalized for movement
    void Update()
    {
        Vector2 input = controls.Player.Move.ReadValue<Vector2>();

        if (input != Vector2.zero)
        {
            lastMoveDirection = input.normalized;
        }

        movement = input.normalized;

        animator.SetFloat("Speed", movement.magnitude);

        if (controls.Player.Interact.triggered)
        {
            if (DialogueManager.Instance.dialoguePanel.activeSelf)
            {
                DialogueManager.Instance.ShowNextLine();
            }
            else
            {
                TryInteract();
            }
        }
    }

    // the linear velocity = movement(the vector in controls) x speed(we can change)
    void FixedUpdate()
    {
        if (CanMove(movement))
        {
            rb.linearVelocity = movement * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    //this is for the followers to move a little later
    void LateUpdate()
    {
        if (positionHistory.Count == 0 ||
            Vector2.Distance(transform.position, positionHistory[0]) > recordDistance)
        {
            positionHistory.Insert(0, transform.position);
        }
        
        if (positionHistory.Count > 100)
            positionHistory.RemoveAt(positionHistory.Count - 1);
    }
    public float interactRadius = 1.2f;
    public LayerMask interactLayer;

    void TryInteract()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRadius, interactLayer);

        if (hit != null)
        {
            Interactable interactable = hit.GetComponent<Interactable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
    bool CanMove(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return true;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            checkDistance,
            groundLayer
        );

        return hit.collider != null;
    }
}