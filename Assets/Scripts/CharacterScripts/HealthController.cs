using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthController : MonoBehaviour
{
    // Start is called before the first frame update
    public HealthData healthData;
    private float maxHealth = 100;
    
    public Slider healthBar;
    public int hits = 0;
    private bool death = false;

    void Update()
    {
            healthBar.value = healthData.health / maxHealth;
        if(!death && healthData.health <= 0)
        {
            GetComponent<Animator>().SetTrigger("death");
            death = true;
           
        }
    }

    public void GetHit(float damage)
    {
        hits++;
        healthData.health -= damage;
        GetComponent<AbstractCheckHits>().GetHit();
       /* if (healthData.health < 0)
        {
            healthData.health = 0;
        }*/
    }

    public void SetNewHealthData(HealthData newHealthData)
    {
        healthData = newHealthData;
    }
}
