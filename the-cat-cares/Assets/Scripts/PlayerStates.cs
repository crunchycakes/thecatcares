using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStates : MonoBehaviour
{
    private Rigidbody2D body;
    private PlayerMovement playerMovement;
    private SpriteRenderer sprite;
    private TMP_Text stateText;
    public enum PlayerState { sad, happy, excited };
    private PlayerState lastPlayerState = PlayerState.sad;

    private float timeSinceLastActivation = 0f;
    private float excitedActivationInterval = 1.5f;

    [SerializeField] private GameObject sadBox;
    [Tooltip("Starting state of player.")] // need to serialise it anyways
    public PlayerState playerState = PlayerState.excited;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        sprite = GetComponent<SpriteRenderer>();
        stateText = GameObject.Find("State Text").GetComponent<TMP_Text>();
        
        string playerStateStr = playerState.ToString();
        stateText.text = playerStateStr[0].ToString().ToUpper() + playerStateStr.Substring(1);
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

        // perform when switching states
        if (lastPlayerState != playerState) {
            deactivateAllStates();
            string playerStateStr = playerState.ToString();
            stateText.text = playerStateStr[0].ToString().ToUpper() + playerStateStr.Substring(1);
        }

        timeSinceLastActivation += Time.deltaTime;

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
        if (timeSinceLastActivation < excitedActivationInterval) {
            return;
        }
        float facing = 1f;
        if (sprite.flipX) {
            facing = -1f;
        }
        // destroy any boxes in scene
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        foreach (GameObject box in boxes) {
            Destroy(box);
        }
        Instantiate(sadBox, transform.position + new Vector3(0.5f * facing, 0f, 0f), Quaternion.identity);
        timeSinceLastActivation = 0f;
    }

    // floating behaviour
    private void excitedActivation() {
        body.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        timeSinceLastActivation = 0f;
    }

    private void excitedDeactivation() {
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void deactivateAllStates() {
        excitedDeactivation();
    }

    public void setPlayerState(PlayerState state) {
        playerState = state;
    }

}
