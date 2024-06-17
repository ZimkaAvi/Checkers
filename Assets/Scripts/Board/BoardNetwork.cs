using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardNetwork : Board
{
    private readonly SyncList<int[]> boardList = new SyncList<int[]>();

    public override IList<int[]> BoardList => boardList;
    public override event Action<Vector3> OnPieceCaptured;
    [Server]
    public override void CaptureOnBoard(Vector2Int piecePosition)
    {
        Capture(BoardList, piecePosition);
        RPCCaptureOnBoard(piecePosition);
        OnPieceCaptured?.Invoke(new Vector3(piecePosition.x, 0, piecePosition.y));
    }
    [ClientRpc]
    private void RPCCaptureOnBoard(Vector2Int piecePosition)
    {
        Capture(BoardList, piecePosition);
    }
    [Server]
    private bool TryPromotePieceOnBoard(PiecePromotionHandler promotedPiece, int x, int z)
    {
        PromotePieceOnBoard(boardList, x, z);
        RPCPromotePieceOnBoard(x, z);
        return true;
    }
    [ClientRpc]
    private void RPCPromotePieceOnBoard(int x, int z)
    {
        if(NetworkServer.active)
        {
            return;
        }
        PromotePieceOnBoard(boardList, x, z);
    }

    public override void OnStartServer()
    {
        FillBoardList(BoardList);
        PieceMovementHandlerNetwork.ServerOnPieceReachedBackline += TryPromotePieceOnBoard;
    }
    public override void OnStopServer()
    {
        PieceMovementHandlerNetwork.ServerOnPieceReachedBackline -= TryPromotePieceOnBoard;
    }
    [Server]
    public override void MoveOnBoard(Vector2Int oldPosition, Vector2Int newPosition, bool nextTurn)
    {
        MoveOnBoard(boardList, oldPosition, newPosition);
        RPCMoveOnBoard(oldPosition, newPosition, nextTurn);

    }
    [ClientRpc]
    private void RPCMoveOnBoard(Vector2Int oldPosition, Vector2Int newPosition, bool nextTurn)
    {
        if(NetworkServer.active)
        {
            return;
        }
        MoveOnBoard(boardList, oldPosition, newPosition);
        if(nextTurn)
        {
            NetworkClient.connection.identity.GetComponent<PlayerNetwork>().CMDNextTurn();
        }
    }


}
