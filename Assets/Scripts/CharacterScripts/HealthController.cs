using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthController : MonoBehaviour
{
    // Start is called before the first frame update
    public HealthData healthData;
    private float maxHealth = 100;
    public string currentState;
    public Slider healthBar;
    public int hits = 0;

    void Update()
    {
            healthBar.value = healthData.health / maxHealth;
    }

    public void GetHit(float damage)
    {
        hits++;
        healthData.health -= damage;
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
