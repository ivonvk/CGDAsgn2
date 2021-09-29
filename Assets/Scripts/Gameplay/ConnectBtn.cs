using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectBtn : MonoBehaviour
{
    public LobbyNetwork lobby;

    public void ConnectServerOnClick()
    {
        lobby.ConnectToSever();
    }
}
