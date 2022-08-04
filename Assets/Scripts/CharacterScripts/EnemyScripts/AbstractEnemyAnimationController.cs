using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemyAnimationController : MonoBehaviour
{
    public abstract float LightAttack();
    public abstract float HeavyAttack();
    public abstract void TriggerStagger();
}
