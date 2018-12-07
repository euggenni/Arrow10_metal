// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

#pragma warning disable 0414, 0108
#pragma warning disable 0618, 0168

public class ConnectC3 : MonoBehaviour
{
    public string connectToIP = "127.0.0.1";
    public int connectPort = 25001;
    private int i = 0;


    private UniSkyAPI uniSky;

    void Awake()
    {
        // Define instance
       

        // Functions to interpolate parameters over time
        /*
        uniSky.LerpCloudCover(0.5f, 5000.0f);
        uniSky.LerpPrecipitationLevel(0.6f, 5000.0f);
        uniSky.LerpStormCloudCover(-1.0f, 10000.0f);
        uniSky.LerpRainLevel(100, 0.2f, 10000.0f);
        uniSky.LerpStormLevel(150, 0.4f, 20000.0f);
        uniSky.LerpSunIntensity(0.2f, 10000.0f);
        uniSky.LerpFogLevel(0.02f, 20000.0f);
        uniSky.LerpAmbientLighting(new Color(0.0f, 0.0f, 0.0f, 0.0f), 5000);
        uniSky.ClearDropletBuffer();
        uniSky.LerpDropletLevel(10, 20000.0f);
        */
    }

    private bool connected;

    private IEnumerator EstablishConnection()
    {
        while (!connected)
        {
            Network.Connect(connectToIP, connectPort);

            yield return new WaitForSeconds(1.5f);
        }    
    }

    void Start()
    {
        Network.useNat = false;
        //StartCoroutine(EstablishConnection());
    }

    // Î÷åâèäíî, ÷òî ãðàôè÷åñêèé èíòåðôåéñ äëÿ êëèåíòà è ñåðâåðà (mixed!)
    void OnGUI()
    {

        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            //We are currently disconnected: Not a client or host
            GUILayout.Label("Connection status: Disconnected");

            connectToIP = GUILayout.TextField(connectToIP, GUILayout.MinWidth(100));
            connectPort = int.Parse(GUILayout.TextField(connectPort.ToString()));

            GUILayout.BeginVertical();
            if (GUILayout.Button("Connect as client"))
            {
                //Connect to the "connectToIP" and "connectPort" as entered via the GUI
                //Ignore the NAT for now
                Network.useNat = false;
                Network.Connect(connectToIP, connectPort);
            }
            GUILayout.EndVertical();
        }
        else
        {
            //Ó íàñ åñòü ñâÿçü(è)!

            if (Network.peerType == NetworkPeerType.Connecting)
            {

                GUILayout.Label("Connection status: Connecting");

            }
            else if (Network.peerType == NetworkPeerType.Client)
            {
                GUILayout.Label("Connection status: Client!");
                GUILayout.Label("Ping is: " + Network.GetAveragePing(Network.connections[0]));

                connected = true;
            }
            else if (Network.peerType == NetworkPeerType.Server)
            {

                GUILayout.Label("Connection status: Server!");
                GUILayout.Label("Connections: " + Network.connections.Length);
                if (Network.connections.Length >= 1)
                {
                    GUILayout.Label("Ping to first player: " + Network.GetAveragePing(Network.connections[0]));
                }
            }

            if (GUILayout.Button("Disconnect"))
            {
                Network.Disconnect(200);
            }
        }


    }


    // NONE of the functions below is of any use in this demo, the code below is only used for demonstration.
    // First ensure you understand the code in the OnGUI() function above.

    //Client functions called by Unity
    void OnConnectedToServer()
    {
        Debug.Log("This CLIENT has connected to a server");
    }

    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        Debug.Log("This SERVER OR CLIENT has disconnected from a server");
        Application.LoadLevel((Application.loadedLevel));
    }

    void OnFailedToConnect(NetworkConnectionError error)
    {
        Debug.Log("Could not connect to server: " + error);
    }


    //Server functions called by Unity
    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player connected from: " + player.ipAddress + ":" + player.port);
    }

    void OnServerInitialized()
    {
        Debug.Log("Server initialized and ready");
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Debug.Log("Player disconnected from: " + player.ipAddress + ":" + player.port);
    }


    // OTHERS:
    // To have a full overview of all network functions called by unity
    // the next four have been added here too, but they can be ignored for now

    void OnFailedToConnectToMasterServer(NetworkConnectionError info)
    {
        Debug.Log("Could not connect to master server: " + info);
    }

    void OnNetworkInstantiate(NetworkMessageInfo info)
    {

        Debug.Log("New object instantiated by " + info.sender);

        //		if (networkView.isMine)
        //		{
        //			//Camera.main.SendMessage("SetTarget", transform);
        //			GetComponent(NetworkInterpolatedTransform).enabled = false;
        //			Find("Camera").GetComponent(MouseOrbitAroundTargetJS).target = myNewTrans;
        //		}
        //		// This is just some remote controlled player
        //		else
        //		{
        //			this.name += "Remote";
        //			GetComponent(TankTrackController).enabled = false;
        //			//GetComponent(ThirdPersonSimpleAnimation).enabled = false;
        //			GetComponent(NetworkInterpolatedTransform).enabled = true;
        //		}
    }


    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        //Custom code here (your code!)
    }

    /* 
     The last networking functions that unity calls are the RPC functions.
     As we've added "OnSerializeNetworkView", you can't forget the RPC functions 
     that unity calls..however; those are up to you to implement.
 
     [RPC]
     void  MyRPCKillMessage (){
        //Looks like I have been killed!
        //Someone send an RPC resulting in this function call
     }
    */

}

