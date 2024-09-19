using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject landingPagePanel, onlinePage, lobbyParent;

    Callback<LobbyCreated_t> lobbyCreated;
    Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    Callback<LobbyEnter_t> lobbyEntered;
    



    public static bool UseSteam
    {
        get; private set;
    } = true;

    public static CSteamID LobbyID { get; private set; }

    private void Start()
    {
        if(!UseSteam)
        {
            return;
        }
        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }


    private void OnDestroy()
    {
        if(!UseSteam)
        {
            return;
        }
        lobbyCreated?.Dispose();
        gameLobbyJoinRequested?.Dispose();
        lobbyEntered?.Dispose();

    }

    public void HostLobby()
    {
        if(UseSteam)
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 2);
        }
        else
        {
            NetworkManager.singleton.StartHost();
        }
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if(!UseSteam)
        {
            return; 
        }

        if(callback.m_eResult != EResult.k_EResultOK)
        {
            landingPagePanel.SetActive(true);
            return;
        }

        LobbyID = new CSteamID(callback.m_ulSteamIDLobby);

        SteamMatchmaking.SetLobbyData(
           LobbyID,
           "hostAddress",
           SteamUser.GetSteamID().ToString()
            );

        NetworkManager.singleton.StartHost();
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        if (!UseSteam)
        {
            return;
        }
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (!UseSteam || NetworkServer.active )
        {
            return;
        }
        LobbyID = new CSteamID(callback.m_ulSteamIDLobby);

        string hostAddress = SteamMatchmaking.GetLobbyData(LobbyID, "hostAddress");

        NetworkManager.singleton.networkAddress = hostAddress;

        NetworkManager.singleton.StartClient();

        landingPagePanel.SetActive(false);
        onlinePage.SetActive(false);

        

    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
            Application.Quit();
        #endif
    }
}
