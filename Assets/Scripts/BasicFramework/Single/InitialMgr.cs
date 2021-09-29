using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InitialMgr : MonoBehaviour
{
    bool Initialized = false;
    private void Start()
    {
        //Invoke("UpdateInitialization", 1f);
        //UpdateInitialization();
        MonoMgr.GetInstance().AddUpdateListener(UpdateInitialization);
    }
    public void ResetScript()
    {
        Initialized = false;
        Debug.Log("ResetScript");
        //Invoke("UpdateInitialization", 1f);
        //  UpdateInitialization();
        MonoMgr.GetInstance().AddUpdateListener(UpdateInitialization);
    }
    void InitializationDelay()
    {
        UpdateInitialization();
    }
    public bool GetInitialized()
    {
        return Initialized;
    }
    void UpdateInitialization()
    {
        if (!Initialized)
        {
            Initialized = true;
            InputMgr.GetInstance().SetCanInput(true);
            InitialPanel(SceneManager.GetActiveScene().name);
            InitialPrefabs(SceneManager.GetActiveScene().name);
            InitialLevelToGameMgr(SceneManager.GetActiveScene().name);
        }
        else if(PanelMgr.GetInstance()!=null&& 
            PanelMgr.GetInstance().GetPanelFunction("InGameMenuPanel")!=null&&
            PanelMgr.GetInstance().GetPanelFunction("GameOverPanel") != null&&
             PanelMgr.GetInstance().GetPanelFunction("UpgradePanel") != null)
        {
            MonoMgr.GetInstance().RemoveUpdateListener(UpdateInitialization);
            if(SceneManager.GetActiveScene().name == "Game")
            {
                PanelMgr.GetInstance().GetPanelFunction("InGameMenuPanel").HideMe();
                PanelMgr.GetInstance().GetPanelFunction("GameOverPanel").HideMe();
                PanelMgr.GetInstance().GetPanelFunction("UpgradePanel").HideMe();
            }
        }
    }
    private void InitialPanel(string sceneName)
    {
        switch (sceneName)
        {
            case "LoginScene":
            case "Game":

                if (PanelMgr.GetInstance() != null)
                {
  
                    CreateInitialPanel<GameTopPanel>("GameTopPanel", layer_type.middle);
                    CreateInitialPanel<GameBottomPanel>("GameBottomPanel", layer_type.middle);
                    
                    CreateInitialPanel<GameOverPanel>("GameOverPanel", layer_type.middle);
                    CreateInitialPanel<InGameMenuPanel>("InGameMenuPanel", layer_type.middle);
                    CreateInitialPanel<CharacterSelectPanel>("CharacterSelectPanel", layer_type.middle);
                    CreateInitialPanel<UpgradePanel>("UpgradePanel", layer_type.middle);


                    
                }
                break;
            case "GameRoomScene":


                
                break;
            default:
                break;
        }
    }
    private void InitialPrefabs(string sceneName)
    {
        switch (sceneName)
        {
            case "SampleScene":
                break;
            default:
                break;
        }
    }
    private void InitialLevelToGameMgr(string sceneName)
    {
        switch (sceneName)
        {
            case "BattleGameScene":
                break;
            default:
                break;
        }
    }
    private void CreateInitialPanel<T>(string panel, layer_type pos) where T : BasePanel
    {
            PanelMgr.GetInstance().CreatePanel<T>(panel, pos, (o) => { Debug.Log(panel + " 面板被生成."); });
    }
    void Nothing()
    {

    }
}
