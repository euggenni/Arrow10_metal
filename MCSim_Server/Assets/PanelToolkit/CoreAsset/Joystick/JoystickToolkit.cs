using System.IO;
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Изменение состояния контрола
/// </summary>
public delegate void OnChangeCall();

[Serializable]
public class DimensionStates
{
    [SerializeField] public bool _isVirtual;

    [SerializeField] private int _dimensionIndex;

    [SerializeField]
    public event OnChangeCall ChangeCallEvent;

    public DimensionStates(OnChangeCall callback, int di, bool isVirtual)
    {
        ChangeCallEvent += callback;
        _dimensionIndex = di;
        _isVirtual = isVirtual;

        //Debug.Log("Joystick control is virtual: " + isVirtual);
    }

    [SerializeField] private int _min = -1, _max = 1, _value = 0;

    [SerializeField] private float dimension = float.NaN;

    [SerializeField]
    private Transform _positionMin, _positionMax;
  
    public float Shift 
    {
        
        get { return dimension*_value; }
    }

    public int Min
    {
        get { return _min; }
        set
        {
            _min = value;
            if (value > 0) _min = 0;
            if (_value < value) _value = value;
        }
    }

    public int Max
    {
        get { return _max; }
        set
        {
            _max = value;
            if (value < 0) _max = 0;
            if (_value > value) _value = value;
        }
    }

    public int Value
    {
        get { return _value; }
        set
        {
            try
                {
                    // Если это виртуальный элемент
                    if (_isVirtual)
                    {
                        _value = (int)value;
                        return;
                    }
                }
                catch
                {
                    Debug.LogWarning("Error during setting value to virtual control.");
                }

            if(!Availiable) return;

            if (value == _value) return;
            _value = value;
            if (value > _max) _value = _max;
            if (value < _min) _value = _min;

           ChangeCallEvent();
        }
    }

    public bool Availiable
    {
        get
        {
            if(dimension > 0) return true;
            return false;
        }
    }


    public Transform PositionMin
    {
        get { return _positionMin; }
        set
        {
            if (_positionMin == value) return;
            _positionMin = value; CalculateDimension();
        }
    }

    public Transform PositionMax
    {
        get { return _positionMax; }
        set
        {
            if (_positionMax == value) return; 
            _positionMax = value; 
            CalculateDimension();
        }
    }

    // Рассчитывает угол между стартовым и конечным положением 
    public void CalculateDimension()
    {
        if (_positionMin == null || _positionMax == null) return;

        //Debug.Log("Index: " + _dimensionIndex);
        float angleBetween;
        angleBetween = Mathf.Abs(_positionMin.transform.localEulerAngles[_dimensionIndex] - _positionMax.transform.localEulerAngles[_dimensionIndex]);

        //if (_positionMin.transform.localEulerAngles[_dimensionIndex] >= _positionMax.transform.localEulerAngles[_dimensionIndex]) // Если начальный угол > конечного
        //    angleBetween = 360 - angleBetween;
        //Debug.Log("leaMin: " + _positionMin.transform.localEulerAngles);
        dimension = angleBetween / Math.Abs(_min - _max);
        //Debug.Log("Min:" + _positionMin.transform.localEulerAngles[_dimensionIndex] + " Max:" + _positionMax.transform.localEulerAngles[_dimensionIndex] + " AngleBetween:" + angleBetween + "  Dimension: " + dimension);
        //sliderDimension = (maximalValue - minimalValue) / sliderSizeX;
    }
}

[Serializable]
public class StateXYZ
{
    [SerializeField]
    public DimensionStates X, Y, Z;

    public StateXYZ(OnChangeCall callback, bool isVirtual)
    {
        X = new DimensionStates(callback, 0, isVirtual);
        Y = new DimensionStates(callback, 1, isVirtual);
        Z = new DimensionStates(callback, 2, isVirtual);   
    }
}

public class JoystickToolkit : PanelControl
{
    [SerializeField] private string _name = "Joystick-NONAME";

    [SerializeField] public Transform StartPosition;

    [SerializeField] public ControlPanelToolkit _parentPanelScript = null;

    [SerializeField] public int ControlID = -1;

    [SerializeField] public StateXYZ _state;

    public JoystickToolkit()
    {
        _state = new StateXYZ(OnChangeCall, false);
    }

    // Возвращается ли в исходное положение
    //[SerializeField] 
    //public bool reversable = false;

    [SerializeField]
    public bool X, Y, Z;

    void OnChangeCall()
    { 
        if(StartPosition)
        {
            transform.localRotation = StartPosition.localRotation;

            if (X && _state.X.Availiable)
            transform.Rotate(_state.X.Shift, 0, 0);

            if (Y && _state.Y.Availiable)
            transform.Rotate(0, _state.Y.Shift, 0);

            if (Z && _state.Z.Availiable)
            transform.Rotate(0, 0, -_state.Z.Shift);
        }
    }

    #region Панель
    public void                 SetParentPanelScript(ControlPanelToolkit panelScript)
    {
        _parentPanelScript = panelScript;

        bool isVirtual = false;
        if (panelScript.GetCore() != null)
            isVirtual = panelScript.GetCore().isVirtual;


        if (_state == null) // Если нет значения. Впервые инициализируем объект
        {
            _state = new StateXYZ(OnChangeCall, isVirtual);
        }
        else // Создали и настроили через инспектор, но при сохранении скрипта все обнуляется
        {
            _state.X._isVirtual = isVirtual;
            _state.Y._isVirtual = isVirtual;
            _state.Z._isVirtual = isVirtual;
        }
    }
    public ControlPanelToolkit  GetParentPanelScript()
    {
        return _parentPanelScript;
    }
    public void                 RemoveParentPanelScript()
    {
        _parentPanelScript = null;
    }
    #endregion


    public override CoreLibrary.Core Core
    {
        get { return _parentPanelScript.Core; }
    }

    public GameObject GameObject { get { return gameObject; } }

    public override object State
    { 
        get
        {
            int[] args = new int[3];
            args[0] = _state.X.Value;
            args[1] = _state.Y.Value;
            args[2] = _state.Z.Value;

            return args;
        } 
        set
        {
            //.GetControlType().Equals(typeof(int[]))
            if(value is int[])
            {
                int[] args = (int[]) value;

                if(args.Length == 3)
                {
                    _state.X.Value = args[0];
                    _state.Y.Value = args[1];
                    _state.Z.Value = args[2];
                }
                else
                {
                    Debug.Log("Incoming array have invalid count of arguments. 3 args is required.");
                }
            }
            else if (value is Vector3)
            {
					_state.X.Value = (int)((Vector3)value).x;
					_state.Y.Value = (int)((Vector3)value).y;
					_state.Z.Value = (int)((Vector3)value).z;
            }
				else
            {
                Debug.Log("Incorrect type of object. Type 'int[]' is required.");
            }
        } 
    }

    public void SetName(string name)
    {
        _name = name;
    }

    public override string GetName()
    {
        return _name;
    }

    public override string GetPanelName()
    {
        if(_parentPanelScript != null){
            return _parentPanelScript.GetName();
        } 
            
        Debug.LogError("ParentPanel is not installed fot this Control [" + GetName() + "]");;
        return "None";
    }

    public override ControlType ControlType {
        get { return ControlType.Joystick; }
    }

    public override void ControlChanged()
    {
    }
}
