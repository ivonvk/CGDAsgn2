using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameMaster : Photon.MonoBehaviour
{

    [SerializeField]
    public PhotonView PhotonView;
    public static GameMaster Instance;


    public float NextStageDelay;

    public int CurrentWaves;
    public int CurrentEnemiesLeft;
    public int CurrentLevels;

    public int CurrentSpawnedEnemies;

    public float DelayGenerate = 1.5f;

    public bool Generating = false;

    private GameObject[] GenerateEnemiesPos;

    public List<LevelData> AllLevels;

    public bool SetupDone = false;

    int genIndex = 0;

    public AudioSource resetmusic;
    public AudioSource battlemusic;


    public GameObject MapCam;
    public void Awake()
    {

        Instance = this;

        // PhotonView = GetComponent<PhotonView>();
        AllLevels = LevelEditor.GetInstance().Levels;

        PhotonNetwork.sendRate = 60;
        PhotonNetwork.sendRateOnSerialize = 60;

    }
    void Start()
    {

        if (PanelMgr.GetInstance() != null && PanelMgr.GetInstance().GetPanelFunction("CharacterSelectPanel") != null)
        {
            PanelMgr.GetInstance().GetPanelFunction("CharacterSelectPanel").HideMe();
        }

        MonoMgr.GetInstance().AddUpdateListener(SetupScene);
    }

    void OnDestroy()
    {
        MonoMgr.GetInstance().RemoveUpdateListener(GameOperation);
        MonoMgr.GetInstance().RemoveUpdateListener(SetupScene);
    }
    void SetupScene()
    {
        if (SetupDone)
        {
            return;
        }
        GenerateEnemiesPos = new GameObject[5];
        GenerateEnemiesPos[0] = transform.GetChild(0).gameObject;
        GenerateEnemiesPos[1] = transform.GetChild(1).gameObject;
        GenerateEnemiesPos[2] = transform.GetChild(2).gameObject;
        GenerateEnemiesPos[3] = transform.GetChild(3).gameObject;
        GenerateEnemiesPos[4] = transform.GetChild(4).gameObject;
        NextStageDelay = 7.5f;
        DelayGenerate = 0.1f;

        MonoMgr.GetInstance().AddUpdateListener(GameOperation);

        resetmusic.Stop();
        battlemusic.Stop();

        SetupDone = true;

    }

    void DelaySetOwnership()
    {


    }
    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {

        PhotonNetwork.SetMasterClient(PhotonNetwork.playerList[PhotonNetwork.playerList.Length - 1]);

        MonoMgr.GetInstance().RemoveUpdateListener(GameOperation);
        MonoMgr.GetInstance().RemoveUpdateListener(SetupScene);

        SetMasterUpdate();

    }
    void SetMasterUpdate()
    {
        if (PhotonNetwork.isMasterClient)
        {
            MonoMgr.GetInstance().AddUpdateListener(GameOperation);
        }

    }
    void Update()
    {

        if (NextStageDelay > 0)
        {
            if (resetmusic != null && battlemusic != null)
            {
                if (!resetmusic.isPlaying)
                {
                    resetmusic.Play();
                }
                if (battlemusic.isPlaying)
                {
                    battlemusic.Stop();
                }


            }
        }
        else
        {
            if (resetmusic != null && battlemusic != null)
            {

                if (resetmusic.isPlaying)
                {
                    resetmusic.Stop();
                }
                if (!battlemusic.isPlaying)
                {
                    battlemusic.Play();
                }
            }

        }
    }
    [PunRPC]
    void RPC_UpdateGameMsgText()
    {
        if (PanelMgr.GetInstance() != null && PanelMgr.GetInstance().GetPanelFunction("GameTopPanel") != null)
        {
            PanelMgr.GetInstance().GetPanelFunction("GameTopPanel").UpdateGameMsgText();
        }

    }
    public void GameEnd(bool win)
    {
        PhotonView.RPC("RPC_EndGame", PhotonTargets.All, win);
    }
    void GameOperation()
    {
        if (!PhotonNetwork.inRoom)
        {
            MonoMgr.GetInstance().RemoveUpdateListener(GameOperation);
            MonoMgr.GetInstance().RemoveUpdateListener(SetupScene);
        }
        if (PlayerNetwork.Instance.PlayersChacraterDone != PhotonNetwork.playerList.Length)
        {
            return;
        }

        if (CurrentLevels >= AllLevels.Count - 1)
        {
            GameEnd(true);
            PanelMgr.GetInstance().ShowOrHidePanel("GameOverPanel", true);
            return;
        }

        if (!PhotonNetwork.isMasterClient || CurrentLevels > AllLevels.Count)
        {
            return;
        }
        PhotonView.RPC("RPC_UpdateGameMsgText", PhotonTargets.All);
        if (NextStageDelay <= 0 && CurrentLevels + 1 < AllLevels.Count)
        {

            if (NextStageDelay <= 0f && CurrentEnemiesLeft <= 0 && CurrentWaves + 1 < AllLevels[CurrentLevels].WavesTotalEnemy.Count &&
                 CurrentSpawnedEnemies >= AllLevels[CurrentLevels].WavesTotalEnemy[CurrentWaves])
            {
                if (PhotonView != null)
                {
                    PhotonView.RPC("RPC_TurnOnGenerating", PhotonTargets.MasterClient, true);
                }
                CurrentSpawnedEnemies = 0;
                CurrentWaves += 1;
                NextStageDelay = 7.5f;
            }
            else if (CurrentWaves >= AllLevels[CurrentLevels].WavesTotalEnemy.Count - 1)
            {
                CurrentWaves = 0;
                CurrentLevels += 1;
                NextStageDelay = 15f;
            }
            if (PhotonView != null)
            {
                PhotonView.RPC("RPC_TurnOnGenerating", PhotonTargets.MasterClient, true);
                PhotonView.RPC("RPC_GenerateMonster", PhotonTargets.MasterClient);
            }
        }
        else
        {
            PhotonView.RPC("RPC_TurnOnGenerating", PhotonTargets.MasterClient, false);
            NextStageDelay -= 1f * Time.deltaTime;
        }
    }
    [PunRPC]
    void RPC_UpgradePanelOpenForAll()
    {
    }
    [PunRPC]
    void RPC_EndGame(bool win)
    {
        PanelMgr.GetInstance().GetPanelFunction("GameOverPanel").ShowMe();
        if (win)
        {
            PanelMgr.GetInstance().GetPanelFunction("GameOverPanel").GameEndMsg("Completed!", win);
        }
        else
        {
            PanelMgr.GetInstance().GetPanelFunction("GameOverPanel").GameEndMsg("Lose!", win);
        }
    }

    [PunRPC]
    void RPC_GenerateMonster()
    {
        if (CurrentLevels + 1 > AllLevels.Count)
            return;
        if (Generating && CurrentSpawnedEnemies < AllLevels[CurrentLevels].WavesTotalEnemy[CurrentWaves])
        {
            if (DelayGenerate > 0f)
            {
                DelayGenerate -= 1f * Time.deltaTime;
            }
            else
            {
                CurrentEnemiesLeft += 1;
                CurrentSpawnedEnemies += 1;
                DelayGenerate = 0.02f;
                Vector3 pos;
                if (genIndex + 1 < GenerateEnemiesPos.Length)
                {
                    genIndex += 1;
                }
                else
                {
                    genIndex = 0;
                }
                pos = GenerateEnemiesPos[genIndex].transform.position;
                GameObject obj = PhotonNetwork.InstantiateSceneObject(Path.Combine("Prefabs", AllLevels[CurrentLevels].Enemy[CurrentWaves]), pos, Quaternion.identity, 0, null);
            }
        }
        else if (CurrentSpawnedEnemies >= AllLevels[CurrentLevels].WavesTotalEnemy[CurrentWaves])
        {
            Generating = false;
        }
    }

    [PunRPC]
    void RPC_TurnOnGenerating(bool b)
    {
        Generating = b;
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(NextStageDelay);
            stream.SendNext(CurrentWaves);
            stream.SendNext(CurrentEnemiesLeft);
            stream.SendNext(CurrentLevels);
            //stream.SendNext(DelayGenerate);
            stream.SendNext(Generating);
            //stream.SendNext(CurrentSpawnedEnemies);
        }
        else
        {
            NextStageDelay = (float)stream.ReceiveNext();
            CurrentWaves = (int)stream.ReceiveNext();
            CurrentEnemiesLeft = (int)stream.ReceiveNext();
            CurrentLevels = (int)stream.ReceiveNext();
            //DelayGenerate = (float)stream.ReceiveNext();
            Generating = (bool)stream.ReceiveNext();
            //CurrentSpawnedEnemies = (int)stream.ReceiveNext();
        }

    }
    public void SpawnPlayerFuncObj(PhotonPlayer player, Vector3 pos, string s)
    {
        photonView.RPC("RPC_SpawnPlayerFuncObj", PhotonTargets.All, player, pos, s);
    }
    [PunRPC]
    public void RPC_SpawnPlayerFuncObj(PhotonPlayer player, Vector3 pos, string s)
    {
        if (PhotonNetwork.isMasterClient)
        {
            GameObject obj = PhotonNetwork.InstantiateSceneObject(Path.Combine("Prefabs", s), pos, Quaternion.identity, 0, null);
            if (obj.GetComponent<WeaponAttack>() != null)
            {
                obj.GetComponent<WeaponAttack>().SetWeapon(player, false);
            }
        }
    }
    public void RangedWeaponObjectShoot(PhotonPlayer player, Vector3 shootPos, Vector3 dirction, bool isplayer, bool isMagic)
    {
        photonView.RPC("RPC_RangedWeaponObjectShoot", PhotonTargets.All, player, shootPos, dirction, isplayer, true, isMagic);
    }
    [PunRPC]
    public void RPC_RangedWeaponObjectShoot(PhotonPlayer player, Vector3 shootPos, Vector3 dirction, bool isplayer, bool ranged, bool isMagic)
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (!isMagic)
            {
                GameObject obj = PhotonNetwork.InstantiateSceneObject(Path.Combine("Prefabs", "Arrow"), shootPos, Quaternion.Euler(dirction), 0, null);
                obj.GetComponent<WeaponAttack>().SetWeapon(player, ranged);
            }
            else
            {
                GameObject obj = PhotonNetwork.InstantiateSceneObject(Path.Combine("Prefabs", "MagicAtkBall"), shootPos, Quaternion.Euler(dirction), 0, null);
                obj.GetComponent<WeaponAttack>().SetWeapon(player, ranged);
            }
        }
    }
    public float GetNextStageDelay()
    {
        return NextStageDelay;
    }
}
