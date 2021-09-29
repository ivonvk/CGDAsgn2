using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuPanel : BasePanel
{
    private Button BackToGameBtn;
    private Button OpenLeavePanelBtn;
    private Button CloseLeavePanelBtn;
    private Button LeaveGameBtn;

    private GameObject ConfirmLeavePanel;
    public override void  Start()
    {
        base.Start();
        BackToGameBtn = GetCtr<Button>("BackToGameBtn");
        OpenLeavePanelBtn = GetCtr<Button>("OpenLeavePanelBtn");
        CloseLeavePanelBtn = GetCtr<Button>("CloseLeavePanelBtn");
        LeaveGameBtn = GetCtr<Button>("LeaveGameBtn");

        BackToGameBtn.onClick.AddListener(BackToGame);
        OpenLeavePanelBtn.onClick.AddListener(OpenLeavePanel);
        CloseLeavePanelBtn.onClick.AddListener(CloseLeavePanel);

        LeaveCurrentMatch leave = gameObject.AddComponent<LeaveCurrentMatch>();
        LeaveGameBtn.onClick.AddListener(leave.OnClick_LeaveMatch);

        ConfirmLeavePanel = GetCtr<Image>("ConfirmLeavePanel").gameObject;
        CloseLeavePanel();
        ConfirmLeavePanel.SetActive(false);
        gameObject.SetActive(false);
    }
    void BackToGame()
    {
        gameObject.SetActive(false);
    }
    void OpenLeavePanel()
    {
        ConfirmLeavePanel.SetActive(true);
    }
    void CloseLeavePanel()
    {
        ConfirmLeavePanel.SetActive(false);
    }
}
