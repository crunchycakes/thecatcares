using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyObject : MonoBehaviour
{

    // TODO: dropping from platform to this platform and vice versa can break this collider

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Player"
            && collision.gameObject.GetComponent<PlayerMovement>().isGrounded().collider == collision.otherCollider)
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.name == "Player") {
            collision.gameObject.transform.SetParent(null);
        }
    }

}
