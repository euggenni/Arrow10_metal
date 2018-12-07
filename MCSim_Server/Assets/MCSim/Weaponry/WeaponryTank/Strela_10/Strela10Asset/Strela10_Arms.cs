using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public class Strela10_Arms : MonoBehaviour
{
    /// <summary>
    /// Контейнеры
    /// </summary>
    [SerializeField]
    public WeaponryArms_Strela10_Launcher[] Launchers;

    private bool comeIn = false;
    public WeaponryArms GetLauncher(int id)
    {
        return Launchers[id - 1];
    }

    // Use this for initialization
    void Awake()
    {
        int i = 0;
        foreach (WeaponryArms_Strela10_Launcher launcher in Launchers)
        {
            launcher.Index = i;
            launcher.Arms = this;
            i++;

           
        }
    }
    [RPC]
    public  void CheckLaunch()
    {
        comeIn = true;
    }
    [RPC]
    public void BelongingTargetCall(NetworkViewID id)
    {
        Debug.LogError("TargetCall server Id = " + id);
        networkView.RPC("BelongingTargetCall", RPCMode.Others, id);
    }
    [RPC]
    public void Shoot(int launcherIndex)
    {
        WeaponryArms_Strela10_Launcher launcher = Launchers[launcherIndex];

        if (Network.isClient)
        {
            networkView.RPC("Shoot", RPCMode.Server, launcherIndex);
        }
        else // Сервер 
        {
            if (launcher.Projectile)
            {

                var strela = gameObject.GetComponentInParents<WeaponryTank_Strela10>(false);
                //strela.TowerHandler.ShootWithPrediction((launcher.Projectile as WeaponryRocket).Target);
                // TODO: обратился к Strela10_TowerHandler
                // Вызвать ShootWithPrediction метод,  который передаю цель, а он смещает установку
                if(comeIn)
                {
                    (launcher.Projectile as WeaponryRocket).LaunchWithNotLie();
                }
                else
                {
                    (launcher.Projectile as WeaponryRocket).Launch();
                }
                launcher.Projectile = null;
                comeIn = false;
            }
        }
    }

    [RPC]
    public void Reload(int launcherIndex)
    {
        if (Network.isServer)
        {
            Launchers[launcherIndex].Reload();
        }

        if (Network.isClient)
        {
            networkView.RPC("Reload", RPCMode.Server, launcherIndex);
        }
    }

    public void LoadAll<T>() where T : WeaponryProjectile
    {
        foreach (var launcher in Launchers)
        {
            launcher.ChargeProjectile<T>();
        }
    }
}
