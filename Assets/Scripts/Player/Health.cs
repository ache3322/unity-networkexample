using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Health : NetworkBehaviour
{
    public const float maxHealth = 100;
    [SyncVar(hook = "OnChangeHealth")]
    public float currentHealth = maxHealth;

    public Slider healthBar;

    public void TakeDamage(float amount)
    {
        if (!isServer)
        {
            return;
        }

        currentHealth -= amount;
        if (currentHealth <= 0.0f)
        {
            currentHealth = 0.0f;
            Debug.Log("Dead!");
        }
    }

    public float CalculateHealth()
    {
        return currentHealth / maxHealth;
    }

    void OnChangeHealth(float health)
    {
        healthBar.value = CalculateHealth();
    }
}
