using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private Animator anim;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    // Start is called before the first frame update
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(dirX * moveSpeed, body.velocity.y);


        if (Input.GetButtonDown("Jump")) {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }

        UpdateAnimationState();
    }

    // update player animation
    private void UpdateAnimationState() {
        if (dirX > 0f) {
            anim.SetBool("running", true);
            sprite.flipX = false;
        } else if (dirX < 0f) {
            anim.SetBool("running", true);
            sprite.flipX = true;
        } else {
            anim.SetBool("running", false);
        }
    }

}
