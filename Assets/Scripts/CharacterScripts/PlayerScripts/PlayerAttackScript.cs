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
            animator.SetBool("slash", true);
            animator.SetFloat("slashDirection", Mathf.Abs(1 - animator.GetFloat("slashDirection")));
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            animator.SetBool("thrust", true);
        }
        else if (Input.GetButtonDown("Fire3"))
        {
            animator.SetBool("block", true);
        }
    }
}
