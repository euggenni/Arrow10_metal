using UnityEngine;

public class Strela10_Operator_JoystickController : MonoBehaviour {
    public JoystickToolkit JoystickHorizontal;
    public JoystickToolkit JoystickVertical;

    private float _horDimension, _vertDimension;
    private float _startHor, _startVert;

    public float TurnSpeedX = 15f, TurnSpeedY = 10f;

    // Use this for initialization
    void Start() {
        _startHor = JoystickHorizontal._state.Y.Value;
        _startVert = JoystickVertical._state.X.Value;

        _horDimension = 1f;
        //Mathf.Abs(JoystickHorizontal._state.Y.Max - JoystickHorizontal._state.Y.Min) / 50f;
        _vertDimension = 2f;
        //Mathf.Abs(JoystickVertical._state.X.Max - JoystickVertical._state.X.Min) / 50f;
        ///Debug.Log("ArtLogging(Strela10_Operator_JoystickController) _startHor = " + _startHor);
        ///Debug.Log("ArtLogging(Strela10_Operator_JoystickController) _startVert = " + _startVert);
    }

    void Update() {
    }

    void FixedUpdate() {
        ///Debug.Log("ArtLogging(Strela10_Operator_JoystickController) JoystickHorizontal._state.Y.Value = " + JoystickHorizontal._state.Y.Value);
        ///Debug.Log("ArtLogging(Strela10_Operator_JoystickController) JoystickVertical._state.X.Value = " + JoystickVertical._state.X.Value);
    }

    //private float DeleteHorizontalDeathZones(float value)
    //{
    //    float result;
    //    if (value > 1800 && value )
    //}

    private float _horEnter = 0f, _vertEnter = 0f;
    //Update is called once per frame
    //void Update()
    //{
    //    _horEnter = 0f;
    //    _vertEnter = 0f;

    //    if (Input.GetKey(KeyCode.LeftArrow))
    //    {
    //        if (Input.GetKey(KeyCode.LeftShift))
    //            JoystickHorizontal._state.Y.Value = -4;
    //        else
    //        {
    //            _horEnter = _horDimension;
    //            JoystickHorizontal._state.Y.Value -= (int)_horEnter;
    //        }
    //    }

    //    if (Input.GetKey(KeyCode.RightArrow))
    //    {
    //        if (Input.GetKey(KeyCode.LeftShift))
    //            JoystickHorizontal._state.Y.Value = 4;
    //        else
    //        {
    //            _horEnter = _horDimension;
    //            JoystickHorizontal._state.Y.Value += (int)_horEnter;
    //        }
    //    }

    //    if (Input.GetKey(KeyCode.UpArrow))
    //    {
    //        if (Input.GetKey(KeyCode.LeftShift))
    //            JoystickVertical._state.X.Value = 4;
    //        else
    //        {
    //            _vertEnter = _vertDimension;
    //            JoystickVertical._state.X.Value += (int)_vertEnter;
    //        }
    //        Debug.Log("UpArrow: " + JoystickVertical._state.X.Value);
    //    }

    //    if (Input.GetKey(KeyCode.DownArrow))
    //    {
    //        if (Input.GetKey(KeyCode.LeftShift))
    //            JoystickVertical._state.X.Value = -4;
    //        else
    //        {
    //            _vertEnter = _vertDimension;
    //            JoystickVertical._state.X.Value -= (int)_vertEnter;
    //        }
    //        Debug.Log("DownArrow: " + JoystickVertical._state.X.Value);
    //    }

    //    if (_horEnter == 0)
    //    {
    //        JoystickHorizontal._state.Y.Value = (int)Mathf.Lerp(JoystickHorizontal._state.Y.Value, _startHor,
    //                                                       Time.fixedDeltaTime * TurnSpeedX);
    //    }

    //    if (_vertEnter == 0)
    //    {
    //        JoystickVertical._state.X.Value = (int)Mathf.Lerp(JoystickVertical._state.X.Value, _startVert,
    //                                                       Time.fixedDeltaTime * TurnSpeedY);
    //    }
    //}
}