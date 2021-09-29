using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameBottomPanel : BasePanel
{


    private Slider PlayerHPBar;
    private Slider PlayerMPBar;
    private Text PlayerJobText;
    //private Button WeaponBtn;
   // private Button Skill1Btn;
   // private Button Skill2Btn;
    //private Button Skill3Btn;
    public override void Start()
    {
        base.Start();

        PlayerHPBar = GetCtr<Slider>("PlayerHPBar");
        PlayerMPBar = GetCtr<Slider>("PlayerMPBar");

       // WeaponBtn = GetCtr<Button>("WeaponBtn");
        PlayerJobText = GetCtr<Text>("PlayerJobText");

     //  Skill1Btn = GetCtr<Button>("Skill1Btn");
      //  Skill2Btn = GetCtr<Button>("Skill2Btn");
       // Skill3Btn = GetCtr<Button>("Skill3Btn");

        //  GetCtr<Button>("btnEquip").onClick.AddListener(EquipWeapon);
        // GetCtr<Button>("btnOpen").onClick.AddListener(SwitchItemsMainPanel);
        // GetCtr<Button>("btnClose").onClick.AddListener(SwitchItemsMainPanel);

        ResetPanel();
    }
    void Update()
    {
        if (PlayerNetwork.Instance != null && PlayerNetwork.Instance.GetCurrentPlayerController() != null)
        {
            PlayerHPDisplay((float)PlayerNetwork.Instance.GetCurrentPlayerController().playerStatus.Health / (float)PlayerNetwork.Instance.GetCurrentPlayerController().playerStatus.MaxHP);
        }
    }
    public override void PlayerHPDisplay(float f)
    {
        PlayerHPBar.value = f;

    }
}
