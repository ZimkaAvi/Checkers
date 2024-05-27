using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardNetwork : Board
{
    private readonly SyncList<int[]> boardList = new SyncList<int[]>();

    public override IList<int[]> BoardList => boardList;

    public override void OnStartServer()
    {
        FillBoardList(BoardList);
    }
    [Server]
    public override void MoveOnBoard(Vector2Int oldPosition, Vector2Int newPosition, bool nextTurn)
    {
        RPCMoveOnBoard(oldPosition, newPosition, nextTurn);

    }
    [ClientRpc]
    private void RPCMoveOnBoard(Vector2Int oldPosition, Vector2Int newPosition, bool nextTurn)
    {
        MoveOnBoard(boardList, oldPosition, newPosition);
        if(nextTurn)
        {
            NetworkClient.connection.identity.GetComponent<PlayerNetwork>().CMDNextTurn();
        }
    }


}
