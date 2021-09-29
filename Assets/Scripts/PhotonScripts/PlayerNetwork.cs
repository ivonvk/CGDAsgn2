using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNetwork : MonoBehaviour
{
    public string PlayerName;
    private PhotonView PhotonView;
    public int PlayersInGame = 0;
    public int PlayersChacraterDone = 0;
    public static PlayerNetwork Instance;

    public bool PlayersAllReady = false;

    private PlayerController CurrentPlayer;


    public int PlayersLimited = 0;

    
    public void Awake()
    {

            Instance = this;

        PhotonView = GetComponent<PhotonView>();


        if (PlayerPrefs.GetString("LocalPlayerName")=="")
        {
            PlayerPrefs.SetString("LocalPlayerName", "Player#" + UnityEngine.Random.Range(1000, 10000).ToString());
        }
        PlayerName = PlayerPrefs.GetString("LocalPlayerName");

      //  PhotonNetwork.sendRate = 60;
        //PhotonNetwork.sendRateOnSerialize = 60;

        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    private void OnSceneFinishedLoading(Scene scene,LoadSceneMode mode)
    {
        if(scene.name == "Game")
        {
            if (PhotonNetwork.isMasterClient )
                MasterLoadedGame();
            else
                NonMasterLoadedGame();
        }
    }
    public PlayerController GetCurrentPlayerController()
    {
        return CurrentPlayer;
    }
   

    private void MasterLoadedGame()
    {
        PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);
        PhotonView.RPC("RPC_LoadGameOthers", PhotonTargets.Others);
    }
    private void NonMasterLoadedGame()
    {
        PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.All,PhotonNetwork.player);
    }

    [PunRPC]
    private void RPC_LoadedGameScene(PhotonPlayer photonPlayer)
    {
       
            PlayerManager.Instance.AddPlayerStatus(photonPlayer);
        
            PlayersInGame += 1;
            Debug.Log(PhotonNetwork.countOfPlayers + ", " + PlayerManager.Instance.PlayerStatus.Count);
        
        
        if (PlayersInGame == PhotonNetwork.playerList.Length)
        {
            Debug.Log("All players are in the game scene.");
            
                PhotonView.RPC("RPC_CreatePlayer", PhotonTargets.All);
                
            
            

            for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {
                PhotonView.RPC("RPC_MasterClientSendOrderSetStatus", PhotonTargets.MasterClient, PhotonNetwork.playerList[i]);

            }


        }
    }
    [PunRPC]
    private void RPC_LoadGameOthers()
    {
        PhotonNetwork.LoadLevel(1);
    }


    public void SetPlayerCharacter(int c,int w)
    {
      
            PhotonView.RPC("RPC_MasterSetPlayerCharacter", PhotonTargets.MasterClient,PhotonNetwork.player, c,w);
        
    }

    [PunRPC]
    void RPC_MasterSetPlayerCharacter(PhotonPlayer player,int c, int w)
    {
        PlayersChacraterDone += 1;
        if (PlayersChacraterDone == PhotonNetwork.playerList.Length)
        {
            PlayersAllReady = true;
               
        }

        PhotonView.RPC("RPC_SetPlayerCharacter", PhotonTargets.All, player, c, w,PlayersChacraterDone-1);

       
    }

    [PunRPC]
    void RPC_SetPlayerCharacter(PhotonPlayer player,int c, int w,int icon)
    {
       
        CurrentPlayer.SetPlayerCharacter(player,c, w, icon);
        CurrentPlayer.transform.position = GameObject.Find("PlayerSpawnPosition").transform.position;

        //PhotonView.RPC("RPC_MasterSetPlayerCharacter", PhotonTargets.All, c, w);

    }

    public void NewHealth(PhotonPlayer photonPlayer, float health,float maxmagic)
    {
        PhotonView.RPC("RPC_NewHealth", photonPlayer, health,maxmagic);
    }
    [PunRPC]
    private void RPC_NewHealth(float health, float maxmagic)
    {
        if (CurrentPlayer == null)
            return;
        //if (health <= 0 && CurrentPlayer.gameObject != null)
            //CurrentPlayer.gameObject.tag = "PlayerDead";
    }
    [PunRPC]
    private void RPC_CreatePlayer()
    {
        PlayersLimited += 1;
        if (PlayersLimited >= PhotonNetwork.playerList.Length && PhotonNetwork.playerList.Length!=1)
            return;
        GameObject obj = (GameObject)PhotonNetwork.Instantiate(Path.Combine("Prefabs", "NewPlayer"), Vector3.up , Quaternion.identity, 0);
        // GameMaster.Instance.GameSetup();

        CurrentPlayer = obj.GetComponent<PlayerController>();

 
    }

    [PunRPC]
    void RPC_MasterClientSendOrderSetStatus(PhotonPlayer photonPlayer)
    {
        
       PhotonView.RPC("RPC_SetAllPlayerStatus",photonPlayer,photonPlayer);
    }

    [PunRPC]
    void RPC_SetAllPlayerStatus(PhotonPlayer photonPlayer)
    {
        PlayerManager.Instance.GetPlayerStatus(photonPlayer);
        Debug.Log(photonPlayer.NickName);
    }


    public void PlayerPlaySound(string s)
    {
        //if (PhotonNetwork.isMasterClient)
       // {
            PhotonView.RPC("RPC_PlayerPlaySound", PhotonTargets.All, s);
        //}
    }
    [PunRPC]
    void RPC_PlayerPlaySound(string s)
    {
        AudioMgr.GetInstance().PlayEffectAudio(s);
    }


}
