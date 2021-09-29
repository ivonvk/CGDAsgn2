using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景管理器
/// 1.可以同步加载场景，可带回调
/// 2.可以异步加载场景，可带回调
/// ps:之所以需要这个类，可以让一个对象在加载场景后，可以在在自己的脚本里书写一些场景加载后，自己需要做的事情，这样做可以 减少耦合，增强内聚
/// </summary>
public class ScenesMgr : SingleMgr<ScenesMgr>
{
    /// <summary>
    /// 同步加载场景（名字）
    /// </summary>
    /// <param name="SceneName">场景名</param>
    /// <param name="fun">加载完成后的回调</param>
    public void LoadScene(string SceneName, UnityAction fun = null)
    {
        SceneManager.LoadScene(SceneName);
        if(fun != null)
            fun();
    }

    /// <summary>
    /// 异步加载场景（名字）
    /// </summary>
    /// <param name="SceneName">场景名</param>
    /// <param name="fun">加载完成后的回调</param>
    public void LoadSceneAsync(string SceneName, UnityAction fun = null)
    {
        MonoMgr.GetInstance().StartCoroutine(LoadAsync(SceneName, fun));
    }

    IEnumerator LoadAsync(string SceneName, UnityAction fun = null)
    {

        AsyncOperation temp = SceneManager.LoadSceneAsync(SceneName);

        while(!temp.isDone)
        {
            EventsCenter.GetInstance().TriggerEvent("当前场景加载进度", temp.progress);
            yield return temp.progress;
        }

        if(fun != null)
            fun();
    }

    /// <summary>
    /// 同步加载场景（下标）
    /// </summary>
    /// <param name="SceneIndex">场景下标</param>
    /// <param name="fun">加载完成后的回调</param>
    public void LoadScene(int SceneIndex, UnityAction fun = null)
    {
        SceneManager.LoadScene(SceneIndex);
        if (fun != null)
            fun();
    }

    /// <summary>
    /// 异步加载场景（下标）
    /// </summary>
    /// <param name="SceneIndex">场景下标</param>
    /// <param name="fun">加载完成后的回调</param>
    public void LoadSceneAsync(int SceneIndex, UnityAction fun = null)
    {
        MonoMgr.GetInstance().StartCoroutine(LoadAsync(SceneIndex, fun));
    }

    IEnumerator LoadAsync(int SceneIndex, UnityAction fun = null)
    {
        AsyncOperation temp = SceneManager.LoadSceneAsync(SceneIndex);

        while (!temp.isDone)
        {
            EventsCenter.GetInstance().TriggerEvent("当前场景加载进度", temp.progress);
            yield return temp.progress;
        }

        if (fun != null)
            fun();
    }
}
