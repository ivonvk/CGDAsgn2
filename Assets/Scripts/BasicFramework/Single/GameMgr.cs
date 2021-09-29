using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMgr : SingleMgr<GameMgr>
{
    private bool IsGameStarter = false;
   // public GameObject temp;

   /* public GameObject GetTemp()
    {
        return temp;
    }*/
    public GameMgr()
    {
        GameMgrResetEventTrigger();
     //   temp = new GameObject("World");
    }
    public void Reset()
    {
      // temp = new GameObject("World");
    }



    public void GameMgrResetEventTrigger()
    {
        EventsCenter.GetInstance().AddEventListener("MainObjectDead", MainObjectDead);
        EventsCenter.GetInstance().AddEventListener("SceneChange", ResetGameStartedInfo);
    }
    void ResetGameStartedInfo()
    {
        SetTheGameStartedOrNot(false);
    }
    public void SetTheGameStartedOrNot(bool start)
    {
        IsGameStarter = start;
    }
    public bool GetGameStartedInfo()
    {
        return IsGameStarter;
    }

    public bool OverTheGame()
    {
        return true;
    }

    public void Progress(int value)
    {
        EventsCenter.GetInstance().TriggerEvent("Progress");
    }

    public void MainObjectDead()
    {
        PanelMgr.GetInstance().CreatePanel<GameOverPanel>("GameOverPanel", layer_type.middle);
        AudioMgr.GetInstance().StopAllEffectAudio();
        Debug.Log("事件管理员得知重要物件已死亡");
    }

    public void NPCObjectDead(bool isEnemy)
    {
        EventsCenter.GetInstance().TriggerEvent("NPCObjectDead");
    }

    public void SceneEnd()
    {
        //Reset();
        if (MonoMgr.GetInstance() != null&& MonoMgr.GetInstance().GetInitialMgr()!=null)
        {
             MonoMgr.GetInstance().GetInitialMgr().ResetScript();
        }

        //EventsCenter.GetInstance().TriggerEvent("SceneChange");

        //PanelMgr.GetInstance().DestoryPanel("GameResultPanel");
        //PanelMgr.GetInstance().DestoryPanel("GameOverPanel");


        //PoolMgr.GetInstance().ClearDic();
        // EventsCenter.GetInstance().ClearEvents();
        // MonoMgr.GetInstance().Reset();

        // InputMgr.GetInstance().Reset();

        //AudioMgr.GetInstance().StopAllEffectAudio();

        // GameMgrResetEventTrigger();
        if (PanelMgr.GetInstance() != null)
        {
            PanelMgr.GetInstance().ResetAllPanel();
        }
    
    }
}
