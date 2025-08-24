using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float health;
    public float maxHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = 100;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void takeDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            die();
        }

    }
    public void TakeHeal(float heal)
    {
        health += heal;

    }
    void die()
    {
        Time.timeScale = 0f;
    }
}
