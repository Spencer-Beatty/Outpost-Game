using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HealthData : UpdatableData
{
    public float damage;
    public float health;
    public float maxHealth;
   
    public float getHealth()
    {
        return health;
    }
    public float getDamage()
    {
        return damage;
    }
    

}
