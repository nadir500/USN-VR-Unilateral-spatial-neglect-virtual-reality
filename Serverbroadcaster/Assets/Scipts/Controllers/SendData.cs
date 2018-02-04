using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LiteNetLib;
using LiteNetLib.Utils;

public class SendData : MonoBehaviour
{

    public Button serverButton;
    public Text serverInfo;
    ServerStart server;
    void Start()
    {
        server = GameObject.Find("Server").GetComponent<ServerStart>();
    }
    void OnGUI()
    {

            serverButton.interactable = server.fadeConfirm;
        if (server._ourPeer != null && server.fadeConfirm)
        {
            serverInfo.text = "Fade Ready Please Press the Button Down There " + server.fadeConfirm;
        }
        else
        if(server.disconnectedFromPeer)
		{
            serverInfo.text = "Not Reached Yet " + server.fadeConfirm;
            serverButton.interactable = false;
            

		}

    }
    public void SendDataBoolean()
    {
        NetPeer _ourPeer = server._ourPeer;
        NetDataWriter _dataWriter = server._dataWriter;
        bool fadeOut = false;
        if (_ourPeer != null)
        {
            _dataWriter.Put(fadeOut);
            _ourPeer.Send(_dataWriter, DeliveryMethod.Sequenced);
            Debug.Log("Sent :D ");
        }
		serverButton.interactable=false;

    }

}
