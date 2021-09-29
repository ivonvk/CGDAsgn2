using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateInstances : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        MonoMgr.GetInstance().AddUpdateListener(SetAllCreatedInstances);
    }
   
    void SetAllCreatedInstances()
    {
        
        
        /*LoadCharacterMgr.GetInstance().RefreshCharacterStats();
        LoadBuildingMgr.GetInstance().RefreshBuildingStats();
        LoadItemsMgr.GetInstance().RefreshItem();
        LoadSaveDataListMgr.GetInstance().LoadAllSavedWorld();*/
        if (PlayerNetwork.Instance == null)
        {
            ResMgr.GetInstance().ResourceLoadAsync<GameObject>("Network/PlayerNetwork",(o)=> { 
                DontDestroyOnLoad(o);
                o.GetComponent<PhotonView>().viewID = 999;
            
            });
        }
        MonoMgr.GetInstance().RemoveUpdateListener(SetAllCreatedInstances);
        DelaySetup();

    }
    void DelaySetup()
    {
      
        
        Debug.Log("ok");
    }

}

