using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
public class GameClient : MonoBehaviour, INetEventListener
{
    public TouchPadController touchPadController;
    
    //a class to manage a udp connection (very smooth) to the server || sending and receiving data from it 
    //you can make a hotspot network and connect the server plus a client to it and it will automatically send recieve data through udp  
    //using of course Litenet Lib from github https://github.com/RevenantX/LiteNetLib and manage it to work perfectly with the project 
    //public string result = "No Command";
    private string defaultResult = "";
    private string[] numbers=new string[9]{"one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
    public string result
    {
        get
        {
            return defaultResult;
        }
        set
        {
            Debug.Log("initialized");
            switch (value)
            {
                 case "one":
                case "two":
                case "three":
                case "four":
                case "five":
                case "six":
                case "seven":
                case "eight":
                case "nine":
                    {
                        value = (Array.IndexOf(numbers,value) + 1 ).ToString();
                        Debug.Log("Numbers");
                        touchPadController.putNumber(value);
                    break;
                    }
                case "finish":
                    {
                        touchPadController.Done();
                    break;
                    }
                case "append":
                    {
                        touchPadController.Add();
                    break;
                    }
                case "clear":
                    {
                        touchPadController.Clear();
                    break;
                    }
                case "left":
                case "right":
                    {
                        touchPadController.SelectObjectPosition(value);
                    break;
                    }
                default:
                    {
                        defaultResult = value;
                        break;
                    }
            }

        }

    }

    private NetManager _netClient; //the client peer 
    private NetPeer _serverPeer; //the server peer 
   // private AudioController audioController;  //audio controller from the scene 
    void Start()
    {
        //audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
        _netClient = new NetManager(this);
        _netClient.Start();
        _netClient.UpdateTime = 50;
        Application.targetFrameRate = 30;
    }

    void Update()
    {
        //receive all pending updates 
        _netClient.PollEvents();
        _serverPeer = _netClient.GetFirstPeer();
        if (_serverPeer != null && _serverPeer.ConnectionState == ConnectionState.Connected)
        {
            //make something if connected to the server 
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
        result = reader.GetString();
        Debug.Log("Command Result = " + result);
        //GET YOUR STRINGS FROM HERE 
        //
        //
        //
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