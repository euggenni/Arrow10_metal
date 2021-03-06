﻿using MilitaryCombatSimulator;
using UnityEngine;

/// <summary>
/// Класс для обработки серверных событий
/// </summary>
public class MCSServerHandler : MonoBehaviour {
    public void Execute(NetworkPlayer sender, MCSCommand cmd) {
        if (cmd.Command.Equals("Execute"))
            MCSGlobalSimulation.Players.List[sender].Weaponry.Execute(cmd);
            Debug.Log("Execute команда прошла " + cmd.Args[0].ToString());
    }

    void OnPlayerConnected(NetworkPlayer player) {
        MCSAccountInfo accountInfo = new MCSAccountInfo("Вася", "Пупкин", "Пупец");

        MCSGlobalSimulation.Players.List.Add(player, new MCSPlayer(player, accountInfo));
        Debug.Log("Добавлен игрок " + MCSGlobalSimulation.Players.List[player].Account.FirstName);
    }

    void OnPlayerDisconnected(NetworkPlayer player) {
    }

    void OnDisconnectedFromServer(NetworkDisconnection info) {
        //Debug.Log("Clean up a bit after server quit");
        Network.RemoveRPCs(Network.player);

        if (Network.isClient)
            Network.DestroyPlayerObjects(Network.player);

        /* 
        * Note that we only remove our own objects, but we cannot remove the other players 
        * objects since we don't know what they are; we didn't keep track of them. 
        * In a game you would usually reload the level or load the main menu level anyway ;).
        * 
        * In fact, we could use "Application.LoadLevel(Application.loadedLevel);" here instead to reset the scene.
        */
        Application.LoadLevel(Application.loadedLevel);
    }

    #region Методы

    /*
     *  OnLevelWasLoaded	 This function is called after a new level was loaded. 
        OnEnable	 This function is called when the object becomes enabled and active. 
        OnDisable	 This function is called when the behaviour becomes disabled () or inactive. 
        OnDestroy	 This function is called when the MonoBehaviour will be destroyed. 
        OnPreCull	 OnPreCull is called before a camera culls the scene. 
        OnPreRender	 OnPreRender is called before a camera starts rendering the scene. 
        OnPostRender	 OnPostRender is called after a camera finished rendering the scene. 
        OnRenderObject	 OnRenderObject is called after camera has rendered the scene. 
        OnWillRenderObject	 OnWillRenderObject is called once for each camera if the object is visible. 
        OnGUI	 OnGUI is called for rendering and handling GUI events. 
        OnRenderImage	 OnRenderImage is called after all rendering is complete to render image 
        OnDrawGizmosSelected	 Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected. 
        OnDrawGizmos	 Implement this OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn. 
        OnApplicationPause	 Sent to all game objects when the player pauses. 
        OnApplicationFocus	 Sent to all game objects when the player gets or looses focus. 
        OnApplicationQuit	 Sent to all game objects before the application is quit. 
        OnPlayerConnected	 Called on the server whenever a new player has successfully connected. 
        OnServerInitialized	 Called on the server whenever a Network.InitializeSimulation was invoked and has completed. 
        OnConnectedToServer	 Called on the client when you have successfully connected to a server 
        OnPlayerDisconnected	 Called on the server whenever a player disconnected from the server. 
        OnDisconnectedFromServer	 Called on the client when the connection was lost or you disconnected from the server. 
        OnFailedToConnect	 Called on the client when a connection attempt fails for some reason. 
        OnFailedToConnectToMasterServer	 Called on clients or servers when there is a problem connecting to the MasterServer. 
        OnMasterServerEvent	 Called on clients or servers when reporting events from the MasterServer. 
        OnNetworkInstantiate	 Called on objects which have been network instantiated with Network.Instantiate 
        OnSerializeNetworkView	 Used to customize synchronization of variables in a script watched by a network view.
     */

    #endregion
}