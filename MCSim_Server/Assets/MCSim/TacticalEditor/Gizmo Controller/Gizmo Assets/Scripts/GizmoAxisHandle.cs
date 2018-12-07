// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public enum AXIS_COLOR { NORMAL, HOVER, DRAG }
public class GizmoAxisHandle : MonoBehaviour
{
    public Color NormalColor;
    public Color HoverColor;
    public Color DragColor;

    public bool ChangeNormalOnStart = true;
    public bool ChangeHoverOnStart = true;
    public bool ChangeDragOnStart = true;

    private AXIS_COLOR _axisColor = AXIS_COLOR.NORMAL;


    void Start()
    {
        if (!transform.renderer)
            return;

        if(ChangeNormalOnStart)
        NormalColor = transform.renderer.material.color;

        if(ChangeHoverOnStart)
        HoverColor = new Color(NormalColor.r, NormalColor.g, NormalColor.b, 1);

        if(ChangeDragOnStart)
        DragColor = new Color(1, 1, 0, .8f);
    }//Start

    void SetAxisColor(AXIS_COLOR axisColor)
    {
        _axisColor = axisColor;

        if (!transform.renderer)
            return;


        switch (_axisColor)
        {
            case AXIS_COLOR.NORMAL:
                transform.renderer.material.color = NormalColor;
                break;

            case AXIS_COLOR.HOVER:
                transform.renderer.material.color = HoverColor;
                break;

            case AXIS_COLOR.DRAG:
                transform.renderer.material.color = DragColor;
                break;
        }//switch
    }//SetAxisColor
}