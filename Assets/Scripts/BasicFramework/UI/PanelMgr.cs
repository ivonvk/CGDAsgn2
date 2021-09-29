using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum layer_type
{
    bottom,
    middle,
    top
}

/// <summary>
/// 面板管理器
/// 1.可以在任何地方创建和删除指定面板
/// 2.可以显示和隐藏面板
/// </summary>
public class PanelMgr : SingleMgr<PanelMgr>
{
    private Dictionary<string, BasePanel> panelDic;
    private Transform bottom;
    private Transform middle;
    private Transform top;

    public PanelMgr()
    {
        panelDic = new Dictionary<string, BasePanel>();
        Transform canvas = ResMgr.GetInstance().ResourceLoad<GameObject>("UI/Canvas/Canvas").transform;
        Transform eventSystem = ResMgr.GetInstance().ResourceLoad<GameObject>("UI/Canvas/EventSystem").transform;
        GameObject.DontDestroyOnLoad(canvas);
        GameObject.DontDestroyOnLoad(eventSystem);

        bottom = canvas.Find("Bottom");
        middle = canvas.Find("Middle");
        top = canvas.Find("Top");




    }

    /// <summary>
    /// 创建面板
    /// </summary>
    /// <typeparam name="T">返回的面板类</typeparam>
    /// <param name="panelName">面板名字</param>
    /// <param name="layer">面板层级</param>
    /// <param name="callBack">加载完成后的回调函数，传递面板类</param>
    public void CreatePanel<T>(string panelName, layer_type layer, UnityAction<T> callBack = null, bool isActive = true) where T : BasePanel
    {

        if (!panelDic.ContainsKey(panelName))
        {
            if (ResMgr.GetInstance() != null)
            {
                ResMgr.GetInstance().ResourceLoadAsync<GameObject>("UI/Panel/" + panelName, (obj) =>
                {

                    Transform father = bottom;
                    switch (layer)
                    {
                        case layer_type.middle:
                            father = middle;
                            break;
                        case layer_type.top:
                            father = top;
                            break;
                    }

                    obj.transform.SetParent(father);
                    obj.SetActive(isActive);
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localScale = Vector3.one;
                    (obj.transform as RectTransform).offsetMax = Vector2.zero;
                    (obj.transform as RectTransform).offsetMin = Vector2.zero;

                    T panel = obj.GetComponent<T>();

                    panel.ShowMe();
                    panelDic.Add(panelName, panel);

                    if (callBack != null)
                        callBack(panel);
                });
            }
        }
        else
        {
             if (callBack != null)
             {
                 panelDic[panelName].gameObject.SetActive(isActive);
                 callBack(panelDic[panelName] as T);
             }
        }
            
    }
    public BasePanel GetPanelFunction(string panelName)
    {

        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName];
        }
        return null;
    }
    public void ResetAllPanel()
    {
        foreach (KeyValuePair<string, BasePanel> key in panelDic)
        {
            panelDic[key.Key].ResetPanel();
        }
    }

    /// <summary>
    /// 显示或者隐藏面板
    /// </summary>
    /// <param name="panelName">面板名</param>
    /// <param name="isActive">是否显示</param>
    public void ShowOrHidePanel(string panelName, bool isActive)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].gameObject.SetActive(isActive);
            if (isActive)
                panelDic[panelName].ShowMe();
            else
                panelDic[panelName].HideMe();
        }
    }

    /// <summary>
    /// 删除指定面板
    /// </summary>
    /// <param name="name">面板名</param>
    public void DestoryPanel(string name)
    {
        if (panelDic.ContainsKey(name))
        {
            GameObject.Destroy(panelDic[name].gameObject);
            panelDic.Remove(name);
        }
    }
}
