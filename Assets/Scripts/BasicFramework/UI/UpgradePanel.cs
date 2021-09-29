using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeStatus
{
    public int MaxHealth = 0;
    public int MaxMagic = 0;
    public int Damage = 0;
    public float AttackSpeed = 0;
    public float WalkSpeed = 0;
    public int Cost = 0;

    public bool GivePoint = false;
    public bool GetPublicPoint = false;
    public bool GetPoint = false;
}
public class SkillStatus
{
    public int Skill_Level_0 = 0;
    public int Skill_Level_1 = 0;
    public int Skill_Level_2 = 0;
}
public class UpgradePanel : BasePanel
{


    public List<UpgradeStatus> Upgrades = new List<UpgradeStatus>();

    private List<SkillStatus> Skills = new List<SkillStatus>();

    private Button UpgradeSelection_Button_0;
    private Button UpgradeSelection_Button_1;
    private Button UpgradeSelection_Button_2;

    private Button KeepPointButton;
    private Button RollUpdateButton;

    private Text PointText;
    private Text PublicPointText;

    private Text MaxHealthText_0;
    //private Text MaxMagicText_0;
    private Text AttackText_0;
    private Text AttackSpeedText_0;
    private Text WalkSpeedText_0;
    private Text CostText_0;

    private Text MaxHealthText_1;
    //private Text MaxMagicText_1;
    private Text AttackText_1;
    private Text AttackSpeedText_1;
    private Text WalkSpeedText_1;
    private Text CostText_1;

    private Text MaxHealthText_2;
    //private Text MaxMagicText_2;
    private Text AttackText_2;
    private Text AttackSpeedText_2;
    private Text WalkSpeedText_2;
    private Text CostText_2;


    private Button UpgradeSkill_Button_0;
    private Button UpgradeSkill_Button_1;
    private Button UpgradeSkill_Button_2;

    private Text SkillInfoText_0;
    private Text SkillCostText_0;

    private Text SkillInfoText_1;
    private Text SkillCostText_1;

    private Text SkillInfoText_2;
    private Text SkillCostText_2;

    

    private Button GiveFivePointButton;
    
