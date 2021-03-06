using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CreateRoom : MonoBehaviour
{
    public MainCanvasManager mainCanvas;
    [SerializeField]
    private Text _roomName;
    private Text RoomName
    {
        get { return _roomName; }
    }

    public void OnClick_CreateRoom()
    {
        if (!PhotonNetwork.insideLobby)
            return;

        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 4 };
        if (PhotonNetwork.CreateRoom(RoomName.text,roomOptions,TypedLobby.Default))
        {
            print("create room successfully sent");
        }
        else
        {
            print("create room failed to send");
        }
    }
    private void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        print("create room failed: " + codeAndMessage[1]);
    }
    private void OnCreatedRoom()
    {
        print("Room create successfully");
        mainCanvas.OpenRoomPanel();
        transform.parent.gameObject.SetActive(false);

    }
}
