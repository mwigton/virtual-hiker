using UnityEngine;
using System.Collections;
using System.Net;

public static class GlobalSettings
{
    #region Network
    public static class NetworkSettings
    {
        public enum NetworkMode
        {
            standalone,
            host,
            client
        }
        public static NetworkMode networkMode;

        public static int networkPort = 23000;
        public static string gameName = "VirtualHiker";
        public static string gameTypeName = "Walking";
        public static string serverConnectIP;
        public static IPAddress groupAddress = IPAddress.Parse("224.0.0.224");
        public static int startupPort = 25000;
    }
    #endregion 
}
