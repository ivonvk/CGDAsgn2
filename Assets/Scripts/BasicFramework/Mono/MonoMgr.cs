using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 公共Mono管理器
/// 1.可以让没有继承MonoBehaviour的类运用MonoBehaviour类的方法
/// </summary>
public class MonoMgr : SingleMgr<MonoMgr>
{
    private MonoController monoCtr;
    private InitialMgr iniPanelMgr;

    

    public GameObject temp;
    Camera MainCamera;
    public MonoMgr()
    {
       temp = new GameObject("EventController");
       
       monoCtr = temp.AddComponent<MonoController>();
       iniPanelMgr = temp.AddComponent<InitialMgr>();
       // if (!MainCamera)
            //MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

    }
    public void Reset()
    {
        MonoBehaviour.Destroy(temp);
        temp = new GameObject("EventController");
        monoCtr = temp.AddComponent<MonoController>();
        iniPanelMgr = temp.AddComponent<InitialMgr>();
        /*if (!MainCamera)
            MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();*/

        //Time.timeScale = 1;

    }
    
   
    public InitialMgr GetInitialMgr()
    {
        return iniPanelMgr;
    }


    /// <summary>
    /// 添加Update监听者
    /// </summary>
    /// <param name="update">需要监听的事件</param>
    public void AddUpdateListener(UnityAction update)
    {
        monoCtr.AddUpdateListener(update);
    }

    /// <summary>
    /// 移除Update监听者
    /// </summary>
    /// <param name="update">需要移除的监听事件</param>
    public void RemoveUpdateListener(UnityAction update)
    {
        monoCtr.RemoveUpdateListener(update);
    }

    /// <summary>
    /// 开启协程
    /// </summary>
    /// <param name="methodName">协程函数名</param>
    /// <returns></returns>
    public Coroutine StartCoroutine(string methodName)
    {
        return monoCtr.StartCoroutine(methodName);
    }

    /// <summary>
    /// 开启协程
    /// </summary>
    /// <param name="methodName">协程函数</param>
    /// <returns></returns>
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return monoCtr.StartCoroutine(routine);
    }

}
