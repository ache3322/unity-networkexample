using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManagerCustom : NetworkManager
{
    static public NetworkManagerCustom s_Singleton;


    //Client numPlayers from NetworkManager is always 0, so we count (throught connect/destroy in LobbyPlayer) the number
    //of players, so that even client know how many player there is.
    [HideInInspector]
    public int _playerNumber = 0;



    void Start()
    {
        s_Singleton = this;

        DontDestroyOnLoad(gameObject);
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        DevLog.Log("NetworkManagerCustom", "Someone connected to the server: " + conn.connectionId);
    }

    //allow to handle the (+) button to add/remove player
    public void OnPlayersNumberModified(int count)
    {
        _playerNumber += count;

        int localPlayerCount = 0;
        foreach (UnityEngine.Networking.PlayerController p in ClientScene.localPlayers)
            localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;
    }
}
