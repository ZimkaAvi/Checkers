using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceNetwork : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandleOwnerSet))] PlayerPiecesHandler owner;

    public override void OnStartServer()
    {
        owner = connectionToClient.identity.GetComponent<PlayerPiecesHandler>();
        Board.Instance.OnPieceCaptured += ServerHandlePieceCaptured;
    }
    public override void OnStopServer()
    {
        Board.Instance.OnPieceCaptured -= ServerHandlePieceCaptured;
    }

    private void HandleOwnerSet(PlayerPiecesHandler oldOwner, PlayerPiecesHandler newOwner)
    {
        transform.parent = newOwner.PiecesParent; 
    }
    [Server]
    private void ServerHandlePieceCaptured(Vector3 capturedPiecePosition)
    {
        if(capturedPiecePosition != transform.position)
        {
            return;
        }
        NetworkServer.Destroy(gameObject);
    }


}
