using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{

    public bool movementFlag;

    private float maxSpeed = 7f;
    private float xAxis;
    private float zAxis;
    private float oldXAxis;
    private float oldZAxis;
    private float lerpFactor = 0.01f;
    private CharacterController controller;
    private Animator animator;

    private void Start()
    {
        movementFlag = false;
        xAxis = 0f;
        zAxis = 0f;
        oldXAxis = 0f;
        oldZAxis = 0f;


        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
    }
    private void Update()
    {
        
        if (GetComponent<Unit>().checkCompletion())
        {
            zAxis = Mathf.Lerp(zAxis, 0, lerpFactor);
            oldZAxis = zAxis;

                
        }
        
        animator.SetFloat("xAxis", xAxis);

       
        animator.SetFloat("zAxis", zAxis);

    }
   
    public void MoveCharacter(float pXAxis, float pZAxis)
    {
        if(!movementFlag)
        {
            float currentSpeed = GetComponent<GladiatorController>().currentSpeed;
            float speed;

            xAxis = Mathf.Lerp(oldXAxis, pXAxis, lerpFactor);
            oldXAxis = xAxis;
            zAxis = Mathf.Lerp(oldZAxis, Mathf.Clamp(pZAxis, 0, currentSpeed / maxSpeed), lerpFactor);

            speed = maxSpeed * zAxis;
            oldZAxis = zAxis;


            controller.Move((transform.forward * zAxis + transform.right * xAxis) * Time.deltaTime * speed);
        }
    }
        
    public void FlagMoveCharacter(float pXAxis, float pZAxis, float currentSpeed)
    {
        
        

        xAxis = Mathf.Lerp(oldXAxis, pXAxis, lerpFactor);
        oldXAxis = xAxis;
        zAxis = Mathf.Lerp(oldZAxis,pZAxis, lerpFactor);

        
        oldZAxis = zAxis;

        
        
        controller.Move(transform.forward  * zAxis * Time.deltaTime * currentSpeed);
    }
    public void SetZAxis(float pZAxis)
    {
        zAxis = pZAxis;
        oldZAxis = zAxis;
    }
    private void CanMove()
    {

    }
}
