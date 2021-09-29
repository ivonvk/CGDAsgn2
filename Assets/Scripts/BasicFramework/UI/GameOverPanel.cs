using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOverPanel : BasePanel
{
    private Text GameEndMsgText;
    public override void Start()
    {
        base.Start();
        LeaveCurrentMatch leaveGame = gameObject.AddComponent<LeaveCurrentMatch>();
        GameEndMsgText = GetCtr<Text>("GameEndMsgText");
        GetCtr<Button>("LeaveButton").onClick.AddListener(leaveGame.OnClick_LeaveMatch);
        GetCtr<Button>("LeaveButton").onClick.AddListener(ClosePanel);
        gameObject.SetActive(false);
    }
 
    public override void GameEndMsg(string s, bool win)
    {
        GameEndMsgText.text = s;
        if (win)
        {
            GameEndMsgText.color = Color.black;
        }
        else
        {
            GameEndMsgText.color = Color.red;
        }
    }
    void ClosePanel()
    {
        gameObject.SetActive(false);
    }
  
}
