using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public CharacterState characterState;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    public string tmpNetworkId;

    private Health health;


    void Start()
    {
        var netId = GetComponent<NetworkIdentity>().netId;

        characterState = GetComponent<CharacterState>();
        health = GetComponent<Health>();

        Debug.Log("[PlayController.cs] The netId is: " + netId);
        if (isLocalPlayer)
        {
            Debug.Log("[PlayController.cs] LocalPlayer netId is: " + netId);
        }

        tmpNetworkId = netId.ToString();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (characterState.status != CharacterStatus.ALIVE)
        {
            // The player is considered "DEAD" and thus
            // they cannot move at all. Disable the input of the character
            return;
        }

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SoundManager.instance.PlaySound("Fire");
            CmdFire();
        }
    }

    public override void OnStartLocalPlayer()
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
    }

    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    [Command]
    public void CmdOnPlayerRescue()
    {
        // Set the status of the character to ALIVE
        characterState.status = CharacterStatus.ALIVE;
        // Restore the health of the "rescued" player to full
        // SyncVar will sync the health bar for all clients to see
        health.currentHealth = Health.maxHealth;

        GetComponentInChildren<MeshRenderer>().material.color = Color.white;

        // We got to synchronize the character's colour across the network...
        // 1. Target other clients and change the material to WHITE
        // 2. Target specific clietn and change the material to BLUE
        RpcOnPlayerRescue();
        TargetOnPlayerRescue(connectionToClient);
    }

    /// <summary>
    /// Targets all clients.
    /// When player is rescued, make changes to all client player objects.
    /// </summary>
    [ClientRpc]
    void RpcOnPlayerRescue()
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.white;
    }

    /// <summary>
    /// Targeted at a specific client player object.
    /// When player is rescued, make changes to that player object.
    /// </summary>
    /// <param name="conn">The specific client we want to RPC.</param>
    [TargetRpc]
    void TargetOnPlayerRescue(NetworkConnection conn)
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
    }
}
