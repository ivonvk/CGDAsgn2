using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentRoomCanvas : MonoBehaviour
{
    public bool Started = false;
    public void OnClickStartSync()
    {
        if (!PhotonNetwork.isMasterClient)
            return;


        Debug.Log(SceneManager.GetActiveScene().buildIndex + 1);
        PhotonNetwork.LoadLevel(1);
    }
    public void OnClickStartDelayed()
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        if (!Started)
        {
            Started = true;
            PhotonNetwork.room.IsOpen = false;
            PhotonNetwork.room.IsVisible = false;


            Debug.Log(SceneManager.GetActiveScene().buildIndex + 1);
            PhotonNetwork.LoadLevel(1);
        }
    }
}
