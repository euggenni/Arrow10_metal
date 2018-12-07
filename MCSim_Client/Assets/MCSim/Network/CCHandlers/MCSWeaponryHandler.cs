using MilitaryCombatSimulator;
using UnityEngine;

public class MCSWeaponryHandler {
    public MCSWeaponryHandler(MCSCommandCenter cc) {
    }

    public void Execute(NetworkPlayer sender, MCSCommand cmd) {
        if (cmd.Command.Equals("Execute")) {
            if (Network.isServer)
                MCSGlobalSimulation.Players.List[sender].Weaponry.Execute(cmd);

            // Если клиент принимает команду с сервера, то первым аргументом идет ID Weaponry
            if (Network.isClient) {
                int weaponryID = (int) cmd.Args[0];

                if (MCSGlobalSimulation.Weapons.List.ContainsKey(weaponryID)) {
                    MCSGlobalSimulation.Weapons.List[weaponryID].Execute(cmd);
                }
            }
        }

        if (cmd.Command.Equals("SetRole")) {
            if (Network.isClient) {
                var weaponry = MCSGlobalSimulation.Weapons.List[(int) cmd.Args[0]];
                //try {
                (weaponry as IWeaponryControl).SetRole(Network.player,
                    cmd.Args[1].ToString());

                MCSPlayer.Me.Role = cmd.Args[1].ToString();
                MCSPlayer.Me.Weaponry = weaponry;
                //}
                //catch (Exception e) { Debug.LogError("Не удалось установить роль " + cmd.Args[0] + ". Ошибка: " + e.Message); }
            }
        }
    }
}