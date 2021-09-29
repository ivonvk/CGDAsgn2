using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源加载管理器
/// 1.可以同步加载资源
/// 2.可以异步加载资源
/// </summary>
public class ResMgr : SingleMgr<ResMgr>
{
    /// <summary>
    /// 同步加载场景
    /// </summary>
    /// <typeparam name="T">返回类型</typeparam>
    /// <param name="path">资源路径</param>
    /// <returns></returns>
    public T ResourceLoad<T>(string path) where T:Object
    {
        T obj = Resources.Load<T>(path);
        if(obj is GameObject)
            return GameObject.Instantiate(obj);

        return obj;
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <typeparam name="T">返回类型</typeparam>
    /// <param name="path">资源路径</param>
    /// <param name="callBack">资源加载完成后的回调函数</param>
    public void ResourceLoadAsync<T>(string path, UnityAction<T> callBack) where T : Object
    {
        MonoMgr.GetInstance().StartCoroutine(ResLoadAsync<T>(path, callBack));
    }

    IEnumerator ResLoadAsync<T>(string path, UnityAction<T> callBack) where T : Object
    {
        ResourceRequest temp = Resources.LoadAsync<T>(path);

       // yield return temp.isDone;
        yield return temp;   //原版的是这样的

        if (temp.asset is GameObject)
            callBack(GameObject.Instantiate(temp.asset) as T);
        else
            callBack(temp.asset as T);
    }
}
