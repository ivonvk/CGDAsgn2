using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyNetwork : MonoBehaviour
{
    public GameObject TitleCanvas;
  private void Start()
    {

        

    }
    public void ConnectToSever()
    {
        if (SetServerIP.Instance != null&& PhotonNetwork.PhotonServerSettings.ServerAddress!="")
        {
            if (!PhotonNetwork.connected)
            {
                Invoke("DelaySetup", 1f);
            }
        }
    }
    void DelaySetup()
    {
            PhotonNetwork.ConnectUsingSettings("0.0.0");
    }

    private void OnConnectedToMaster()
    {
        print("Connected to master.");
        TitleCanvas.SetActive(true);
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.playerName = PlayerNetwork.Instance.PlayerName;

        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnJoinedLobby()
    {
        print("Joined lobby");

    }
}
