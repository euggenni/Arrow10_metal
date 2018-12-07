// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class BoxSelector : MonoBehaviour
{
    private Color NormalColor;
    private Color HilightColor;
    public bool OverBox = false;
    public GizmoController GC = null;

    void Awake()
    {
        GC = GameObject.Find("GizmoAdvanced").GetComponent<GizmoController>();
        GC.Hide();

        NormalColor = GetComponentInChildren<MeshRenderer>().material.color;
        HilightColor = NormalColor*2f;
    }

//Start


    private void OnMouseEnter()
    {
        foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
        {
            rend.material.color = HilightColor;
        }
        //renderer.material.color = HilightColor;
        OverBox = true;
    }

//OnMouseEnter

    private void OnMouseExit()
    {
        foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
        {
            rend.material.color = NormalColor;
        }
        //renderer.material.color = NormalColor;

        if(GC && !GC.IsOverAxis())
        OverBox = false;
    }

//OnMouseEnter

    private void OnMouseDown()
    {
        if (GC == null)
            return;

        if (GC.IsOverAxis())
            return;

        GC.Hide();

        GC.SetSelectedObject(transform);

        if (GC.IsHidden())
            GC.Show(GIZMO_MODE.TRANSLATE);
    }

    void OnDestroy()
    {
        if(GC)
        GC.Hide();
    }

    void Update()
    {
        if (GC == null)
            return;

            if (Input.GetMouseButtonUp(0) && !OverBox && !GC.IsOverAxis())
            {
                if (GC.SelectedObject == transform)
                {
                    GC.Hide();
                }
            } //if

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GC.Hide();
            }
    }//Update

//OnMouseDown
    /*
    void  Update (){
        if(GC == null)
            return;
		
        if(Input.GetMouseButtonUp(0) && !OverBox){
            GC.Hide();
        }//if
        else if(OverBox){
            if(!GC.IsOverAxis()){
                if(Input.GetMouseButtonUp(0))				
                    GC.SetSelectedObject(transform);
                    if(GC.IsHidden())
                        GC.Show(GIZMO_MODE.TRANSLATE);
            }//if
        }//else if
    }//Update
    */
}