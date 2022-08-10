using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    //movement flag parts
    
    public bool movementFlag;

    private float maxSpeed = 7f;
    private float currentSpeed = 7f;
    private float xAxis;
    private float zAxis;
    private float oldXAxis;
    private float oldZAxis;
    private float lerpFactor = 0.01f;
    private float standingTurnSpeed = 5f;

    private Animator animator;
    private Transform player;

    

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        movementFlag = false;
        xAxis = 0f;
        zAxis = 0f;
        oldXAxis = 0f;
        oldZAxis = 0f;

        
        
        animator = GetComponent<Animator>();

        

    }
    private void Update()
    {
        
        if (GetComponent<Unit>().checkCompletion())
        {
            Quaternion targetRotation = Quaternion.LookRotation(
                   new Vector3(player.position.x, transform.position.y, player.position.z) - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * standingTurnSpeed);

            currentSpeed = 1.5f;

            zAxis = Mathf.Lerp(zAxis, 0, lerpFactor);
            oldZAxis = zAxis;
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, lerpFactor);
        }
        
        animator.SetFloat("xAxis", xAxis);

       
        animator.SetFloat("zAxis", zAxis);

    }
   
    public void MoveCharacter(float pXAxis, float pZAxis)
    {
        if(!movementFlag)
        {
            
            
            float speed;

            xAxis = Mathf.Lerp(oldXAxis, pXAxis, lerpFactor);
            oldXAxis = xAxis;
            speed = maxSpeed * zAxis;
            zAxis = Mathf.Lerp(oldZAxis, Mathf.Clamp(pZAxis, 0, currentSpeed/maxSpeed), lerpFactor);
            

            oldZAxis = zAxis;


        }
    }
    
    
    

    

   
    public void SetZAxis(float pZAxis)
    {
        zAxis = pZAxis;
        oldZAxis = zAxis;
    }
    

    
}
