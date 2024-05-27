using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnsHandlerNetworked : TurnsHandler
{
    public override void OnStartServer()
    {
        PlayerPiecesHandler.OnPiecesSpawned += NextTurn;
        Players = LocalGameManager.Instance.Players;
    }

    public override void OnStopServer()
    {
        PlayerPiecesHandler.OnPiecesSpawned -= NextTurn;
    }

    protected override void FillMovesList()
    {
        base.FillMovesList();
        RPCGenerateMoves(piecesHandler);
    }
    [ClientRpc]
    private void RPCGenerateMoves(PlayerPiecesHandler playerPieces)
    {
        if(NetworkServer.active)
        {
            return;
        }
        GenerateMoves(playerPieces.PiecesParent);
        
    }

    
}
