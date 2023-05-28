using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    private Rigidbody2D body;
    private PlayerMovement playerMovement;
    private SpriteRenderer sprite;
    private enum PlayerState { sad, happy, excited };
    private PlayerState playerState = PlayerState.excited;
    private PlayerState lastPlayerState = PlayerState.sad;

    [SerializeField] private GameObject happyBox;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        sprite = GetComponent<SpriteRenderer>();
        //playerMovement.setHappyState(true);
    }

    private void Update() {

        // handle state changes
        // switching to happy state
        if (playerState == PlayerState.happy && lastPlayerState != PlayerState.happy) {
            playerMovement.setHappyState(true);
        } else if (playerState != PlayerState.happy && lastPlayerState == PlayerState.happy) {
            playerMovement.setHappyState(false);
        }

        // handle pressing fire key
        Fire1KeyDown();
        Fire1KeyUp();

        // record last state here i guess
        lastPlayerState = playerState;

    }

    private void Fire1KeyDown() {
        if (Input.GetButtonDown("Fire1")) { // this is "i", or left mouse button
            switch (playerState) {

                case PlayerState.happy:
                    break;

                case PlayerState.sad:
                    sadActivation();
                    break;
                
                case PlayerState.excited:
                    excitedActivation();
                    break;

            }
        }
    }

    private void Fire1KeyUp() {
        if (Input.GetButtonUp("Fire1")) {
            switch (playerState) {

                case PlayerState.happy:
                    break;

                case PlayerState.sad:
                    break;
                
                case PlayerState.excited:
                    excitedDeactivation();
                    break;

            }
        }
    }

    // spawn box
    private void sadActivation() {
        float facing = 1f;
        if (sprite.flipX) {
            facing = -1f;
        }
        Instantiate(happyBox, transform.position + new Vector3(0.5f * facing, 0f, 0f), Quaternion.identity);
    }

    // floating behaviour
    private void excitedActivation() {
        body.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    private void excitedDeactivation() {
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

}
