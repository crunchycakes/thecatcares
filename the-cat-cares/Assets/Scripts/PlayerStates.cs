using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private SpriteRenderer sprite;
    private enum PlayerState { sad, happy };

    [SerializeField] private GameObject happyBox;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        sprite = GetComponent<SpriteRenderer>();
        //playerMovement.setHappyState(true);
    }

    private void Update() {
        if (Input.GetButtonDown("Fire1")) { // this is "i", or left mouse button
            float facing = 1f;
            if (sprite.flipX) {
                facing = -1f;
            }
            Instantiate(happyBox, transform.position + new Vector3(0.5f * facing, 0f, 0f), Quaternion.identity);
        }
    }

}
