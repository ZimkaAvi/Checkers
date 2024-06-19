using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class GameOverHandlerNetworked : GameOverHandler
{
    public override void OnStartServer()
    {
        TurnsHandler.Instance.OnGameOver += HandleGameOver;  
    }
    public override void OnStopServer()
    {
        TurnsHandler.Instance.OnGameOver -= HandleGameOver;
    }
    [Server]
    private void HandleGameOver(string result)
    {
        RPCGameOver(result);
    }
    [ClientRpc]
    private void RPCGameOver(string result)
    {
        CallGameOver(result);
    }

}
