using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] int maxHealth = 1;
    [SerializeField] int damage = 1;

    int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    public void ChangeHealth(int changeValue)
    {
        currentHealth += changeValue;

        if (currentHealth <= 0) Destroy(gameObject);
    }
    public int GetHitValue()
    {
        return damage;
    }
}