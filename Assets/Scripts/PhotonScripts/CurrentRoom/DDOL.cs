using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOL : MonoBehaviour
{
    public static DDOL Instance;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Screen.SetResolution(1366, 768, false);

    }
}
