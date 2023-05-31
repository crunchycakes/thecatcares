using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StressedScientist : MonoBehaviour
{
    
    private Animator anim;
    [SerializeField] private TMP_Text finishText;

    private void Start() {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name == "Player") {
            anim.SetBool("stressed", false);
            Invoke("finish", 2f);
        }
    }

    private void finish() {
        finishText.gameObject.SetActive(true);
    }

}
