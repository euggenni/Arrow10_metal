using UnityEngine;
using System.Collections;

public class PopupMenuScript : MonoBehaviour
{
    public UIPopupList listUI;
    public UILabel label;
    public UISprite labelBackground;

    public bool HideHeader = false;
	// Use this for initialization
	void Start () {
        ShowHeader(HideHeader);
	}

    public void ShowHeader(bool val)
    {
        if(!val)
        {
            label.enabled = false;
            labelBackground.enabled = false;
        }
        else
        {
            label.enabled = true;
            labelBackground.enabled = true;
        }
    }
}
