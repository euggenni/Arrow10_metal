using System.Collections.Generic;
using System.Text.RegularExpressions;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public class MCSCameraSwitcher : MonoBehaviour
{
    public UIPopupList list;

    void Awake()
    {
        //MCSGlobalSimulation.WeaponryInstantiatedEvent += OnWeaponryInstantiated;
        LoadWeaponryList();
        list.selection = "Свободная камера";
	}

    public void OnWeaponryInstantiated(Weaponry weaponry)
    {
        //Debug.Log("Свитчер ["+gameObject.name+"] принял Weaponry " + weaponry.Name);
        //if (weaponry is WeaponryProjectile) return;

        //list.items.Add("[" + weaponry.ID + "] " + weaponry.Name);
        LoadWeaponryList();
    }

    public void OnSelectedWeaponryChanged(string SelectedItem)
    {
        if (string.IsNullOrEmpty(SelectedItem)) return;

        if (SelectedItem.Equals("Свободная камера")) {
            SwitchToFreeCamera();
            return;
        }

        string pattern = "^(\\[)(\\d+)(\\])";
        Match match = Regex.Match(SelectedItem, pattern);

        string res;
        try
        {
            res = match.ToString().Substring(1, match.Length - 2);
        }
        catch
        {
            return;
        }

        int weapornyID;

        if(int.TryParse(res, out weapornyID))
        {
            try
            {
                Weaponry weaponry = MCSGlobalSimulation.Weapons.List[weapornyID];
                SwitchToWeaponryCamera(weaponry);
            } catch {}
        }
    }

    public static void SwitchToFreeCamera()
    {
        MouseOrbitAroundTarget moat = MCSUICenter.MainCamera.GetComponent<MouseOrbitAroundTarget>();
        moat.enabled = false;

        MouseAround ma = MCSUICenter.MainCamera.GetComponent<MouseAround>();

        MCSUICenter.MainCamera.transform.rotation = Quaternion.Euler(moat.y, moat.x, 0);
        //ma.rotationX = moat.x;
        //ma.rotationY = moat.y;
        ma.enabled = true;
    }

    public static void SwitchToWeaponryCamera(Weaponry weaponry)
    {
        MCSUICenter.MainCamera.GetComponent<MouseAround>().enabled = false;

        MouseOrbitAroundTarget moat = MCSUICenter.MainCamera.GetComponent<MouseOrbitAroundTarget>();
        moat.enabled = true;
        moat.target = weaponry.gameObject.transform;
    }

    public void LoadWeaponryList()
    {
        list.items.Clear();
        list.items.Add("Свободная камера");

        foreach (KeyValuePair<int, Weaponry> weaponry in MCSGlobalSimulation.Weapons.List)
        {
            if (weaponry.Value == null || weaponry.Value.gameObject.name.Contains("Cloud") || weaponry.Value.gameObject.name.Contains("LieTarget") || weaponry.Value.gameObject.name.Contains("War"))
            {
                MCSGlobalSimulation.Weapons.List.Remove(weaponry.Key);
                continue;
            }

            if(weaponry.Value is WeaponryProjectile) continue;

            list.items.Add("[" + weaponry.Value.ID + "] " + weaponry.Value.Name);
        }
    }
}
