using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterState : NetworkBehaviour
{
    [SyncVar(hook = "OnChangeStatus")]
    public CharacterStatus status = CharacterStatus.ALIVE;


    void OnChangeStatus(CharacterStatus status)
    {
        DevLog.Log("CharacterState", "Player id <" + GetComponent<NetworkIdentity>().netId + "> status = " + status.ToString());
        this.status = status;
    }
}

public enum CharacterStatus
{
    ALIVE,
    RESCUE,
    DEAD
}