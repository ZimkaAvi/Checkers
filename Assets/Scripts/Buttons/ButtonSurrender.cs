using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ButtonSurrender : MonoBehaviour
{
    

    public void Surrender()
    {
        if (LocalGameManager.Instance)
        {
            TurnsHandler.Instance.Surrender();
        }
        else
        {
            PlayerNetwork surrenderingPlayer = NetworkClient.connection.identity.GetComponent<PlayerNetwork>();

            List<PlayerNetwork> players = ((CheckersNetworkManager)NetworkManager.singleton).NetworkPlayers;

            foreach (PlayerNetwork player in players)
            {
                if(player != surrenderingPlayer)
                {
                    surrenderingPlayer.CMDGameOver($"Winner: {player.DisplayName}!");
                }
            }
        }
    }
}
