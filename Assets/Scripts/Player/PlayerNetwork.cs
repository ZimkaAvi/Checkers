using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class PlayerNetwork : Player
{
    [SyncVar(hook = nameof (ClientDisplayNameUpdated))] string displayName;


    public static event Action ClientInfoUpdated;

    public override void OnStartClient()
    {
        if(isClientOnly)
        {
            ((CheckersNetworkManager)NetworkManager.singleton).NetworkPlayers.Add(this);
        }
    }

    public override void OnStopClient()
    {
        if (isClientOnly)
        {
            ((CheckersNetworkManager)NetworkManager.singleton).NetworkPlayers.Remove(this);

            
        }

        ClientInfoUpdated?.Invoke();
    }

    public string DisplayName
    {
        get { return displayName; }

        [Server]
        set { displayName = value; }
    }

    private void ClientDisplayNameUpdated(string oldName, string newName)
    {
        ClientInfoUpdated?.Invoke();
    }
}
