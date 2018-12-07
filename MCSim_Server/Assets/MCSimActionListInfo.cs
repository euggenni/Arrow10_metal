using UnityEngine;
using System.Collections;
using System.IO;

public class MCSimActionListInfo : MonoBehaviour {

    public static string text = "";
    public UILabel actionListInfo;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        actionListInfo.text = text;
	}

    public void clearList()
    {
        text = "";
    }

    public void saveToFile()
    {
        StreamWriter fl = new StreamWriter("log.txt");
        fl.Write(text);
        fl.Close();
    }
}
