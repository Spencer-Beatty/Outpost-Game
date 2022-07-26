using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update]
    public HealthData healthData;
    private HealthController healthController;
    private Animator animator;
    public CharacterController controller;
    private CurrentAnimationState animationState;
    [SerializeField]
    private float speed = 2f;
    public float gravity = -9.81f;
    Vector3 velocity;
    //private bool isWalking = true;
    private float lastXInput = 0f;
    private float lastZInput = 0f;

    private void Start()
    {
        animationState = GetComponent<CurrentAnimationState>();
        healthController= GetComponent<HealthController>();
        animator = GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        healthController.currentState = animationState.GetCurrentState();
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        x = Mathf.Lerp(lastXInput, x, 0.1f);
        z = Mathf.Lerp(lastZInput, z, 0.1f);
        lastXInput = x;
        lastZInput = z;
        
        animator.SetFloat("xAxis", x);
        animator.SetFloat("zAxis", z);
       
        Vector3 move = transform.right * x  + transform.forward * z ;
        controller.Move(move * speed *Time.deltaTime) ;

        if (controller.isGrounded)
        {
            velocity = Vector3.zero;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        
    }

   

    
}
