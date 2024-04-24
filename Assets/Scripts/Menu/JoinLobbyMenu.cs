using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] GameObject onlinePage;
    [SerializeField] InputField addressInput;

    private void OnEnable()
    {
        CheckersNetworkManager.ClientConnected += OnClientConnected;
    }
    private void OnDisable()
    {
        CheckersNetworkManager.ClientConnected -= OnClientConnected;
    }

    private void OnClientConnected()
    {
      onlinePage.SetActive(false);
        gameObject.SetActive(false);
        
    }


    public void Join()
    {
        NetworkManager.singleton.networkAddress = addressInput.text;
        NetworkManager.singleton.StartClient();
    }

}
