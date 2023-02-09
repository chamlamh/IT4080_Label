using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System;
using UnityEditor.PackageManager;

public class Main : NetworkBehaviour
{
    public It4080.NetworkSettings netSettings;

    // Start is called before the first frame update
    void Start()
    {
        netSettings.startServer += NetSettingOnServerStart;
        netSettings.startClient += NetSettingOnClientStart;
        netSettings.startHost += NetSettingOnHostStart;


    }

    private void startClient(IPAddress ip, ushort port)
    {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.ConnectionData.Address = ip.ToString();
        utp.ConnectionData.Port = port;

        NetworkManager.Singleton.OnClientConnectedCallback += ClientOnClientConnented;
        NetworkManager.Singleton.OnClientConnectedCallback += ClientOnClientDisconnected;

        NetworkManager.Singleton.StartClient();
        netSettings.hide();
        Debug.Log("started client");
    }


    private void startServer(IPAddress ip, ushort port)
    {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.ConnectionData.Address = ip.ToString();
        utp.ConnectionData.Port = port;

        NetworkManager.Singleton.OnClientConnectedCallback += HostOnClientConnented;
        NetworkManager.Singleton.OnClientConnectedCallback += HostOnClientDisconnected;

        NetworkManager.Singleton.StartServer();
        netSettings.hide();
        Debug.Log("started server");
    }

    private void startHost(IPAddress ip, ushort port)
    {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.ConnectionData.Address = ip.ToString();
        utp.ConnectionData.Port = port;

        NetworkManager.Singleton.OnClientConnectedCallback += HostOnClientConnented;
        NetworkManager.Singleton.OnClientConnectedCallback += HostOnClientDisconnected;

        NetworkManager.Singleton.StartHost();
        netSettings.hide();
        Debug.Log("started host");
    }

    private void printIs(string msg)
    {
        Debug.Log($"[{msg}]  server:{IsServer} host:{IsHost} client:{IsClient} owner:{IsOwner}");
    }

    private void HostOnClientConnented(ulong clientId)
    {
        Debug.Log($"Client connected:  {clientId}");
    }

    private void HostOnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client disconnected:  {clientId}");
    }
    private void ClientOnClientDisconnected(ulong clientId)
    {   
    }

    private void ClientOnClientConnented(ulong clientId)
    {   
    }

    private void NetSettingOnHostStart(IPAddress ip, ushort port)
    {
        startServer(ip, port);
    }

    private void NetSettingOnClientStart(IPAddress ip, ushort port)
    {
        startHost(ip, port);
    }

    private void NetSettingOnServerStart(IPAddress ip, ushort port)
    {
        startClient(ip, port);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
