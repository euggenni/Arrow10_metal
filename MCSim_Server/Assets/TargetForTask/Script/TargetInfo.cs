using UnityEngine;
using System.Collections;
using System;

public class TargetInfo : MonoBehaviour {

	// Use this for initialization
    private static string text = "��������� ����:\n";

    public static string Text
    {
        set { text = value; }
        get { return text; }
    }
    

}
