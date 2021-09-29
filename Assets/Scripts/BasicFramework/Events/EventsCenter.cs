using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEvent
{

}

public class EventsData<T>: IEvent
{
    public UnityAction<T> action;

    public EventsData(UnityAction<T> action)
    {
        this.action = action;
    }
}

public class EventsData : IEvent
{
    public UnityAction action;

    public EventsData(UnityAction action)
    {
        this.action = action;
    }
}

/// <summary>
/// 事件中心
/// 1.事件监听可以有参或无参
/// 2.添加,移除 有参或无参 事件监听
/// 3.移除事件
/// 4.触发事件
/// ps: 1.注册事件的时候一定要注意事件名字以及参数是否一致
///     2.无参和有参的委托事件名不可重复
/// </summary>
public class EventsCenter : SingleMgr<EventsCenter>
{
    private Dictionary<string, IEvent> eventDic= new Dictionary<string, IEvent>();

    #region 有参事件
    /// <summary>
    /// 添加有参事件监听者
    /// </summary>
    /// <typeparam name="T">委托返回类型</typeparam>
    /// <param name="evnetName">事件名</param>
    /// <param name="callBack">回调函数</param>
    public void AddEventListener<T>(string evnetName, UnityAction<T> callBack)
    {
        if(eventDic.ContainsKey(evnetName))
            (eventDic[evnetName] as EventsData<T>).action += callBack;
        else
            eventDic.Add(evnetName, new EventsData<T>(callBack));
    }

    /// <summary>
    /// 移除有参事件监听者
    /// </summary>
    /// <typeparam name="T">委托返回类型</typeparam>
    /// <param name="eventName">事件名</param>
    /// <param name="callBack">回调函数</param>
    public void RemoveEventListener<T>(string eventName, UnityAction<T> callBack)
    {
        if (eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventsData<T>).action -= callBack;
    }

    /// <summary>
    /// 触发有参事件
    /// </summary>
    /// <typeparam name="T">委托返回类型</typeparam>
    /// <param name="eventName">事件名</param>
    /// <param name="t">委托传递的对象</param>
    public void TriggerEvent<T>(string eventName, T t)
    {
        if (eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventsData<T>).action(t);
    }
    #endregion

    #region 无参事件
    /// <summary>
    /// 添加无参事件监听者
    /// </summary>
    /// <param name="evnetName">事件名</param>
    /// <param name="callBack">回调函数</param>
    public void AddEventListener(string evnetName, UnityAction callBack)
    {
        if (eventDic.ContainsKey(evnetName))
            (eventDic[evnetName] as EventsData).action += callBack;
        else
            eventDic.Add(evnetName, new EventsData(callBack));
    }

    /// <summary>
    /// 移除无参事件监听者
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="callBack">回调函数</param>
    public void RemoveEventListener(string eventName, UnityAction callBack)
    {
        if (eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventsData).action -= callBack;
    }

    /// <summary>
    /// 触发无参事件
    /// </summary>
    /// <param name="eventName">事件名</param>
    public void TriggerEvent(string eventName)
    {
        if (eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventsData).action();
    }
    #endregion

    /// <summary>
    /// 移除事件
    /// </summary>
    /// <param name="eventName">事件名</param>
    public void RemoveEvent(string eventName)
    {
        if (eventDic.ContainsKey(eventName))
            eventDic.Remove(eventName);
    }

    /// <summary>
    /// 清空所有委托
    /// ps:切换场景的时候可以调用，保证不会出现内存泄漏
    /// </summary>
    public void ClearEvents()
    {
        eventDic.Clear();
    }
}
