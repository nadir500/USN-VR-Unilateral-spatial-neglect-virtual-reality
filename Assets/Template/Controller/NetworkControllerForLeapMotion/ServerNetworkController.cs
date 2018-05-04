using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerNetworkController : NetworkDiscovery {
    
    /*this class is for making a server inside our app and then connect this server with a client (you can call it p2p if you want) 
      the client actually is the leap motion device connected to a certain network and the app should be on the same network also 
     */

    void Start()
    {
        Application.runInBackground = true; //make it run in background for connecting 
        startServer();
        NetworkManager.singleton.StartHost();
    }

    //Call to create a leap motion server (so simple)
    public void startServer()
    {
        int serverPort = createServer(); //create server port for the app
        if (serverPort != -1)
        {
            Debug.Log("Server created on port : " + serverPort);
            broadcastData = serverPort.ToString();
            Initialize(); //it's a network discovery function to start broadcasting on the network 
            StartAsServer();  //define the app as the leap motion server 
         }
        else
        {
            Debug.Log("Failed to create Server"); //can't create the server 
        }
    }
    //defining the ports for the server to generate
    int minPort = 10000;
    int maxPort = 10010;
    int defaultPort = 10000;

    //Creates a server then returns the port the server is created with. Returns -1 if server is not created
    private int createServer()
    {
        int serverPort = -1;
        //Connect to default port
        bool serverCreated = NetworkServer.Listen(defaultPort);
        if (serverCreated)
        {
            serverPort = defaultPort;
            Debug.Log("Server Created with deafault port");
        }
        else
        {
            Debug.Log("Failed to create with the default port");
            //Try to create server with other port from min to max except the default port which we tried already
            for (int tempPort = minPort; tempPort <= maxPort; tempPort++)
            {
                //Skip the default port since we have already tried it
                if (tempPort != defaultPort)
                {
                    //Exit loop if successfully create a server
                    if (NetworkServer.Listen(tempPort))
                    {
                        serverPort = tempPort;
                        break;
                    }
                    //If this is the max port and server is not still created, show, failed to create server error
                    if (tempPort == maxPort)
                    {
                        Debug.LogError("Failed to create server");
                    }
                }
            }
        }
        return serverPort;
    }
   
}
