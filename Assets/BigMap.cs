using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMap : MonoBehaviour
{
    public GameObject BigMapUI;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            BigMapUI.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            BigMapUI.SetActive(false);
        }
    }
}
