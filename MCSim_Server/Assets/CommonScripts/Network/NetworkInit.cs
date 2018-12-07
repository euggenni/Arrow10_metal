using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public class NetworkInit : MonoBehaviour
{
    void Start()
    {
        // This is our own player
        NetworkInterpolatedTransform[] trans_interp_script = GetComponentsInChildren<NetworkInterpolatedTransform>();
        NetworkInterpolatedRotation[] rotat_interp_script = GetComponentsInChildren<NetworkInterpolatedRotation>();

        if (Network.isServer)
        {
            foreach (NetworkInterpolatedTransform networkInterpolatedTransform in trans_interp_script)
            {
                networkInterpolatedTransform.enabled = false;
            }

            foreach (NetworkInterpolatedRotation networkInterpolatedRotation in rotat_interp_script)
            {
                networkInterpolatedRotation.enabled = false;
            }
        }
        else // This is just some remote controlled player
        {
            foreach (NetworkInterpolatedTransform networkInterpolatedTransform in trans_interp_script)
            {
                networkInterpolatedTransform.enabled = true;
            }

            foreach (NetworkInterpolatedRotation networkInterpolatedRotation in rotat_interp_script)
            {
                networkInterpolatedRotation.enabled = true;
            }

            Weaponry weaponry = GetComponent<Weaponry>();
            if(weaponry) {
                name = weaponry.Name + "_" + weaponry.ID;
            }
        }
    }
}