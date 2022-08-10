using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheckHits : AbstractCheckHits
{
    
    private float weaponLength = 1f;
    
    public Transform sword;
    HealthController healthController;
    private HealthController playerHealthController;
    void Start()
    {
        healthController = GetComponent<HealthController>();
        playerHealthController = GameObject.Find("Player").GetComponent<HealthController>();
    }
   
    public override IEnumerator Attack()
    {
        bool isAttacking = true;
        float currentTime = Time.realtimeSinceStartup + 3f;
        while (Time.realtimeSinceStartup < currentTime && isAttacking)
        {


            RaycastHit hit;

            if (Physics.Raycast(sword.position, sword.TransformDirection(Vector3.forward), out hit, weaponLength))
            {

                Debug.DrawRay(sword.position, sword.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                Debug.Log("Did Hit" + " : " + transform.name);


                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("character") && !hit.transform.Equals(transform))
                {

                    float damage = healthController.healthData.damage;
                    playerHealthController.GetHit(damage);
                    isAttacking = false;
                }
                else
                {
                    yield return null;
                }
            }
            else
            {
                Debug.DrawRay(sword.position, sword.TransformDirection(Vector3.forward) * weaponLength, Color.white);
                yield return null;
            }
        }
    }
    public override void GetHit()
    {
        Debug.Log("Inside Get Hit");
        
        GetComponent<AbstractEnemyAnimationController>().TriggerStagger();
    }
}
