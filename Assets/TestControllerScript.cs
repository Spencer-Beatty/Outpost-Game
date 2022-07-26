using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestControllerScript : MonoBehaviour
{
    public HealthData player;
    public HealthData enemy;


    private void Start()
    {
        player.health = player.maxHealth;
        enemy.health = enemy.maxHealth;
    }
}
