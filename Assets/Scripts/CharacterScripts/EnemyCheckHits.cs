using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheckHits : AbstractCheckHits
{
    private float timeAtLastHit = -0.3f;
    private float weaponLength = 1f;
    public Transform sword;
    HealthController healthController;
    private HealthController playerHealthController;
    void Start()
    {
        healthController = GetComponent<HealthController>();
        playerHealthController = GameObject.Find("Player").GetComponent<HealthController>();
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
                playerHealthController.GetHit(damage);

                timeAtLastHit = Time.realtimeSinceStartup;
            }
        }
        else
        {
            Debug.DrawRay(sword.position, sword.TransformDirection(Vector3.forward) * weaponLength, Color.white);
            
        }
    }
    public override void GetHit()
    {
        Debug.Log("Inside Get Hit");
        GetComponent<AbstractEnemyAnimationController>().TriggerStagger();
    }
}
