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

    public RectTransform littleHealthBar;
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
            RpcOnPlayerDead();
            DevLog.Log("Health", "Player id <" + GetComponent<NetworkIdentity>().netId + "> is dead...");
        }
    }

    void OnChangeHealth(float health)
    {
        littleHealthBar.sizeDelta = new Vector2(health, littleHealthBar.sizeDelta.y);
        healthBar.value = health / maxHealth;
    }


    [ClientRpc]
    void RpcOnPlayerDead()
    {
        // Change the color of the player to reflect their "dead" state
        GetComponentInChildren<MeshRenderer>().material.color = Color.red;

        // By setting the "isAlive" bool to false, we trigger
        // the SyncVar hook in the CharacterState
        GetComponent<CharacterState>().status = CharacterStatus.RESCUE;
    }
}
