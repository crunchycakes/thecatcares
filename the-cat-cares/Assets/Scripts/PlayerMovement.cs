using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D body;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    private Collider2D platformCollider;
    
    private float dirX = 0f;
    private float dirY = 0f;
    private float timeSinceLastDown = -1f;
    private float coyoteTimeCounter = 0f;
    private bool IsGroundedState = true;

    [Tooltip("Grace period to allow jumping after leaving jumpable ground.")]
    [SerializeField] private float coyoteTimeLimit = 0.2f;
    [Tooltip("Length of time to ignore platform collision after pressing down.")]
    [SerializeField] private float platformFallInterval = 0.2f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private LayerMask jumpableGround;

    private enum MovementState { idle, running, jumping, falling };

    // Start is called before the first frame update
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        platformCollider = GameObject.Find("Platform").GetComponent<Collider2D>();
        IsGroundedState = isGrounded();
    }

    // Update is called once per frame
    private void Update()
    {
        IsGroundedState = isGrounded();

        MovementJump();
        MovementHorizontal();
        MovementVertical();

        UpdateAnimationState();
    }

    private void MovementJump() {
        if (IsGroundedState) {
            coyoteTimeCounter = 0f;
        } else {
            coyoteTimeCounter += Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && coyoteTimeCounter < coyoteTimeLimit) {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }
    }

    private void MovementHorizontal() {
        dirX = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(dirX * moveSpeed, body.velocity.y);
    }

    private void MovementVertical() {
        dirY = Input.GetAxisRaw("Vertical");

        if (dirY < 0f && IsGroundedState) {
            Physics2D.IgnoreCollision(coll, platformCollider, true);
            timeSinceLastDown = 0f;
        } else if (timeSinceLastDown >= 0f) {
            timeSinceLastDown += Time.deltaTime;
            if (timeSinceLastDown >= platformFallInterval) {
                timeSinceLastDown = -1f;
                Physics2D.IgnoreCollision(coll, platformCollider, false);
            }
        }
    }

    // update player animation
    private void UpdateAnimationState() {
        MovementState state;

        if (dirX > 0f) {
            state = MovementState.running;
            sprite.flipX = false;
        } else if (dirX < 0f) {
            state = MovementState.running;
            sprite.flipX = true;
        } else {
            state = MovementState.idle;
        }

        if (body.velocity.y > 0.1f) {
            state = MovementState.jumping;
        } else if (body.velocity.y < -0.1f) {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    public RaycastHit2D isGrounded() {
        Vector3 feetPos = coll.bounds.center - coll.bounds.extents.y * Vector3.up;
        return Physics2D.BoxCast(feetPos, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Platform") {
            platformCollider = collision.collider;
        }
    }

}
