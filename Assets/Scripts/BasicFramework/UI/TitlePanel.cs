using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class TitlePanel : BasePanel
{

    private Button NewGameButton;
    private Button LoadGamesButton;

    public override void Start()
    {
        base.Start();
        NewGameButton = GetCtr<Button>("NewGameButton");
        LoadGamesButton = GetCtr<Button>("LoadGamesButton");
        NewGameButton.onClick.AddListener(NewGameStart);
        LoadGamesButton.onClick.AddListener(SavedGamesList);

    }

    void NewGameStart()
    {
        SaveLoadMgr.GetInstance().SetSaveFileName("");
        GameMgr.GetInstance().SceneEnd();
        //ScenesMgr.GetInstance().LoadSceneAsync("RandomTinyWorldMap", Nothing);
    }
    void Nothing()
    {

    }
    void SavedGamesList()
    {
        CreateInitialPanel<LoadGamesPanel>("LoadGamesPanel", layer_type.middle);
    }
    private void CreateInitialPanel<T>(string panel, layer_type pos) where T : BasePanel
    {
        PanelMgr.GetInstance().CreatePanel<T>(panel, pos, (o) => { Debug.Log(panel + " 面板被生成."); });

    }
}
