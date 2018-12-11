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

        NetworkServer.SpawnWithClientAuthority(bullet, connectionToClient);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    [Command]
    public void CmdRevive()
    {
        characterState.status = CharacterStatus.ALIVE;
        GetComponentInChildren<MeshRenderer>().material.color = Color.white;
        health.currentHealth = 100;

        RpcRevive();
    }

    [ClientRpc]
    void RpcRevive()
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
    }
}
