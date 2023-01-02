using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkCommandLine : MonoBehaviour
{
    private NetworkManager netManager;
    private static string val = "";
    private static bool getIP = false;

    void Start()
    {
        netManager = GetComponentInParent<NetworkManager>();

        if (Application.isEditor) return;

        var args = GetCommandlineArgs();

        if (args.TryGetValue("-mode", out string mode))
        {
            switch (mode)
            {
                case "server":
                    netManager.StartServer();
                    break;
                case "host":
                    netManager.StartHost();
                    break;
                case "client":

                    netManager.StartClient();
                    break;
            }
        }
    }

    private Dictionary<string, string> GetCommandlineArgs()
    {
        Dictionary<string, string> argDictionary = new Dictionary<string, string>();

        var args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; ++i)
        {
            var arg = args[i].ToLower();
            if (arg.StartsWith("-"))
            {
                var value = i < args.Length - 1 ? args[i + 1].ToLower() : null;
                value = (value?.StartsWith("-") ?? false) ? null : value;

                argDictionary.Add(arg, value);
            }
        }
        return argDictionary;
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 200, 200));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer && !getIP)
        {
            StartButtons();
        }
        else if (getIP)
        {
            GetServerIP();
        }
        else 
        {
            StatusLabels();
        }

        GUILayout.EndArea();
    }

    static void StartButtons()
    {
        if (GUILayout.Button("Host (Windows Only)")) NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client")) NetworkCommandLine.getIP = true;
        if (GUILayout.Button("Server (Windows Only)")) NetworkManager.Singleton.StartServer();
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }
    
    static void GetServerIP()
    {
        NetworkCommandLine.val = GUILayout.TextField(NetworkCommandLine.val);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(val, 7777);
        if (GUILayout.Button("Connect")) NetworkManager.Singleton.StartClient();
    }
}