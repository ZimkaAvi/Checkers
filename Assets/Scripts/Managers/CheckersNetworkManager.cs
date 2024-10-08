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
    public List<Player> Players { get; } = new();


    public static event Action ClientConnected;
    public static event Action ServerGameStarted;

    public override void OnStartServer()
    {
        GameObject boardInstance = Instantiate(boardPrefab);
        GameObject turnhandlerInstance = Instantiate(turnsHandlerPrefab);

        NetworkServer.Spawn(boardInstance);
        NetworkServer.Spawn(turnhandlerInstance);

    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.StartsWith("Game"))
        {
            ServerGameStarted?.Invoke();
                

            GameObject gameOverhandlerInstance = Instantiate(gameOverHandlerPrefab);

            NetworkServer.Spawn(gameOverhandlerInstance);
        }
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        ClientConnected?.Invoke();
        
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

        Players.Add(player);

        NetworkServer.AddPlayerForConnection(conn, player.gameObject);

        player.IsLobbyOwner = player.IsWhite = numPlayers == 1;

        if(MainMenu.UseSteam)
        {
            CSteamID steamID = SteamMatchmaking.GetLobbyMemberByIndex(MainMenu.LobbyID, numPlayers - 1);
            player.SteamID = steamID.m_SteamID;
        }
        else
        {
            player.DisplayName = player.IsWhite ? "White" : "Black";
        }
    }


    public override void OnServerDisconnect(NetworkConnection conn)
    {
        PlayerNetwork player = conn.identity.GetComponent<PlayerNetwork>();

        NetworkPlayers.Remove(player);
        Players.Remove(player);

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        NetworkPlayers.Clear();
        Players.Clear();
    }
}
