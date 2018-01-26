using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public class GameClient : MonoBehaviour, INetEventListener
{
    private NetManager _netClient;

    void Start()
    {
        _netClient = new NetManager(this);
        _netClient.Start();
        _netClient.UpdateTime = 50;
    }

    void Update()
    {
        //method
        _netClient.PollEvents();
        var peer = _netClient.GetFirstPeer();
        if (peer != null && peer.ConnectionState == ConnectionState.Connected)
        {
           
        }
        else
        {
            _netClient.SendDiscoveryRequest(new byte[] {1}, 5000);
        }
    }


    void OnDestroy()
    {
        if (_netClient != null)
            _netClient.Stop();
    }

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
}