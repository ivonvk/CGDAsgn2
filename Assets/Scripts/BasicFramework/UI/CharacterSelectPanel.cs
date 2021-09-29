using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPanel : BasePanel
{
    private Text JobNameText;
    private Text TypeText;
    private Text SkillText;
    //private Text DifficultText;

    private Button SwordsmanBtn;
    private Button SwordswomanBtn;
    private Button ShieldSoldierBtn;
    private Button ShieldWomanSoldierBtn;
    private Button ArcherBtn;
    private Button WomanArcherBtn;
    private Button PriestBtn;
    private Button WomanPriestBtn;

    private Button ConfirmBtn;

    private int SelectedJobIndex = 0;
    private int SelectedWeaponIndex = 0;



    public override void Start()
    {
        base.Start();
        JobNameText= GetCtr<Text>("JobNameText");
        TypeText = GetCtr<Text>("TypeText");
        SkillText = GetCtr<Text>("SkillText");
        //DifficultText = GetCtr<Text>("DifficultText");

        SwordsmanBtn = GetCtr<Button>("SwordsmanBtn");
        SwordswomanBtn = GetCtr<Button>("SwordswomanBtn");
        ShieldSoldierBtn = GetCtr<Button>("ShieldSoldierBtn");
        ShieldWomanSoldierBtn = GetCtr<Button>("ShieldWomanSoldierBtn");
        ArcherBtn = GetCtr<Button>("ArcherBtn");
        WomanArcherBtn = GetCtr<Button>("WomanArcherBtn");
        PriestBtn = GetCtr<Button>("PriestBtn");
        WomanPriestBtn = GetCtr<Button>("WomanPriestBtn");

        ConfirmBtn = GetCtr<Button>("ConfirmBtn");

        SwordsmanBtn.onClick.AddListener(Swordsman_m_Seleted);
        SwordswomanBtn.onClick.AddListener(Swordsman_f_Seleted);
        ShieldSoldierBtn.onClick.AddListener(ShieldSoldier_m_Seleted);
        ShieldWomanSoldierBtn.onClick.AddListener(ShieldSoldier_f_Seleted);
        ArcherBtn.onClick.AddListener(Archer_m_Seleted);
        WomanArcherBtn.onClick.AddListener(Archer_f_Seleted);
        PriestBtn.onClick.AddListener(Priest_m_Seleted);
        WomanPriestBtn.onClick.AddListener(Priest_f_Seleted);

        ConfirmBtn.onClick.AddListener(ConfirmCharacter);
        HideMe();
    }
    void Swordsman_m_Seleted()
    {
        JobNameText.text = "Job: " + "Swordsman";
        TypeText.text = "Type: " + "Melee";
        SkillText.text = "Skill: " + "Attack Heal, Destroy Ranged Attack";
        //DifficultText.text = "Difficult: " + "Normal";

        SelectedJobIndex = 0;
        SelectedWeaponIndex = 0;
    }
    void Swordsman_f_Seleted()
    {
        JobNameText.text = "Job: " + "Swordswoman";
        TypeText.text = "Type: " + "Melee";
        SkillText.text = "Skill: " + "Attack Heal, Destroy Ranged Attack";
        //DifficultText.text = "Difficult: " + "Normal";

        SelectedJobIndex = 4;
        SelectedWeaponIndex = 2;
    }
    void ShieldSoldier_m_Seleted()
    {
        JobNameText.text = "Job: " + "Shield Soldier";
        TypeText.text = "Type: " + "Melee";
        SkillText.text = "Skill: " + "Destroy Ranged Attack, Set-up Barricade";
        //DifficultText.text = "Difficult: " + "Normal";

        SelectedJobIndex = 1;
        SelectedWeaponIndex = 1;
    }
    void ShieldSoldier_f_Seleted()
    {
        JobNameText.text = "Job: " + "Shield Woman Soldier";
        TypeText.text = "Type: " + "Melee";
        SkillText.text = "Skill: " + "Destroy Ranged Attack, Set-up Barricade";
        //DifficultText.text = "Difficult: " + "Normal";

        SelectedJobIndex = 5;
        SelectedWeaponIndex = 3;
    }
    void Archer_m_Seleted()
    {
        JobNameText.text = "Job: " + "Archer";
        TypeText.text = "Type: " + "Ranged";
        SkillText.text = "Skill: " + "Set-up Trap";
        //DifficultText.text = "Difficult: " + "Normal";

        SelectedJobIndex = 2;
    }
    void Archer_f_Seleted()
    {
        JobNameText.text = "Job: " + "Woman Archer";
        TypeText.text = "Type: " + "Ranged";
        SkillText.text = "Skill: " + "Set-up Trap";
        //DifficultText.text = "Difficult: " + "Normal";

        SelectedJobIndex = 6;
    }
    void Priest_m_Seleted()
    {
        JobNameText.text = "Job: " + "Priest";
        TypeText.text = "Type: " + "Ranged";
        SkillText.text = "Skill: " + "Heal, AOE Damage";
        //DifficultText.text = "Difficult: " + "Normal";

        SelectedJobIndex = 3;
    }
    void Priest_f_Seleted()
    {
        JobNameText.text = "Job: " + "Woman Priest";
        TypeText.text = "Type: " + "Ranged";
        SkillText.text = "Skill: " + "Heal, AOE Damage";
        //DifficultText.text = "Difficult: " + "Normal";

        SelectedJobIndex = 7;
    }
    void ConfirmCharacter()
    {
        if (PlayerNetwork.Instance != null && PlayerNetwork.Instance.GetCurrentPlayerController() != null)
        {
            PlayerNetwork.Instance.SetPlayerCharacter(SelectedJobIndex, SelectedWeaponIndex);

            //PanelMgr.GetInstance().ShowOrHidePanel("UpgradePanel", true);
            HideMe();
        }
        
    }
}
