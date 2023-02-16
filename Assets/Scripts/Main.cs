using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System;

public class Main : NetworkBehaviour
{
    private NetworkManager netManager;

    public It4080.NetworkSettings netSettings;
    public It4080.Chat Chat;

    // Start is called before the first frame update
    void Start()
    {
        netSettings.startServer += NetSettingOnServerStart;
        netSettings.startClient += NetSettingOnClientStart;
        netSettings.startHost += NetSettingOnHostStart;
        netSettings.setStatusText("Not Connented");

        Chat.sendMessage += ChatOnSendMessage;
        It4080.Chat.ChatMessage msg = new It4080.Chat.ChatMessage();
        msg.message = "Hello World";
        Chat.ShowMessage(msg);
    }

    private void ChatOnSendMessage(It4080.Chat.ChatMessage msg)
    {
        Chat.RequestSendMessageServerRpc(msg.message);
    }

    private void startClient(IPAddress ip, ushort port)
    {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.ConnectionData.Address = ip.ToString();
        utp.ConnectionData.Port = port;

        NetworkManager.Singleton.OnClientConnectedCallback += ClientOnClientConnented;
        NetworkManager.Singleton.OnClientDisconnectCallback += ClientOnClientDisconnected;


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
        NetworkManager.Singleton.OnClientDisconnectCallback += HostOnClientDisconnected;

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
        NetworkManager.Singleton.OnClientDisconnectCallback += HostOnClientDisconnected;

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
        netSettings.setStatusText($"Host {clientId}");
    }

    private void HostOnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client disconnected:  {clientId}");
    }
    private void ClientOnClientDisconnected(ulong clientId)
    {
        netSettings.setStatusText($"Client {clientId} Disconnected");
        Chat.SendChatMessageClientRpc($"Client {clientId} Disconnected");
    }

    private void ClientOnClientConnented(ulong clientId)
    {
        netSettings.setStatusText($"Connected as {clientId}");
        Chat.SendChatMessageClientRpc($"Client {clientId} connected");
    }

    private void NetSettingOnHostStart(IPAddress ip, ushort port)
    {
        startHost(ip, port);
    }

    private void NetSettingOnClientStart(IPAddress ip, ushort port)
    {
        startClient(ip, port);
        
    }

    private void NetSettingOnServerStart(IPAddress ip, ushort port)
    {
        startServer(ip, port);
    }
}
