using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;

    private void Start()
    {
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag ("Player"))
        {
            other.GetComponent<PlayerStats>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
