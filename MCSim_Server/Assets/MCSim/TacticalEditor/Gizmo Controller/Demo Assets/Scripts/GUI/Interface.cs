// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class Interface : MonoBehaviour
{
    public GizmoController GC;
    public Transform SelectedObject;
    public Texture2D LogoGraphic;

    void Start()
    {
        //GC.Hide();
    }//Start

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, Screen.height - 25, Screen.width - 20, 25));
        GUILayout.BeginHorizontal();
        GUILayout.Label("LMB: Camera Rotate");
        GUILayout.Label("RMB: Camera Pan");
        GUILayout.Label("1: Translate Mode");
        GUILayout.Label("2: Rotate Mode");
        GUILayout.Label("3: Scale Mode");
        GUILayout.Label("S: Enable/Disable Snapping");
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        GUI.DrawTexture(new Rect(Screen.width - 130, 2, 128, 128), LogoGraphic);
    }//OnGUI
}