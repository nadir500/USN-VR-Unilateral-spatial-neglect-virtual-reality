﻿using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public class GameClient : MonoBehaviour, INetEventListener
{
    //a class to manage a udp connection (very smooth) to the server || sending and receiving data from it 
    //you can make a hotspot network and connect the server plus a client to it and it will automatically send recieve data through udp  
    //using of course Litenet Lib from github https://github.com/RevenantX/LiteNetLib and manage it to work perfectly with the project 

    private NetManager _netClient; //the client peer 
    private NetPeer _serverPeer; //the server peer 
    private int isCross = 0; //to manage the congrats audio by the DR 
    private AudioController audioController;  //audio controller from the scene 
    void Start()
    {
        audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
        _netClient = new NetManager(this);
        _netClient.Start();
        _netClient.UpdateTime = 50;
    }

    void Update()
    {
        //receive all pending updates 
        _netClient.PollEvents();
        _serverPeer = _netClient.GetFirstPeer();
        if (_serverPeer != null && _serverPeer.ConnectionState == ConnectionState.Connected)
        {
            //making somethinf if connected to the server 
        }
        else
        {
            _netClient.SendDiscoveryRequest(new byte[] { 1 }, 5000);
        }
    }

    public void SendDataToServer(bool fadeCondition)
    {
        NetDataWriter _dataWriter = new NetDataWriter();
        if (_serverPeer != null)
        {
            _dataWriter.Put(fadeCondition);
            _serverPeer.Send(_dataWriter, DeliveryMethod.Sequenced);
        }
    }

    void OnDestroy()
    {
        if (_netClient != null)
            _netClient.Stop();
    }
    /**********************************Here is the interface functions we use when receiving data/lost connection/on connected with server *********************************/
    public void OnPeerConnected(NetPeer peer)
    {
        Debug.Log("[CLIENT] We connected to " + peer.EndPoint);
    }

    public void OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
    {
        Debug.Log("[CLIENT] We received error " + socketErrorCode);
    }

    public void OnNetworkReceive(NetPeer peer, NetDataReader reader, DeliveryMethod deliveryMethod)
    {
        RoadController.fadeout_after_crossing = reader.GetBool();
        Debug.Log("is CROSS " + isCross);
        if (isCross == 0)
        {
            audioController.playAudioClip("DRSounds/Congrats_Midwalk", 0, -1);
            isCross = 1;
        }
        else
        {
            audioController.playAudioClip("DRSounds/TouchpadInstructions", 0, 15);

        }


    }

    public void OnNetworkReceiveUnconnected(NetEndPoint remoteEndPoint, NetDataReader reader,
        UnconnectedMessageType messageType)
    {
        if (messageType == UnconnectedMessageType.DiscoveryResponse && _netClient.PeersCount == 0)
        {
            Debug.Log("[CLIENT] Received discovery response. Connecting to: " + remoteEndPoint);
            _netClient.Connect(remoteEndPoint, "sample_app");
        }
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {

    }

    public void OnConnectionRequest(ConnectionRequest request)
    {

    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Debug.Log("[CLIENT] We disconnected because " + disconnectInfo.Reason);
    }
    /****************************************END*******************************************/
}