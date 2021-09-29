using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LobbyCanvas : MonoBehaviour
{
    private string SelectedRoomName;
    public GameObject JoinRoomPanel;
    public Text RoomName;
    [SerializeField]
    private RoomLayoutGroup _roomLayoutGroup;
    private RoomLayoutGroup RoomLayoutGroup
    {
        get { return RoomLayoutGroup; }
    }
    public void OnClickJoinRoom(string roomName)
    {
        SelectedRoomName = roomName;
        RoomName.text = roomName;
        JoinRoomPanel.SetActive(true);
    }
    public void JoinRoom()
    {
        if (PhotonNetwork.JoinRoom(SelectedRoomName))
        {
            MainCanvasManager.Instance.LobbyCanvas.gameObject.SetActive(true);
        }
        else
        {
            print("Join room failed.");
        }

    }
}
