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

    public List<PlayerNetwork> NetworkPlayers { get; } = new();



    public static event Action ClientConnected;

    


    public override void OnClientConnect()
    {
        base.OnClientConnect();
        ClientConnected!.Invoke();
        
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        SceneManager.LoadScene("Lobby Scene");
        Destroy(gameObject);
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        PlayerNetwork player = Instantiate(playerPrefab).GetComponent<PlayerNetwork>();

        NetworkPlayers.Add(player);

        

        NetworkServer.AddPlayerForConnection(conn, player.gameObject);

        player.IsLobbyOwner = player.IsWhite = numPlayers == 1;

        player.DisplayName = player.IsWhite ? "White" : "Black";
    }


    public override void OnServerDisconnect(NetworkConnection conn)
    {
        PlayerNetwork player = conn.identity.GetComponent<PlayerNetwork>();

        NetworkPlayers.Remove(player);

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        NetworkPlayers.Clear();
    }
}
