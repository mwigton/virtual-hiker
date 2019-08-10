using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{

    public void HostButton()
    {
        GlobalSettings.NetworkSettings.networkMode = GlobalSettings.NetworkSettings.NetworkMode.host;
        Application.LoadLevel(1);
    }

    public void ClientButton()
    {
        GlobalSettings.NetworkSettings.networkMode = GlobalSettings.NetworkSettings.NetworkMode.client;
        Application.LoadLevel(2);
    }
}
