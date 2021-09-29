using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class LoadGamesPanel : BasePanel
{
    private ScrollRect ScrollPanel;
    private Transform ScrollContent;
    private string[] SavedFileNames;
    private Button StartLoadedButton;
    private Button CloseButton;
    public List<Button> AllSaveButton = new List<Button>(new Button[100]);
    public override void Start()
    {
        base.Start();

        CloseButton = GetCtr<Button>("CloseButton");
        StartLoadedButton = GetCtr<Button>("StartLoadedButton");
        StartLoadedButton.onClick.AddListener(StartLoadedWorld);
        ScrollPanel = GetCtr<ScrollRect>("ScrollPanel");
        ScrollContent = GetCtr<Image>("Content").transform;
        CloseButton.onClick.AddListener(CloseSaveListPanel);
        ResetPanel();
        ResetSaveData();
    }
    public override void ResetSaveData()
    {
        ResetButtons();
        Invoke("AddSavedFileNames", 1f);
    }
    void CloseSaveListPanel()
    {
        PanelMgr.GetInstance().ShowOrHidePanel("LoadGamesPanel", false);
    }

    public override void AddSavedFileNames()
    {
        SavedFileNames = LoadSaveDataListMgr.GetInstance().GetSavedNames();

        DisplayAllSavedFiles();

    }
    private void StartLoadedWorld()
    {
       /* if (SaveLoadMgr.GetInstance().GetSaveFileName() != "")
        {
            GameMgr.GetInstance().SceneEnd();
            ScenesMgr.GetInstance().LoadSceneAsync("RandomTinyWorldMap", Nothing);
        }*/
    }
    void Nothing()
    {

    }


    public void ResetButtons()
    {


        for (int i = 0; i < AllSaveButton.Count; i++)
        {
            if (AllSaveButton[i] != null)
            {

                PoolMgr.GetInstance().PushObj(AllSaveButton[i].gameObject);
                AllSaveButton[i] = null;
            }
        }

    }
    public override void DisplayAllSavedFiles()
    {
        float totalAddY = 0;
        ScrollContent.GetComponent<RectTransform>().sizeDelta = new Vector2(ScrollContent.GetComponent<RectTransform>().sizeDelta.x, 100f);
        for (int i = 0; i < SavedFileNames.Length; i++)
        {

            if (SavedFileNames[i] == null)
                continue;
            else
                totalAddY += 100f;
            string sn = SavedFileNames[i];
            PoolMgr.GetInstance().GetObj("UI/SavedData", (o) =>
            {
                o.transform.SetParent(ScrollContent);
                Button b = o.GetComponent<Button>();
                
                for (int j = 0; j < AllSaveButton.Count; j++)
                {
                    if (AllSaveButton[j]== null)
                    {
                        AllSaveButton[j] = b;
                        break;
                    }
                }
                b.onClick.AddListener(() => SaveLoadMgr.GetInstance().SetSaveFileName(sn));
                o.transform.GetChild(0).GetComponent<Text>().text = sn;

            });
            
        }
        ScrollContent.GetComponent<RectTransform>().sizeDelta = new Vector2(ScrollContent.GetComponent<RectTransform>().sizeDelta.x, ScrollContent.GetComponent<RectTransform>().sizeDelta.y + totalAddY);

    }

}
