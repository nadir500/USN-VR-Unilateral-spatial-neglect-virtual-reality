using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public class GameClient : MonoBehaviour, INetEventListener
{
    private NetManager _netClient;
    private NetPeer _serverPeer;
    private NetDataWriter _dataWriter;
    private int isCross = 0;
    private AudioController audioController;
    void Start()
    {
        _netClient = new NetManager(this);
        _dataWriter= new NetDataWriter();
        _netClient.Start();
        _netClient.UpdateTime = 50;

        audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
     

    }

    void Update()
    {
        //method
        _netClient.PollEvents();
        _serverPeer = _netClient.GetFirstPeer();
        if (_serverPeer != null && _serverPeer.ConnectionState == ConnectionState.Connected)
        {
        }
        else
        {
            _netClient.SendDiscoveryRequest(new byte[] { 1 }, 5000);
        }
    }

    public void SendDataToServer(bool fadeCondition)
    {
        if(_serverPeer!=null)
        {
        _dataWriter.Put(fadeCondition);
        _serverPeer.Send(_dataWriter,DeliveryMethod.Sequenced);
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
        
        if (isCross == 0)
        {
            audioController.playAudioClip("Congrats_Midwalk",0,0);
             isCross =1;
        }
      /*  else
        {
            StartCoroutine(GameObject.Find("FadeGameObject").GetComponent<Fading>().playSound("ThankYou"));            
        }*/

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