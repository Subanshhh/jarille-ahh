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

    

    public float interactDistance = 1.5f;
    private Vector2 lastMoveDirection = Vector2.down;

    Animator anim;
    

    float lastVertical = -1f; // start facing down
    //at awake, the rigidbody and controls are setup
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();
        anim = GetComponent<Animator>();
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
        if (CombatManager.Instance != null && CombatManager.Instance.isInCombat)
        {
            return; // 🚫 stop movement completely
        }
        Vector2 input = controls.Player.Move.ReadValue<Vector2>();

        if (input != Vector2.zero)
        {
            lastMoveDirection = input.normalized;
        }

        if (DialogueManager.Instance != null && DialogueManager.Instance.isDialogueActive)
        {
            movement = Vector2.zero;
            return;
        }
        movement = input.normalized;

        //animator.SetFloat("Speed", movement.magnitude);

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
        movement = UnityEngine.InputSystem.Keyboard.current != null
    ? new Vector2(
        (UnityEngine.InputSystem.Keyboard.current.dKey.isPressed ? 1 : 0) +
        (UnityEngine.InputSystem.Keyboard.current.aKey.isPressed ? -1 : 0),
        (UnityEngine.InputSystem.Keyboard.current.wKey.isPressed ? 1 : 0) +
        (UnityEngine.InputSystem.Keyboard.current.sKey.isPressed ? -1 : 0)
    )
    : Vector2.zero;

        // Save last vertical direction
        if (movement.y != 0)
        {
            lastVertical = movement.y;
        }

        // Send to animator
        anim.SetFloat("MoveX", movement.x);
        anim.SetFloat("MoveY", movement.y);
        anim.SetFloat("LastVertical", lastVertical);
        anim.SetBool("IsMoving", movement != Vector2.zero);
    }

    // the linear velocity = movement(the vector in controls) x speed(we can change)
    void FixedUpdate()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.isDialogueActive)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        if (CanMove(movement))
        {
            rb.linearVelocity = movement * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
        if (CombatManager.Instance != null && CombatManager.Instance.isInCombat)
        {
            rb.linearVelocity = Vector2.zero;
            return;
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