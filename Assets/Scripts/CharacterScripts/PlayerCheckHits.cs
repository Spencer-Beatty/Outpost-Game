using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckHits : AbstractCheckHits
{
    
    private float weaponLength = 1f;
    public Transform sword;
    HealthController healthController;
    private bool isAttacking = false;
    private void Start()
    {
        healthController = GetComponent<HealthController>();
    }
    
    public override void GetHit()
    {
        GetComponent<Animator>().SetTrigger("takeDamage");
    }

    public override IEnumerator Attack()
    {
        isAttacking = true;
        float currentTime = Time.realtimeSinceStartup + 3f;
        while (Time.realtimeSinceStartup < currentTime && isAttacking)
        {
            RaycastHit hit;

            if (Physics.Raycast(sword.position, sword.TransformDirection(Vector3.forward), out hit, weaponLength))
            {

                Debug.DrawRay(sword.position, sword.TransformDirection(Vector3.forward) * hit.distance, Color.red);

                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("character"))
                {

                }
                Debug.Log(hit.transform.name + "   " + hit.transform.gameObject.layer);
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("character") && !hit.transform.Equals(transform))
                {
                    float damage = healthController.healthData.damage;
                    hit.transform.GetComponent<HealthController>().GetHit(damage);
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
        Debug.Log("peace out");


    }
}

