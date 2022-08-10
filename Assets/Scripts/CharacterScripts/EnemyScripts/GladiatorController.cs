using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GladiatorController : MonoBehaviour
{
    
    public HealthData GladiatorHealthData;
  


    private Transform player;
    public Transform hand;
    public LayerMask layer;


    public EnemyMovementController movementController;
    
    
    public float walkingThreshold = 3f;
 
    private GameObject[] patrolWaypoints;
    //private int idleIndex;


   

    //for behaviour tree
    private bool playerDetected = false;

    float outOfRangeDistance = 20f;

  
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
                    
                    playerDetected = true;
                }
                
                
            } else
            {
                if (playerDetected)
                {
                    
                    GetComponent<Unit>().target = patrolWaypoints[0].transform;
                    playerDetected = false;
                }
                
            }
        
            yield return new WaitForSeconds(5f);
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
