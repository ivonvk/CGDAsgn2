using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerManager : Photon.MonoBehaviour
{
    public static PlayerManager Instance;



    public List<PlayerStatus> PlayerStatus = new List<PlayerStatus>();
    public int checkDown = 0;
    public void UpdateUpgradePoint(UpgradeStatus u, PhotonPlayer photonPlayer)
    {

        UpdateStatusFunc(u, photonPlayer.ID-1);



    }
    void Awake()
    {
        //photonView = GetComponent<photonView>();
        Instance = this;
        PlayerStatus.Add(new PlayerStatus(null, 100, 100));
        PlayerStatus.Add(new PlayerStatus(null, 100, 100));
        PlayerStatus.Add(new PlayerStatus(null, 100, 100));
        PlayerStatus.Add(new PlayerStatus(null, 100, 100));
    }
    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        for (int i = 0; i < PlayerStatus.Count; i++)
        {

            if (PlayerStatus[i].isHurt)
            {
                PlayerStatus[i].HurtCounter += 1f * Time.deltaTime;
                if (PlayerStatus[i].HurtCounter > 0.5f)
                {
                    PlayerStatus[i].HurtCounter = 0;
                    if (PhotonNetwork.isMasterClient)
                    {
                        photonView.RPC("RPC_StopHurt", PhotonTargets.MasterClient, i, false);
                    }
                }
            }
            if (PlayerStatus[i].PhotonPlayer != null)
            {

                if (PlayerStatus[i].Health <= 0)
                {

                    photonView.RPC("RPC_AddCheckDown", PhotonTargets.MasterClient);

                    photonView.RPC("RPC_isDown", PhotonTargets.MasterClient, i, true);


                }
                else
                {

                    photonView.RPC("RPC_AddCheckDown", PhotonTargets.MasterClient);

                    photonView.RPC("RPC_isDown", PhotonTargets.MasterClient, i, false);

                }
            }
            /*if(PlayerStatus[i].PublicUpgradePoint < 10)
            {
                if (checkDown == PlayerStatus.Count)
                {
                    GameMaster.Instance.GameEnd(false);
                }
            }
            else
            {
                checkDown = 0;
            }*/
            ModifyHealth(PlayerStatus[i].PhotonPlayer, 0);


        }

        photonView.RPC("RPC_UpdateOtherPlayersHP", PhotonTargets.All);
    }

    [PunRPC]
    void RPC_AddCheckDown()
    {

        if (PlayerStatus[0].isDown && PlayerStatus[1].isDown && PlayerStatus[2].isDown && PlayerStatus[3].isDown && PlayerStatus[0].PublicUpgradePoint < 10)
        {
            GameMaster.Instance.GameEnd(false);
        }

    }
    [PunRPC]
    void RPC_isDown(int index, bool down)
    {

        PlayerStatus[index].isDown = down;

    }
    [PunRPC]
    void RPC_StopHurt(int index, bool hurt)
    {


            PlayerStatus[index].isHurt = false;
        


    }


    public void AddPlayerStatus(PhotonPlayer photonPlayer)
    {

        photonView.RPC("RPC_AddPlayerStatusSync", PhotonTargets.All, photonPlayer);

    }
    [PunRPC]
    void RPC_AddPlayerStatusSync(PhotonPlayer photonPlayer)
    {

       
            PlayerStatus[0].PhotonPlayer = PhotonNetwork.playerList[0];
            if (PhotonNetwork.playerList.Length >= 2)
            {
                PlayerStatus[1].PhotonPlayer = PhotonNetwork.playerList[1];
                if (PhotonNetwork.playerList.Length >= 3)
                {
                    PlayerStatus[2].PhotonPlayer = PhotonNetwork.playerList[2];
                    if (PhotonNetwork.playerList.Length >= 4)
                    {
                        PlayerStatus[3].PhotonPlayer = PhotonNetwork.playerList[3];
                    }
                }
            }
        
        PlayerStatus[0].Health = 150;
        PlayerStatus[1].Health = 150;
        PlayerStatus[2].Health = 150;
        PlayerStatus[3].Health = 150;
        PlayerStatus[0].MaxHP = 150;
        PlayerStatus[1].MaxHP = 150;
        PlayerStatus[2].MaxHP = 150;
        PlayerStatus[3].MaxHP = 150;
        photonView.RPC("RPC_SetupAllStatus", PhotonTargets.All);
    }
    [PunRPC]
    void RPC_SetupAllStatus()
    {
        photonView.RPC("RPC_UpdateOtherPlayersHP", PhotonTargets.All);
    }
    public void UpgradePlayerStatus(UpgradeStatus upgrade)
    {



        photonView.RPC("RPC_UpgradePlayerStatus",

               PhotonTargets.All,

               upgrade.MaxHealth,

               upgrade.MaxMagic,

               upgrade.Damage,

               upgrade.AttackSpeed,

               upgrade.WalkSpeed);


        //PlayerNetwork.Instance.NewHealth(PlayerStatus[i].PhotonPlayer, PlayerStatus[i].Health, PlayerStatus[i].Magic);





    }
    [PunRPC]
    void RPC_UpgradePlayerStatus(int hp, int mp, int dmg, float atkspeed, float walkspeed)
    {
        for (int x = 0; x < PlayerStatus.Count; x++)
        {
            PlayerStatus[x].MaxHP += hp;
            PlayerStatus[x].MaxMP += mp;
            PlayerStatus[x].Damage += dmg;
            PlayerStatus[x].AttackSpeed += atkspeed;
            PlayerStatus[x].MoveSpeed += walkspeed;
        }



    }
    public void ModifyHealth(PhotonPlayer photonPlayer, int value)
    {
        if (photonPlayer == null)
            return;
        if (PhotonNetwork.isMasterClient)
        {
            photonView.RPC("RPC_ReducePlayerHP", PhotonTargets.All, photonPlayer, value, true);

        }
    }
    [PunRPC]
    void RPC_ReducePlayerHP(PhotonPlayer photonPlayer, int value, bool hurt)
    {
        if (photonPlayer == null)
            return;
        if (value < 0)
        {
            PlayerStatus[photonPlayer.ID - 1].isHurt = hurt;
        }
        if (PlayerStatus[photonPlayer.ID - 1].Health + value > PlayerStatus[photonPlayer.ID - 1].MaxHP)
        {
            PlayerStatus[photonPlayer.ID - 1].Health = PlayerStatus[photonPlayer.ID - 1].MaxHP;
        }
        else
        {
            PlayerStatus[photonPlayer.ID - 1].Health += value;
        }
        if (PlayerStatus[photonPlayer.ID - 1].Health <= 0)
        {

            PlayerStatus[photonPlayer.ID - 1].isDown = true;


        }
        else if (PlayerStatus[photonPlayer.ID - 1].Health > 0)
        {
            PlayerStatus[photonPlayer.ID - 1].isDown = false;


        }


    }
    [PunRPC]
    void RPC_UpdateOtherPlayersHP()
    {
        if (PanelMgr.GetInstance() != null && PanelMgr.GetInstance().GetPanelFunction("GameTopPanel") != null)
        {
            if (!PlayerStatus[0].isDown)
            {
                photonView.RPC("RPC_UpdateOtherPlayersHPSecond", PhotonTargets.All, 0, (float)PlayerStatus[0].Health / (float)PlayerStatus[0].MaxHP);

            }
            if (!PlayerStatus[1].isDown)
            {
                photonView.RPC("RPC_UpdateOtherPlayersHPSecond", PhotonTargets.All, 1, (float)PlayerStatus[1].Health / (float)PlayerStatus[1].MaxHP);
            }
            if (!PlayerStatus[2].isDown)
            {
                photonView.RPC("RPC_UpdateOtherPlayersHPSecond", PhotonTargets.All, 2, (float)PlayerStatus[2].Health / (float)PlayerStatus[2].MaxHP);
            }
            if (!PlayerStatus[3].isDown)
            {
                photonView.RPC("RPC_UpdateOtherPlayersHPSecond", PhotonTargets.All, 3, (float)PlayerStatus[3].Health / (float)PlayerStatus[3].MaxHP);
            }

        }
    }
    [PunRPC]
    void RPC_UpdateOtherPlayersHPSecond(int i, float f)
    {
        if (PanelMgr.GetInstance() != null && PanelMgr.GetInstance().GetPanelFunction("GameTopPanel") != null)
        {
            PanelMgr.GetInstance().GetPanelFunction("GameTopPanel").UpdateTeammatesDisplay(i, f, true);
        }
    }

    [PunRPC]
    void RPC_UpdateAllPlayerHP(int value)
    {

        GetPlayerStatus(photonView.owner).Health = value;

    }
    public PlayerStatus GetPlayerStatus(PhotonPlayer photonPlayer)
    {
        if (photonPlayer != null)
        {
            return PlayerStatus[photonPlayer.ID - 1];
        }


        return null;
    }
    public void UpdateStatusFunc(UpgradeStatus upgrade, int id)
    {

        if (upgrade.GetPublicPoint)
        {

            photonView.RPC("RPC_IncreasePublicUpgradePoint", PhotonTargets.All, -upgrade.Cost);
        }
        else if (upgrade.GetPoint)
        {

            photonView.RPC("RPC_IncreaseUpgradePoint", PhotonTargets.All, id, upgrade.Cost);
        }
        else if (!upgrade.GivePoint)
        {
            Debug.Log(upgrade.Cost);
            if (PlayerStatus[id].UpgradePoint - upgrade.Cost >= 0)
            {

                photonView.RPC("RPC_IncreaseUpgradePoint", PhotonTargets.All, id, -upgrade.Cost);
                UpgradePlayerStatus(upgrade);
            }
            else if (PlayerStatus[id].PublicUpgradePoint - upgrade.Cost >= 0)
            {

                photonView.RPC("RPC_IncreasePublicUpgradePoint", PhotonTargets.All, -upgrade.Cost);

                UpgradePlayerStatus(upgrade);

            }


        }
        else if (upgrade.GivePoint&&PlayerStatus[id].UpgradePoint + upgrade.Cost >= 0)
        {



            photonView.RPC("RPC_IncreaseUpgradePoint", PhotonTargets.All,id, -upgrade.Cost);
            photonView.RPC("RPC_IncreasePublicUpgradePoint", PhotonTargets.All, upgrade.Cost);
            UpgradePlayerStatus(upgrade);
        }

    }
    [PunRPC]
    void RPC_IncreaseUpgradePoint(int id, int i)
    {

        PlayerStatus[id].UpgradePoint += i;

       /* if (PanelMgr.GetInstance() != null && PanelMgr.GetInstance().GetPanelFunction("UpgradePanel") != null&&PlayerStatus[id].PhotonPlayer==PhotonNetwork.player)
        {
            PanelMgr.GetInstance().GetPanelFunction("UpgradePanel").UpdateUpgradePoint(PlayerStatus[id].UpgradePoint);
        }*/
    }

    [PunRPC]
    void RPC_IncreasePublicUpgradePoint(int i)
    {
        for (int x = 0; x < PlayerStatus.Count; x++)
        {
            PlayerStatus[x].PublicUpgradePoint += i;
            if (PanelMgr.GetInstance() != null && PanelMgr.GetInstance().GetPanelFunction("UpgradePanel") != null)
            {
                PanelMgr.GetInstance().GetPanelFunction("UpgradePanel").UpdatePublicUpgradePoint(PlayerStatus[x].PublicUpgradePoint);
            }
        }



    }
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {


        if (stream.isWriting)
        {


        }
        else
        {



        }

    }
}

