using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGladiatorAnimationController : AbstractEnemyAnimationController
{
    private Animator animator;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        animator = GetComponent<Animator>();
        StartCoroutine("AssessEnemyStance");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator AssessEnemyStance()
    {
        while (true)
        {
            if (distanceTo() < 2.5f) //striking distance
            {
                float attackTime;
                int pot = Random.Range(0, 7);
                if(pot < 1)
                {
                    animator.SetTrigger("quickAttack");
                    attackTime = 3f;
                }else if(pot <= 7)
                {
                    attackTime = LightAttack();
                    
                }
                else
                {
                    attackTime = HeavyAttack();
                }
                
                float recoveryTime = attackTime / 4;
                //Attack cooldown

                yield return new WaitForSeconds(attackTime + recoveryTime);

            }
            else
            {
                animator.SetTrigger("cancelAttack");
                yield return new WaitForSeconds(0.5f);
            }


        }

    }

    public override void TriggerStagger()
    {
        float stumbleThreshold;
        float staggerThreshold = 0f;
        HeavyStagger();
        LightStagger();
        /*AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float framePercent = stateInfo.normalizedTime - Mathf.FloorToInt(stateInfo.normalizedTime);
        if (stateInfo.tagHash == Animator.StringToHash("lightAttack"))
        {
            stumbleThreshold = 0.4f;
        }
        else if (stateInfo.tagHash == Animator.StringToHash("heavyAttack"))
        {
            stumbleThreshold = 0.6f;
            staggerThreshold = 0.4f;
        }
        else
        {
            stumbleThreshold = 0;
        }
        Debug.Log(framePercent);
        if (framePercent < stumbleThreshold)
        {
            if (framePercent < staggerThreshold)
            {
                HeavyStagger();
            }
            else
            {
                LightStagger();
            }
        }*/
    }
    public override float HeavyAttack()
    {
        animator.SetTrigger("heavyAttack");
        
        return 5;
    }

    public void HeavyStagger()
    {
        animator.SetTrigger("stagger");
    }

    public override float LightAttack()
    {
        animator.SetTrigger("fastAttack");
        
        
        return 0;
    }

    public void LightStagger()
    {
        animator.SetTrigger("cancelAttack");
    }

    private float distanceTo()
    {
        Vector3 target1 = transform.position;
        Vector3 target2 = player.position;
        float x = Mathf.Max(target1.x - target2.x, target2.x - target1.x);
        float z = Mathf.Max(target1.z - target2.z, target2.z - target1.z);

        return Mathf.Sqrt(x * x + z * z);
    }
}
