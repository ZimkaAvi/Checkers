using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckersNetworkManager : NetworkManager
{
    [SerializeField] GameObject gameOverHandlerPrefab, boardPrefab, 
        turnsHandlerPrefab;

    public static event Action ClientConnected;

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        ClientConnected!.Invoke();
        
    }
}
