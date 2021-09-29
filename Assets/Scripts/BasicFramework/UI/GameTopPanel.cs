using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class TeammateUIs
{
    public Slider PlayerHPBar;
    public Text PlayerNameText;
    public GameObject PlayerBox;
}
public class GameTopPanel : BasePanel
{

    List<TeammateUIs> PlayerUI = new List<TeammateUIs>();
    private Button MenuBtn;
    private Text GameMsgText;
    private Text PublicPointText;
    public override void Start()
    {
        base.Start();
        if (PlayerUI.Count <= 0)
        {
            PlayerUI.Add(new TeammateUIs());
            PlayerUI.Add(new TeammateUIs());
            PlayerUI.Add(new TeammateUIs());
            PlayerUI.Add(new TeammateUIs());
        }

        PlayerUI[0].PlayerHPBar = GetCtr<Slider>("Player2HPBar");
        PlayerUI[1].PlayerHPBar = GetCtr<Slider>("Player3HPBar");
        PlayerUI[2].PlayerHPBar = GetCtr<Slider>("Player4HPBar");
        PlayerUI[3].PlayerHPBar = GetCtr<Slider>("Player5HPBar");

        PlayerUI[0].PlayerNameText = GetCtr<Text>("Player2NameText");
        PlayerUI[1].PlayerNameText = GetCtr<Text>("Player3NameText");
        PlayerUI[2].PlayerNameText = GetCtr<Text>("Player4NameText");
        PlayerUI[3].PlayerNameText = GetCtr<Text>("Player5NameText");

        PlayerUI[0].PlayerBox = GetCtr<Image>("Player2").gameObject;
        PlayerUI[1].PlayerBox = GetCtr<Image>("Player3").gameObject;
        PlayerUI[2].PlayerBox = GetCtr<Image>("Player4").gameObject;
        PlayerUI[3].PlayerBox = GetCtr<Image>("Player5").gameObject;

        PlayerUI[0].PlayerBox.SetActive(false);
        PlayerUI[1].PlayerBox.SetActive(false);
        PlayerUI[2].PlayerBox.SetActive(false);
        PlayerUI[3].PlayerBox.SetActive(false);

        MenuBtn = GetCtr<Button>("MenuBtn");
        GameMsgText = GetCtr<Text>("GameMsgText");

        PublicPointText = GetCtr<Text>("PublicPointText");

        MenuBtn.onClick.AddListener(OpenMenu);


      //  GetCtr<Button>("btnEquip").onClick.AddListener(EquipWeapon);
       // GetCtr<Button>("btnOpen").onClick.AddListener(SwitchItemsMainPanel);
       // GetCtr<Button>("btnClose").onClick.AddListener(SwitchItemsMainPanel);

        ResetPanel();
    }
    
    void OpenMenu()
    {
        PanelMgr.GetInstance().ShowOrHidePanel("InGameMenuPanel", true);
    }
    void Update()
    {
        //GameMsgText.text = "Wave: " + GameMaster.Instance.GetGameMasterValue("CurrentWave");
    }
    public override void UpdateGameMsgText()
    {
        if(PlayerManager.Instance.GetPlayerStatus(PhotonNetwork.player) == null)
        {
            return;
        }
        if (GameMaster.Instance.NextStageDelay > 0)
        {
            GameMsgText.text = GameMaster.Instance.NextStageDelay.ToString("F0");
        }
        else
        {
            GameMsgText.text = "Wave: " + (GameMaster.Instance.CurrentWaves+1).ToString("F0");
        }
        PublicPointText.text =  "Public Point: " +PlayerManager.Instance.GetPlayerStatus(PhotonNetwork.player).PublicUpgradePoint.ToString();
    }
    public override void UpdateTeammatesDisplay(int index,float value,bool active)
    {
        PlayerUI[index].PlayerHPBar.value = value;
        PlayerUI[index].PlayerBox.SetActive(active);
    }
}
