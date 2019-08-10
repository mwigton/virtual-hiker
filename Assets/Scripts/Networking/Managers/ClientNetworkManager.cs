using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;

[RequireComponent(typeof(NetworkView))]
public class ClientNetworkManager : MonoBehaviour
{
    #region Variables

    public static ClientNetworkManager singleton;

    string serverIp;

    Client client;

    IPEndPoint remote_end;
    UdpClient udp_client;

    #endregion

    public event EventHandler Step;

    void Awake()
    {

        if (singleton == null) singleton = this;

        DebugOutput.Log("Console init");
        DebugOutput.Log("==================");
        DebugOutput.Log(networkView.viewID.ToString());
    }

    void Start() 
    {
        FindServers();
    }

    public void OnFailedToConnect(NetworkConnectionError error)
    {
        DebugOutput.LogError("Failed to connect to " + serverIp + ":" + GlobalSettings.NetworkSettings.networkPort + " : " + error.ToString());
    }

    void OnConnectedToServer()
    {
        DebugOutput.Log("Connected to " + serverIp, color: Color.green);
    }


    void FindServers()
    {
        // multicast receive setup
        remote_end = new IPEndPoint(IPAddress.Any, GlobalSettings.NetworkSettings.startupPort);
        udp_client = new UdpClient(remote_end);
        udp_client.JoinMulticastGroup(GlobalSettings.NetworkSettings.groupAddress);

        DebugOutput.Log("Looking for Host...");

        udp_client.BeginReceive(new AsyncCallback(ServerLookup), null);
        StartCoroutine("MakeConnection");
    }

    void ServerLookup(IAsyncResult ar)
    {
        // receivers package and identifies IP
        udp_client.EndReceive(ar, ref remote_end);
        serverIp = remote_end.Address.ToString();
    }

    IEnumerator MakeConnection()
    {
        // Waits untill we get the server ip from Broadcast packets, then trys to connect
        while (serverIp == null) { yield return new WaitForEndOfFrame(); }

        DebugOutput.Log("Found server: " + serverIp);
        client = new Client(serverIp, GlobalSettings.NetworkSettings.networkPort);
        if (client == null) { serverIp = null; }
    }

    [RPC]
    public void ReceiveStep()
    {
        Step(this, null);
    }

}
