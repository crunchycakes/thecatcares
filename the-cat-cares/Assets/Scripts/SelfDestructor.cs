using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructor : MonoBehaviour
{
    [SerializeField] private float timeToLive = 5f;
    // Start is called before the first frame update
    void Start()
    {
        if (timeToLive > 0f) {
            Destroy(gameObject, timeToLive);
        }
    }

    public void DestroyNow() {
        Destroy(gameObject);
    }
}
