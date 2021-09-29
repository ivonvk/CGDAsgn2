using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 输入管理器
/// 1.外部只需注册监听即可获取各种输入事件
/// </summary>
public class InputMgr : SingleMgr<InputMgr>
{
    private bool canInput;
    bool Initialized = false;
    //JoystickPanel joystickPanel;
    //GameTopPanel gameTopPanel;
   


    public InputMgr()
    {
        MonoMgr.GetInstance().AddUpdateListener(Update);

        
    }
    public void Reset()
    {
        MonoMgr.GetInstance().AddUpdateListener(Update);
    }
    public bool CheckInitialized()
    {
        return Initialized;
    }
    public void AddControllerToInputMgr(string panel) 
    {
        switch (panel)
        {
            case "JoystickPanel":
                //joystickPanel = PanelMgr.GetInstance().GetPanelFunction(panel) as JoystickPanel;
                break;
            case "CombatPanel":
                //gameTopPanel = PanelMgr.GetInstance().GetPanelFunction(panel) as GameTopPanel;
                break;

        }
    }
    
    private void Update()
    {


        GetKeyDownOrUp(KeyCode.W);
        GetKeyDownOrUp(KeyCode.A);
        GetKeyDownOrUp(KeyCode.S);
        GetKeyDownOrUp(KeyCode.D);
        GetKeyDownOrUp(KeyCode.F);
        GetKeyDownOrUp(KeyCode.V);
        GetKeyDownOrUp(KeyCode.R);
        GetKeyDownOrUp(KeyCode.J);
        GetKeyDownOrUp(KeyCode.L);
        GetKeyDownOrUp(KeyCode.I);
        GetKeyDownOrUp(KeyCode.K);
        GetKeyDownOrUp(KeyCode.Z);
        GetKeyDownOrUp(KeyCode.LeftArrow);
        GetKeyDownOrUp(KeyCode.RightArrow);
        GetKeyDownOrUp(KeyCode.UpArrow);
        GetKeyDownOrUp(KeyCode.DownArrow);
        GetKeyDownOrUp(KeyCode.Space);
        GetKeyDownOrUp(KeyCode.C);




    
        Initialized = true;

    }





    /// <summary>
    /// 获取按键是否按下或抬起
    /// </summary>
    /// <param name="key">按键</param>
    private void GetKeyDownOrUp(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            EventsCenter.GetInstance().TriggerEvent<KeyCode>("按键跳起", key);
        }

        if (Input.GetKey(key))
        {
            if(key !=KeyCode.Space)
                EventsCenter.GetInstance().TriggerEvent<KeyCode>("按键按下", key);
            if (key == KeyCode.Z)
            {
     
                EventsCenter.GetInstance().TriggerEvent<KeyCode>("Attack", key);
            }
        }
        
        if (Input.GetKeyUp(key))
        {
            EventsCenter.GetInstance().TriggerEvent<KeyCode>("按键抬起", key);
        }

    }

    /// <summary>
    /// 设置是否可以进行输入操作
    /// </summary>
    /// <param name="canInput">是否可以进行输入操作</param>
    public void SetCanInput(bool canInput)
    {
        this.canInput = canInput;
    }
}
