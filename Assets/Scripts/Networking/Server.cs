using UnityEngine;
using System.Collections;

public class Server
{
    public Connection connection = new Connection();

    public Server(int port)
    {
        connection.port = port;
        StartServer();
    }

    public void StartServer(int port)
    {
        DebugOutput.Log("Starting Server... ");
        Network.InitializeServer(1, port, false);        
    }

    public void StartServer()
    {
        DebugOutput.Log("Starting Server... ");
        Network.InitializeServer(1, connection.port, false);
    }
}
