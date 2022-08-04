using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GladiatorController : MonoBehaviour
{
    
    public HealthData GladiatorHealthData;
  

    private Animator animator;
    private Transform player;
    public Transform hand;
    public LayerMask layer;


    public EnemyMovementController movementController;
    
    
    public float walkingThreshold = 3f;
 
    private GameObject[] patrolWaypoints;
    //private int idleIndex;


    private float lerpFactor = 0.01f;

    //for behaviour tree
    private bool playerDetected = false;
    float strikingDistance = 2.5f;
    float nearStrikingDistance = 5f;
    float outOfRangeDistance = 20f;

    //speeds
    float poiseSpeed = 1.5f;
    float walkingSpeed = 3.5f;
    float runningSpeed = 6;
    //float chargingSpeed = 7;

    public float currentSpeed = 7f;



   

    private void Start()
    {
        playerDetected = false;
        patrolWaypoints = new GameObject[]
        {
            new GameObject()
            
        };
        patrolWaypoints[0].transform.position = transform.position;

        player = GameObject.Find("Player").transform;
        
        animator = GetComponent<Animator>();
        
        StartCoroutine("DetectPlayer");
       
    }


    IEnumerator DetectPlayer()
    {
        while (true)
        {
            
            if (distanceTo() <= outOfRangeDistance)
            {
                
                if (!playerDetected)
                {
                    
                    GetComponent<Unit>().target = player;
                    StartCoroutine("BehaviourState");
                    playerDetected = true;
                }
                
                
            } else
            {
                if (playerDetected)
                {
                    StopCoroutine("BehaviourState");
                    GetComponent<Unit>().target = patrolWaypoints[0].transform;
                    playerDetected = false;
                }
                
            }
        
            yield return new WaitForSeconds(5f);
        }
        
        
        
    }

    private IEnumerator BehaviourState()
    {
        while (true)
        {


            
            //find a way to not setspeed every time without comparing floats

            //player is detected
            //assess enemy position
            if (distanceTo() < strikingDistance)
            {

                currentSpeed = Mathf.Lerp(currentSpeed, poiseSpeed, lerpFactor);
      

                //move at poisespeed and assess enemy stance

            }
            else if (distanceTo() < nearStrikingDistance)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, walkingSpeed, lerpFactor);
            }
            else if (distanceTo() < outOfRangeDistance)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, runningSpeed, lerpFactor);
            }
            else
            {
                currentSpeed = walkingSpeed;
            }

            yield return null;
        }
    }
   
   
    private float distanceTo()
    {
        Vector3 target1 = transform.position;
        Vector3 target2 = player.position;
        float x = Mathf.Max(target1.x - target2.x, target2.x - target1.x);
        float z = Mathf.Max(target1.z - target2.z, target2.z - target1.z);

        return Mathf.Sqrt(x*x+z*z);
    }

    
}
