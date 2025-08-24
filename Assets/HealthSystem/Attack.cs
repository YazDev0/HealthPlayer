using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public HealthSystem HS;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "damage")
        {
            HS.takeDamage(10);
        }
        if (other.gameObject.tag == "heal")
        {
            HS.TakeHeal(10);
        }
    }
}  

