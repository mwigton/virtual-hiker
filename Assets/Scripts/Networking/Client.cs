using UnityEngine;
using System.Collections;

public class Client
{
    public Connection connection = new Connection();

    public Client(string ip, int port)
    {
        connection.ipAddress = ip;
        connection.port = port;

        connectClient();
    }

    public void connectClient(string ip, int port)
    {
        Network.Connect(ip, port);
        DebugOutput.Log("Connecting to " + ip + ":" + port + "... ");
    }

    public void connectClient()
    {
        if (connection.ipAddress != null && connection.port != 0)
        {
            Network.Connect(connection.ipAddress, connection.port);
            DebugOutput.Log("Connecting to " + connection.ipAddress + ":" + connection.port + "... ");
        }
        else
        {
            if (connection.ipAddress == null)
            {
                if (connection.port == 0)
                {
                    DebugOutput.Log("");
                }

            }
            DebugOutput.Log("Cannot connect because connection settings null");
        }
    }
}
