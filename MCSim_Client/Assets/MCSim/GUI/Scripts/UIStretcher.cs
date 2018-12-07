using UnityEngine;

public class UIStretcher : MonoBehaviour {
    public enum Side {
        Vertical,
        Horizontal
    }

    public Side Align = Side.Horizontal;

    // Use this for initialization
    void Start() {
        float multiplier = 1f;

        switch (Align) {
            case Side.Horizontal:
                multiplier = Screen.width / transform.localScale.x;
                break;

            case Side.Vertical:
                multiplier = Screen.height / transform.localScale.y;
                break;
        }

        transform.localScale *= multiplier;
    }

    void Update() {
        //float multiplier = 1f;

        //switch (Align)
        //{
        //    case Side.Horizontal:
        //        multiplier = Screen.width / transform.localScale.x;
        //        break;

        //    case Side.Vertical:
        //        multiplier = Screen.height / transform.localScale.y;
        //        break;
        //}

        //transform.localScale *= multiplier;
    }
}