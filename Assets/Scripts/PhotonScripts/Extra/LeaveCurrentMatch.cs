using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveCurrentMatch : Photon.MonoBehaviour
{
   public void OnClick_LeaveMatch()
    {
        PhotonNetwork.LeaveRoom();
        EventsCenter.GetInstance().ClearEvents();
        


 
        if (PhotonNetwork.playerList.Length <= 0)
        {
            PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
          
        }
        PlayerNetwork.Instance.PlayersInGame = 0;
        PlayerNetwork.Instance.PlayersChacraterDone = 0;
        PlayerNetwork.Instance.PlayersLimited = 0;
        PlayerNetwork.Instance.PlayersAllReady = false;
        Invoke("LoadSceneQuit", 0.2f);

    }
    void LoadSceneQuit()
    {
        if (!PhotonNetwork.inRoom)
        {
            
            PhotonNetwork.LoadLevel(0);
            PhotonNetwork.Disconnect();
            PanelMgr.GetInstance().GetPanelFunction("GameTopPanel").HideMe();
            PanelMgr.GetInstance().GetPanelFunction("GameBottomPanel").HideMe();
            PanelMgr.GetInstance().GetPanelFunction("InGameMenuPanel").HideMe();

            PanelMgr.GetInstance().GetPanelFunction("CharacterSelectPanel").HideMe();
            PanelMgr.GetInstance().GetPanelFunction("GameOverPanel").HideMe();
        }
    }


}
