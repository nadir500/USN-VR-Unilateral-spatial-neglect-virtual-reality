using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public class ServerStart : MonoBehaviour, INetEventListener
{
    private NetManager _netServer;
    public NetPeer _ourPeer;
    public NetDataWriter _dataWriter;
    public bool fadeConfirm;
    public bool disconnectedFromPeer=false;
    void Start()
    {
        Debug.Log("DDD");
        fadeConfirm=false;
        _dataWriter = new NetDataWriter();
        _netServer = new NetManager(this, 100);
        _netServer.Start(5000);
        _netServer.DiscoveryEnabled = true;
        _netServer.UpdateTime = 15;
    }
	void Update()
	{
		_netServer.PollEvents();

	}  
  void OnDestroy()
    {
        if (_netServer != null)
            _netServer.Stop();
    }

    public void OnPeerConnected(NetPeer peer)
    {
        Debug.Log("[SERVER] We have new peer " + peer.EndPoint);
        disconnectedFromPeer=false;
        _ourPeer = peer;
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectReason reason, int socketErrorCode)
    {
        disconnectedFromPeer=true;
    }

    public void OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
    {
        Debug.Log("[SERVER] error " + socketErrorCode);
    }

    public void OnNetworkReceiveUnconnected(NetEndPoint remoteEndPoint, NetDataReader reader,
        UnconnectedMessageType messageType)
    {
        if (messageType == UnconnectedMessageType.DiscoveryRequest)
        {
            Debug.Log("[SERVER] Received discovery request. Send discovery response");
            _netServer.SendDiscoveryResponse(new byte[] {1}, remoteEndPoint);
        }
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
        request.AcceptIfKey("sample_app");
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Debug.Log("[SERVER] peer disconnected " + peer.EndPoint + ", info: " + disconnectInfo.Reason);
        disconnectedFromPeer = true;
        if (peer == _ourPeer)
            _ourPeer = null;
    }

    public void OnNetworkReceive(NetPeer peer, NetDataReader reader, DeliveryMethod deliveryMethod)
    {
        //Debug.Log("SAY WAAAAT " + reader.GetBool());
        fadeConfirm = reader.GetBool();
        Debug.Log("Still in first point  " + fadeConfirm);
    }
}
