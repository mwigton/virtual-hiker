using System.Collections;
using System.Net.Sockets;
using UnityEngine;
using System.Text;
using System.Net;
using System;

[RequireComponent(typeof(NetworkView))]
public class HostNetworkManager : MonoBehaviour
{

    Server server;
    bool clientConnected;
    UdpClient udp_client;
    void Awake()
    {
        DebugOutput.Log("Console init");
        DebugOutput.Log("==================");
        DebugOutput.Log(networkView.viewID.ToString());
    }

    void Start()
    {
        server = new Server(GlobalSettings.NetworkSettings.networkPort);
        StepDetector.singleton.Step += SendStep;
    }

    void SendStep(object sender, EventArgs e)
    {
        //DebugOutput.Log("Sending Step");
        networkView.RPC("ReceiveStep", RPCMode.Others);
    }

    void OnServerInitialized()
    {
        DebugOutput.Log("Debug path: " + Application.persistentDataPath);
        DebugOutput.Log("Server started on " + Network.player.ipAddress + ":" + GlobalSettings.NetworkSettings.networkPort, color: Color.green);
        StartCoroutine("StartBroadcast");
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        DebugOutput.Log("Client connected from " + player.ipAddress, color: Color.green);
        clientConnected = true;
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        DebugOutput.LogError("Client disconnected from " + player.ipAddress);
        clientConnected = false;
        StartCoroutine("StartBroadcast");
    }

    IEnumerator StartBroadcast()
    {
        // multicast send setup
        udp_client = new UdpClient();
        udp_client.JoinMulticastGroup(GlobalSettings.NetworkSettings.groupAddress);
        IPEndPoint remote_end = new IPEndPoint(GlobalSettings.NetworkSettings.groupAddress, GlobalSettings.NetworkSettings.startupPort);

        DebugOutput.Log("Broadcasting Server on port " + GlobalSettings.NetworkSettings.startupPort);

        // sends multicast
        while (!clientConnected)
        {
            var buffer = Encoding.ASCII.GetBytes(GlobalSettings.NetworkSettings.gameName);
            udp_client.Send(buffer, buffer.Length, remote_end);
            yield return new WaitForSeconds(1);
        }
    }

    void OnApplicationQuit()
    {
        Debug.Log("Quitting...");
        udp_client.Client.Close();
        udp_client.Close();
        Network.Disconnect();
        //stepManager.debugOutputFile.Close();
    }

    [RPC]
    public void ReceiveStep()
    {
    }
}
