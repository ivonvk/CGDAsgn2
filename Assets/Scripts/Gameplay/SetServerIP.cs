using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetServerIP : MonoBehaviour
{

    public static SetServerIP Instance;
    InputField ServerIPinput;
    void Start()
    {
        Instance = this;
        ServerIPinput = GetComponent<InputField>();
        if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.SelfHosted)
        {
            ServerIPinput.text = PhotonNetwork.PhotonServerSettings.ServerAddress;
        }
    }
    void Update()
    {
        if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.SelfHosted)
        {
            PhotonNetwork.PhotonServerSettings.ServerAddress = ServerIPinput.text;
        }
        else if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.PhotonCloud)
        {
            ServerIPinput.text = "Connected to the Photon Cloud!";
        }
    }
}
