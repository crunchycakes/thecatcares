using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D body;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    private CompositeCollider2D platformCollider;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    private float dirY = 0f;
    private float timeSinceLastDown = -1f;
    [Tooltip("Length of time to ignore platform collision after pressing down.")]
    [SerializeField] private float platformFallInterval = 0.2f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    private enum MovementState { idle, running, jumping, falling };

    // Start is called before the first frame update
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        platformCollider = GameObject.Find("Platform").GetComponent<CompositeCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(dirX * moveSpeed, body.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded()) {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }

        dirY = Input.GetAxisRaw("Vertical");

        if (dirY < 0f && isGrounded()) {
            Physics2D.IgnoreCollision(coll, platformCollider, true);
            timeSinceLastDown = 0f;
        } else if (timeSinceLastDown >= 0f) {
            timeSinceLastDown += Time.deltaTime;
            if (timeSinceLastDown >= platformFallInterval) {
                timeSinceLastDown = -1f;
                Physics2D.IgnoreCollision(coll, platformCollider, false);
            }
        }

        UpdateAnimationState();
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

    private bool isGrounded() {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

}
