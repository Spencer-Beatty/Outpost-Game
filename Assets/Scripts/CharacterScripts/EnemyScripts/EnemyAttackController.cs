using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    private Animator animator;
    //private int animationLayer = 0;
    private Transform player;
    EnemyMovementController movementController;
    HealthController healthController;
    //Times should be stored in a data format
    private float blockTime;
    private float lightAttackTime;
    private float heavyAttackTime;
   // private float poiseTime;
    private string currentState;
    private int upperIndex;
    private bool weaponHit =false;
    public int hits;
    private void Start()
    {
        movementController = GetComponent<EnemyMovementController>();
        healthController = GetComponent<HealthController>();
        animator = GetComponent<Animator>();
        upperIndex = animator.GetLayerIndex("Upper");
        blockTime = lightAttackTime = heavyAttackTime = 0.4f;
        
        player = GetComponent<GladiatorController>().player;
        currentState = player.GetComponentInParent<CurrentAnimationState>().GetCurrentState();
        
    }

    private void Update()
    {
        currentState = player.GetComponentInParent<CurrentAnimationState>().GetCurrentState();
        
    }

    
    public IEnumerator AssessEnemyStance()
    {
        while (true)
        {
            
            
            if (currentState.Equals("block"))
            {
                healthController.currentState = "attack";
                    heavyAttackTime = HeavyAttack();
                    yield return new WaitForSeconds(heavyAttackTime);

            }
            else if (currentState.Equals("attack"))
            {
                healthController.currentState = "block";
                blockTime = Block();
                float currentTime = Time.timeSinceLevelLoad;
                yield return MoveBackwards(blockTime/2f);
                
 
            }
            else if (currentState.Equals("idle"))
            {
                healthController.currentState = "attack";
                lightAttackTime = LightAttack();
                float decisionTime = (1 / 10) * lightAttackTime;
                lightAttackTime = lightAttackTime - decisionTime;
                //Attack cooldown
                yield return new WaitForSeconds(lightAttackTime);


            }
            else
            {
                //unexpected state
                Debug.Log("UnexpectedState");
            }
            
            yield return null;
        }
        
    }

    public float Block()
    {
        // block
        animator.SetTrigger("block");
        AnimatorStateInfo stateInfo = animator.GetNextAnimatorStateInfo(upperIndex);
        
        return stateInfo.length;
    }

    public float LightAttack()
    {
        // for now slash
        animator.SetTrigger("slash");
        AnimatorStateInfo stateInfo = animator.GetNextAnimatorStateInfo(upperIndex);
       
        return stateInfo.length;
    }

    public float HeavyAttack()
    {
        // for now thrust
        animator.SetTrigger("thrust");
        AnimatorStateInfo stateInfo =  animator.GetNextAnimatorStateInfo(upperIndex);
        return stateInfo.length;
    }
    public IEnumerator WeaponHit()
    {
        Debug.Log("sword");
        weaponHit = true;
        yield return new WaitForSeconds(0.5f);
        weaponHit = false;
    }


    public IEnumerator MoveBackwards(float blockTime)
    {
       
        if(healthController.hits > hits)
        {
            StartCoroutine("WeaponHit");
        }
        movementController.movementFlag = true;

        float currentTime = Time.timeSinceLevelLoad;
        float staggerTime = blockTime/5f;
        while (currentTime +blockTime > Time.timeSinceLevelLoad)
        {
            if(weaponHit )//&& Time.timeSinceLevelLoad - currentTime + blockTime < staggerTime)
            {
                animator.SetTrigger("stagger");
                movementController.movementFlag = false;
                yield return new WaitForSeconds(animator.GetNextAnimatorStateInfo(upperIndex).length);
                StopCoroutine("MoveBackwards");
                
                
            }
            movementController.FlagMoveCharacter(0, -0.5f, 4f);
            yield return null;
        }
        
        movementController.movementFlag = false;
    }
    
}
