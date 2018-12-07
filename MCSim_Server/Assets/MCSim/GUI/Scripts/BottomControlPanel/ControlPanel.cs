using UnityEngine;
using System.Collections;

public class ControlPanel : MCSUIObject
{
    public GameObject Sprite;
    public Transform LeftTop, BottomRight;

	void Start ()
	{
        //float k = ScaleWidth / ScreenWidth;
        Vector3 vc = MCSUICenter.GUICamera.ScreenToWorldPoint(new Vector2(0, 0));
        transform.position = new Vector3(transform.position.x, vc.y, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
       
       // Debug.Log("ScreenWidth: " + Sprite.renderer.bounds.size);
	}

    void FixedUpdate()
    {
        
    }

    void OnGUI()
    {
        //if (!calibred)
        //{
        //    Vector3 vc = MCSUICenter.GUICamera.ScreenToWorldPoint(new Vector2(0, 0));
        //    transform.position = new Vector3(vc.x, vc.y, transform.position.z);
        //    calibred = true;
        //}
    }

    //private bool calibred = false;

    public override void Hide()
    {
        throw new System.NotImplementedException();
    }

    public override void Show()
    {
        throw new System.NotImplementedException();
    }

    public override void Close()
    {
        throw new System.NotImplementedException();
    }

    public void OnWeaponryInstantiated(MilitaryCombatSimulator.Weaponry weaponry)
    {
        throw new System.NotImplementedException();
    }
}
