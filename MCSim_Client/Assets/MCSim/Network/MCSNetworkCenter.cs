using UnityEngine;

public class MCSNetworkCenter : MonoBehaviour {
    void OnConnectedToServer() {
        UICenter.Panels.CloseAll();
        MCSGlobalSimulation.Players.List.Add(Network.player,
            new MCSPlayer(Network.player, new MCSAccountInfo("User", "User", "GORO")));

        var panel = UICenter.Panels.Instantiate("PanelTrainingMode");
        panel.Show();
    }
}