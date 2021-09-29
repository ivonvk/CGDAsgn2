using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PoolData
{
    public List<GameObject> objList;
    public GameObject father;

    public PoolData(GameObject obj, GameObject pool)
    {
        objList = new List<GameObject>() { obj };
        father = new GameObject(obj.name);
        father.transform.SetParent(pool.transform);
        obj.transform.SetParent(father.transform);

        obj.SetActive(false);
    }

    public void PushObj(GameObject obj)
    {
        obj.transform.SetParent(father.transform);
        objList.Add(obj);
        obj.SetActive(false);
    }
    
    public GameObject GetObj()
    {
        GameObject tempObj = objList[0];
        objList.RemoveAt(0);
        tempObj.SetActive(true);
        tempObj.transform.SetParent(null);
        return tempObj;
    }
}

/// <summary>
/// 缓存池
/// 1.可以将对象存入或取出缓存池
/// 2.清空缓存池
/// </summary>
public class PoolMgr : SingleMgr<PoolMgr>
{

    private Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

    private GameObject poolObj;

    /// <summary>
    /// 从缓存池取出对象（父类至空）
    /// </summary>
    /// <param name="path">对象名字</param>
    /// <param name="callBack">返回对象的函数</param>
    public void GetObj(string path, UnityAction<GameObject> callBack)
    {
        if (poolDic.ContainsKey(path) && poolDic[path].objList.Count > 0)
            callBack(poolDic[path].GetObj());
        else
        {
            GameObject tempObj = GameObject.Instantiate(Resources.Load<GameObject>(path));  //TODO:用资源管理器加载
            tempObj.name = path;
            tempObj.transform.SetParent(null);
            callBack(tempObj);
        }
    }

    /// <summary>
    /// 将对象存入缓存池
    /// </summary>
    /// <param name="obj">需要存入的对象</param>
    public void PushObj(GameObject obj)
    {
        if (poolObj == null)
        {
            poolObj = new GameObject("Pool");

            GameObject.DontDestroyOnLoad(poolObj);
        }

        if (!poolDic.ContainsKey(obj.name))
            poolDic.Add(obj.name, new PoolData(obj, poolObj));
        else
            poolDic[obj.name].PushObj(obj);
    }

    /// <summary>
    /// 清空字典
    /// ps：场景切换时可以调用
    /// </summary>
    public void ClearDic()
    {
        poolDic.Clear();
    }
}
