using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHits : MonoBehaviour
{
    private float timeAtLastHit = -0.3f;
    private float weaponLength = 1f;
    public Transform sword;
    HealthController healthController;
    

    private void Start()
    {
        healthController = GetComponent<HealthController>();
        
    }




    void FixedUpdate()
    {
        RaycastHit hit;

        
        if (timeAtLastHit < Time.realtimeSinceStartup - 0.3f && 
            Physics.Raycast(sword.position, sword.TransformDirection(Vector3.forward), out hit, weaponLength))
        {
            Debug.DrawRay(sword.position, sword.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            Debug.Log("Did Hit");
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("character") && !hit.transform.Equals(transform))
            {
                
               
                    float damage = healthController.healthData.damage;
                    HealthController enemyHealthController = hit.transform.GetComponent<HealthController>();
                if (! enemyHealthController.currentState.Equals("block"))
                {
                    enemyHealthController.GetHit(damage);
                }
                    

                

            }
            else 
            { 
                Debug.Log(hit.transform.name);
            }


            timeAtLastHit = Time.realtimeSinceStartup;
        }
        else
        {
            Debug.DrawRay(sword.position, sword.TransformDirection(Vector3.forward) * weaponLength, Color.white);
            // Debug.Log("Did not Hit");
        }
    }
}
