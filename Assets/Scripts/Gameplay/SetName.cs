using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetName : MonoBehaviour
{

    public static SetName Instance;
    InputField Nameinput;
    void Start()
    {
        Instance = this;
        Nameinput = GetComponent<InputField>();
        Nameinput.text = "Player "+Random.Range(0, 1000).ToString();
    }
    void Update()
    {
        if (PhotonNetwork.insideLobby)
        {
            PhotonNetwork.playerName = Nameinput.text;
        }
    }

}
