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
    public string DisplayName
    {
        get { return displayName; }

        [Server]
        set { displayName = value; }
    }

    [SyncVar(hook = nameof(AuthorityHandleLobbyOwnerStateUpdated))] bool isLobbyOwner;
    public bool IsLobbyOwner
    {
        get { return IsLobbyOwner; }

        [Server]
        set { isLobbyOwner = value; }
    }

    public static event Action ClientInfoUpdated;
    public static event Action<bool> AuthorityLobbyOwnerStateUpdated;

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


    private void ClientDisplayNameUpdated(string oldName, string newName)
    {
        ClientInfoUpdated?.Invoke();
    }

    private void AuthorityHandleLobbyOwnerStateUpdated(bool oldState, bool newState)
    {
        if (!hasAuthority) return;

        AuthorityLobbyOwnerStateUpdated?.Invoke(newState);

    }
}
