using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform sword;
    private Animator animator;
    private bool attacking;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        
    }

    
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("light");

            GetComponent<AbstractCheckHits>().StartCoroutine("Attack");

        }
        else if (Input.GetButtonDown("Fire2"))
        {
            animator.SetTrigger("heavy");
        }
        else if (Input.GetButtonDown("Fire3"))
        {
            animator.SetBool("block", true);
        }

        if (Input.GetButtonUp("Fire3"))
        {
            animator.SetBool("block", false);
        }
    }
}
