using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] Button startGameButton;
    [SerializeField] Text[] playerNameTexts = new Text[2];
    

    private void OnEnable()
    {
        PlayerNetwork.ClientInfoUpdated += OnClientInfoUpdated;

        PlayerNetwork.AuthorityLobbyOwnerStateUpdated += AuthorityHandleLobbyOwnerStateUpdated;
    }


    private void OnDisable()
    {
        PlayerNetwork.ClientInfoUpdated -= OnClientInfoUpdated;

        PlayerNetwork.AuthorityLobbyOwnerStateUpdated -= AuthorityHandleLobbyOwnerStateUpdated;
    }

    private void OnClientInfoUpdated()
    {
        List<PlayerNetwork> players = ((CheckersNetworkManager)NetworkManager.singleton).NetworkPlayers;
         
        for(int i = 0; i < players.Count; i++)
        {
            playerNameTexts[i].text = players[i].DisplayName;
        }
        for(int i = players.Count; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting for player...";
        }

        startGameButton.interactable = players.Count == 2;
    }
    private void AuthorityHandleLobbyOwnerStateUpdated(bool state)
    {
 

        startGameButton.gameObject.SetActive(state);
    }

    
    public void StartGame()
    {
        NetworkManager.singleton.ServerChangeScene("Game Scene");
    }
}
