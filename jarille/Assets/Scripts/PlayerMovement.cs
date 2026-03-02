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

    // controls yeah
    private PlayerControls controls;
    
    // to track the position for the followers
    public List<Vector2> positionHistory = new List<Vector2>();
    public float recordDistance = 0.1f;

    //at awake, the rigidbody and controls are setup
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();
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
        movement = input.normalized;
    }

    // the linear velocity = movement(the vector in controls) x speed(we can change)
    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
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
}