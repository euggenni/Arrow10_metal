// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MilitaryCombatSimulator;

public class SpawnscriptC3 : MonoBehaviour
{


    void Update()
    {

    }
	
    void OnServerInitialized()
    {
        MCSGlobalSimulation.InitializeServer();
        //MCSGlobalSimulation.Instantiate<WeaponryTank_Strela10>();
        //MCSGlobalSimulation.Instantiate(null);
    }


    void OnPlayerConnected(NetworkPlayer player)
    {
    }

  

    void OnGUI()
    {
    }


    void OnPlayerDisconnected(NetworkPlayer player)
    {
    }

    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        Debug.Log(MCSSerializer.SerializeToString(MCSGlobalSimulation.Log));

        //Debug.Log("Clean up a bit after server quit");
        //Network.RemoveRPCs(Network.player);
        //Network.DestroyPlayerObjects(Network.player);

        /* 
        * Note that we only remove our own objects, but we cannot remove the other players 
        * objects since we don't know what they are; we didn't keep track of them. 
        * In a game you would usually reload the level or load the main menu level anyway ;).
        * 
        * In fact, we could use "Application.LoadLevel(Application.loadedLevel);" here instead to reset the scene.
        */
        //Application.LoadLevel(Application.loadedLevel);
    }
}