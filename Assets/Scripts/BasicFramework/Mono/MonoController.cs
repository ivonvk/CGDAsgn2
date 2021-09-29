using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Mono控制器
/// </summary>
public class MonoController : MonoBehaviour
{
    private event UnityAction updateEvent;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if(updateEvent != null)
            updateEvent();
    }

    public void AddUpdateListener(UnityAction fun)
    {
        updateEvent += fun;
    }

    public void RemoveUpdateListener(UnityAction fun)
    {
        updateEvent -= fun;
    }
}