    public override void Start()
    {
        base.Start();
        UpgradeSelection_Button_0 = GetCtr<Button>("UpgradeSelection_Button_0");
        UpgradeSelection_Button_1 = GetCtr<Button>("UpgradeSelection_Button_1");
        UpgradeSelection_Button_2 = GetCtr<Button>("UpgradeSelection_Button_2");

        UpgradeSelection_Button_0.onClick.AddListener(UpgradeSelection_0);
        UpgradeSelection_Button_1.onClick.AddListener(UpgradeSelection_1);
        UpgradeSelection_Button_2.onClick.AddListener(UpgradeSelection_2);

        KeepPointButton = GetCtr<Button>("KeepPointButton");
        RollUpdateButton = GetCtr<Button>("RollUpdateButton");

        KeepPointButton.onClick.AddListener(KeepPoint);
        RollUpdateButton.onClick.AddListener(RollUpgrade);

        PointText = GetCtr<Text>("PointText");
        PublicPointText = GetCtr<Text>("PublicPointText");

        MaxHealthText_0 = GetCtr<Text>("MaxHealthText_0");
        AttackText_0 = GetCtr<Text>("AttackText_0");
        AttackSpeedText_0 = GetCtr<Text>("AttackSpeedText_0");
        WalkSpeedText_0 = GetCtr<Text>("WalkSpeedText_0");
        CostText_0 = GetCtr<Text>("CostText_0");

        MaxHealthText_1 = GetCtr<Text>("MaxHealthText_1");
        AttackText_1 = GetCtr<Text>("AttackText_1");
        AttackSpeedText_1 = GetCtr<Text>("AttackSpeedText_1");
        WalkSpeedText_1 = GetCtr<Text>("WalkSpeedText_1");
        CostText_1 = GetCtr<Text>("CostText_1");

        MaxHealthText_2 = GetCtr<Text>("MaxHealthText_2");
        AttackText_2 = GetCtr<Text>("AttackText_2");
        AttackSpeedText_2 = GetCtr<Text>("AttackSpeedText_2");
        WalkSpeedText_2 = GetCtr<Text>("WalkSpeedText_2");
        CostText_2 = GetCtr<Text>("CostText_2");

        UpgradeSkill_Button_0 = GetCtr<Button>("UpgradeSkill_Button_0");
        UpgradeSkill_Button_1 = GetCtr<Button>("UpgradeSkill_Button_1");
        UpgradeSkill_Button_2 = GetCtr<Button>("UpgradeSkill_Button_2");

        SkillInfoText_0 = GetCtr<Text>("SkillInfoText_0");
        SkillCostText_0 = GetCtr<Text>("SkillCostText_0");

        SkillInfoText_1 = GetCtr<Text>("SkillInfoText_1");
        SkillCostText_1 = GetCtr<Text>("SkillCostText_1");

        SkillInfoText_2 = GetCtr<Text>("SkillInfoText_2");
        SkillCostText_2 = GetCtr<Text>("SkillCostText_2");

        GiveFivePointButton = GetCtr<Button>("GiveFivePointButton");
        GiveFivePointButton.onClick.AddListener(GiveFivePoint);

        Upgrades.Add(new UpgradeStatus());
        Upgrades.Add(new UpgradeStatus());
        Upgrades.Add(new UpgradeStatus());

        gameObject.SetActive(false);
        RollUpgrade();
    }
    void KeepPoint()
    {
        RollUpdateButton.interactable = true;
        HideMe();
        
    }
    void Update()
    {
        if (PlayerNetwork.Instance != null && PlayerNetwork.Instance.GetCurrentPlayerController() != null && PlayerNetwork.Instance.GetCurrentPlayerController().playerStatus != null)
        {
            UpdateUpgradePoint(PlayerNetwork.Instance.GetCurrentPlayerController().playerStatus.UpgradePoint);
        }
    }
    void RollUpgrade()
    {
        // if (GameMaster.Instance.IncreaseUpgradePoint(-10)>=0)
        //   return;
        if (PlayerManager.Instance.GetPlayerStatus(PhotonNetwork.player) == null)
        {
            return;
        }
            if (PlayerManager.Instance.GetPlayerStatus(PhotonNetwork.player).UpgradePoint-10<0)
        {
            return;
        }

        UpgradeStatus u = new UpgradeStatus();
        u.AttackSpeed = 0;
        u.Cost = 10;
        u.Damage = 0;
        u.MaxHealth = 0;
        u.MaxMagic = 0;
        u.WalkSpeed = 0;
        u.GivePoint = false;

        PlayerManager.Instance.UpdateStatusFunc(u, PhotonNetwork.player.ID-1);
        UpgradeSelection_Button_0.interactable = true;
        UpgradeSelection_Button_1.interactable = true;
        UpgradeSelection_Button_2.interactable = true;


        Upgrades[0].MaxHealth = Random.Range(10, 10 * GameMaster.Instance.CurrentLevels)*2;
        //Upgrades[0].MaxMagic = Random.Range(1, 5 * GameMaster.Instance.CurrentLevels);
        Upgrades[0].Damage = Random.Range(5, 12 * GameMaster.Instance.CurrentLevels)+1;
        Upgrades[0].AttackSpeed = Random.Range(-0.01f, -0.05f * GameMaster.Instance.CurrentLevels);
        Upgrades[0].WalkSpeed = Random.Range(0.01f, 0.05f * GameMaster.Instance.CurrentLevels);
        Upgrades[0].Cost = (int)(((float)(Upgrades[0].MaxHealth + Upgrades[0].MaxMagic + Upgrades[0].Damage + Upgrades[0].AttackSpeed + Upgrades[0].WalkSpeed)) ) ;

        MaxHealthText_0.text = "Max Health+ " + Upgrades[0].MaxHealth.ToString("F0");
        //MaxMagicText_0.text = "Max Magic+ " + Upgrades[0].MaxMagic.ToString("F0");
        AttackText_0.text = "Attack +" + Upgrades[0].MaxHealth.ToString("F0");
        AttackSpeedText_0.text = "Attack Speed +" + (-Upgrades[0].AttackSpeed).ToString("F2");
        WalkSpeedText_0.text = "Walk Speed +" + (Upgrades[0].WalkSpeed).ToString("F2");
        CostText_0.text = "Cost: " + Upgrades[0].Cost.ToString("F0") + " Upgrade Point";

        Upgrades[1].MaxHealth = Random.Range(1, 5 * GameMaster.Instance.CurrentLevels) * 2;
        //Upgrades[1].MaxMagic = Random.Range(15, 15 * GameMaster.Instance.CurrentLevels);
        Upgrades[1].Damage = Random.Range(4, 8 * GameMaster.Instance.CurrentLevels)+1;
        Upgrades[1].AttackSpeed = Random.Range(-0.01f, -0.05f * GameMaster.Instance.CurrentLevels);
        Upgrades[1].WalkSpeed = Random.Range(0.01f, 0.05f * GameMaster.Instance.CurrentLevels);
        Upgrades[1].Cost = (int)(((float)(Upgrades[1].MaxHealth + Upgrades[1].MaxMagic + Upgrades[1].Damage + Upgrades[1].AttackSpeed + Upgrades[1].WalkSpeed)) );

        MaxHealthText_1.text = "Max Health+ " + Upgrades[1].MaxHealth.ToString("F0");
        //MaxMagicText_1.text = "Max Magic+ " + Upgrades[1].MaxMagic.ToString("F0");
        AttackText_1.text = "Attack +" + Upgrades[1].MaxHealth.ToString("F0");
        AttackSpeedText_1.text = "Attack Speed +" + (-Upgrades[1].AttackSpeed).ToString("F2");
        WalkSpeedText_1.text = "Walk Speed +" + (Upgrades[1].WalkSpeed).ToString("F2");
        CostText_1.text = "Cost: " + Upgrades[1].Cost.ToString("F0") + " Upgrade Point";

        Upgrades[2].MaxHealth = Random.Range(1, 5 * GameMaster.Instance.CurrentLevels) * 2;
        //Upgrades[2].MaxMagic = Random.Range(1, 5 * GameMaster.Instance.CurrentLevels);
        Upgrades[2].Damage = Random.Range(1, 2 * GameMaster.Instance.CurrentLevels)+1;
        Upgrades[2].AttackSpeed = Random.Range(-0.02f, -0.06f * GameMaster.Instance.CurrentLevels);
        Upgrades[2].WalkSpeed = Random.Range(0.02f, 0.04f * GameMaster.Instance.CurrentLevels);
        Upgrades[2].Cost = (int)(((float)(Upgrades[2].MaxHealth + Upgrades[2].MaxMagic + Upgrades[2].Damage + Upgrades[2].AttackSpeed + Upgrades[2].WalkSpeed))) ;

        MaxHealthText_2.text = "Max Health+ " + Upgrades[2].MaxHealth.ToString("F0");
        //MaxMagicText_2.text = "Max Magic+ " + Upgrades[2].MaxMagic.ToString("F0");
        AttackText_2.text = "Attack +" + Upgrades[2].MaxHealth.ToString("F0");
        AttackSpeedText_2.text = "Attack Speed +" + (-Upgrades[2].AttackSpeed).ToString("F2");
        WalkSpeedText_2.text = "Walk Speed +" + (Upgrades[2].WalkSpeed).ToString("F2");
        CostText_2.text = "Cost: " + Upgrades[2].Cost.ToString("F0") + " Upgrade Point";
    }
    void UpgradeSelection_0()
    {

        UpgradeSelection_Button_0.interactable = false;

        PlayerManager.Instance.UpdateUpgradePoint(Upgrades[0], PhotonNetwork.player);
        // PlayerManager.Instance.UpdateUpgradePoint(Upgrades[0]);
    }
    void UpgradeSelection_1()
    {
        UpgradeSelection_Button_1.interactable = false;
        PlayerManager.Instance.UpdateUpgradePoint(Upgrades[1], PhotonNetwork.player);

    }
    void UpgradeSelection_2()
    {
        UpgradeSelection_Button_2.interactable = false;
        PlayerManager.Instance.UpdateUpgradePoint(Upgrades[2],PhotonNetwork.player);


    }
    void GiveFivePoint()
    {
        UpgradeStatus u = new UpgradeStatus();
        u.AttackSpeed = 0;
        u.Cost = 5;
        u.Damage = 0;
        u.MaxHealth = 0;
        u.MaxMagic = 0;
        u.WalkSpeed = 0;
        u.GivePoint = true;
        PlayerManager.Instance.UpdateStatusFunc(u, PhotonNetwork.player.ID-1);


    }
    public override void UpdateUpgradePoint(int i)
    {
        PointText.text = "Upgrade Point: " + i.ToString("F0");
    }
    public override void UpdatePublicUpgradePoint(int i)
    {
        PublicPointText.text = "Public Point: " + i.ToString("F0");
    }
}
