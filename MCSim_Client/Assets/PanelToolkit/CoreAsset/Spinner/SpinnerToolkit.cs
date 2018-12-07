using System;
using System.Collections;
using UnityEngine;
using Object = System.Object;

/* Класс для крутящихся объектов пультов управления, 
 * например крутящийся тумблер или шкала */

#pragma warning disable 0618, 0108

public enum OPTIONS {
    X = 0,
    Y = 1,
    Z = 2
}

[RequireComponent(typeof(SphereCollider))]
public class SpinnerToolkit : PanelControl {
    // Инвертировать вращение
    public bool Inverse;

    // Ось по которой происходит вращение
    public OPTIONS Axe = OPTIONS.X;

    [SerializeField]
    private ControlPanelToolkit parentPanelScript = null;

    public void SetParentPanelScript(ControlPanelToolkit panelScript) {
        parentPanelScript = panelScript;
    }

    public ControlPanelToolkit getParentPanelScript() {
        if (!parentPanelScript) {
            parentPanelScript = gameObject.GetComponentInParents<ControlPanelToolkit>(false);
        }

        return parentPanelScript;
    }

    public void removeParentPanelScript() {
        parentPanelScript = null;
    }

    [SerializeField]
    public int SpinnerID = -1;

    [SerializeField]
    private bool _isManual; // Можем ли вручную изменять значение

    public bool isManual {
        get { return _isManual; }
        set {
            if (value) {
                if (this.gameObject.GetComponent<SphereCollider>() == null)
                    this.gameObject.AddComponent<SphereCollider>();
            } else if (this.gameObject.GetComponent<SphereCollider>() != null) {
                try {
                    Debug.Log("Spinner " + name + " is not manual and should not have SphereCollider attached to it!");

                    if (Application.isEditor)
                        DestroyImmediate(this.gameObject.GetComponent<SphereCollider>());
                } catch {
                    Debug.Log("Ошибка при удалении компонента");
                }
            }
            _isManual = value;
        }
    }

    [SerializeField]
    public bool isCyclical; // Цикличность

    [SerializeField]
    public bool showSlider;

    [SerializeField]
    public bool showLine = true;

    private bool change;

    [SerializeField]
    private Transform startRotation; // Трансформ положения, соответстующего минимальному  значению

    public Transform StartRotation {
        get { return this.startRotation; }
        set {
            // Чтобы не пересчитывала dimension при обновлении интерфейса
            if (this.startRotation == value) return;

            this.startRotation = value;
            CalculateDimension();
        }
    }

    [SerializeField]
    private Transform endRotation; // Трансформ положения, соответстующего максимальному значению

    public Transform EndRotation {
        get { return this.endRotation; }
        set {
            // Чтобы не пересчитывала dimension при обновлении интерфейса
            if (this.endRotation == value) return;

            this.endRotation = value;
            CalculateDimension();
        }
    }

    [SerializeField]
    private int minimalValue = 0; // Минимальное значение

    public int MinimalValue {
        get { return this.minimalValue; }
        set {
            if (value >= this.maximalValue) return;
            this.minimalValue = value;

            if (this._value < this.minimalValue) this._value = this.minimalValue;
        }
    }

    [SerializeField]
    private int maximalValue = 100; // Максимальное значение

    public int MaximalValue {
        get { return this.maximalValue; }
        set {
            if (value <= 0) return;
            if (value <= this.minimalValue) return;
            this.maximalValue = value;

            if (this._value > this.maximalValue) this._value = this.maximalValue;
        }
    }

    [SerializeField]
    private int _value = 0; // Текущее значение

    public int Value {
        get { return this._value; }
        set {
            try {
                // Если это виртуальный элемент
                if (getParentPanelScript().GetCore() != null)
                    if (getParentPanelScript().GetCore().isVirtual) {
                        _value = value;
                        return;
                    }
            } catch {
                if (!Application.isEditor)
                    Debug.LogWarning("Error during setting value to control.");
            }

            if (this.startRotation == null || this.endRotation == null) {
                return;
            }

            if (this._value.Equals(value)) return;

            if (!isCyclical) // Если не циклично, то не можем выйти за рамки
            {
                this._value = value;

                if (value < this.minimalValue) {
                    this._value = this.minimalValue;
                }

                if (value > maximalValue) {
                    this._value = this.maximalValue;
                }
            } else // Если циклично, то при переходе за границу "дозволенного" пересчитываем и приводим в эти границы
            {
                this._value = VerifyValue(value);
            }

            transform.rotation = startRotation.rotation;

            float shift = _value * dimension;
            if (Inverse) shift = -shift;

            if (Axe == OPTIONS.X)
                transform.Rotate(shift, 0, 0);

            if (Axe == OPTIONS.Y)
                transform.Rotate(0, shift, 0);

            if (Axe == OPTIONS.Z)
                transform.Rotate(0, 0, shift);

            if (ChangedSpinnerState != null)
                ChangedSpinnerState.Invoke(new StateEventArgs(Port, _value == 0 ? false : true));
        }
    }

    public int Port { get; set; }

    public event Action<StateEventArgs> ChangedSpinnerState;

    // Скольков градусов в единице значения
    [SerializeField]
    private float dimension = 0;

    private Transform temp_transform;

    // Рассчитывает угол между стартовым и конечным положением 
    public void CalculateDimension() {
        if (this.startRotation == null || this.endRotation == null) return;

        float angleBetween;
        angleBetween = Mathf.Abs(this.startRotation.transform.localEulerAngles[(int) Axe] -
                                 this.endRotation.transform.localEulerAngles[(int) Axe]);

        if (this.startRotation.transform.localEulerAngles[(int) Axe] >=
            this.endRotation.transform.localEulerAngles[(int) Axe]) // Если начальный угол < конечного
            angleBetween = 360 - angleBetween;

        dimension = angleBetween / (float) this.maximalValue;

        sliderDimension = (maximalValue - minimalValue) / sliderSizeX;

        transform.rotation = startRotation.rotation;
    }

    // Приводит указанное значение к заданным границам, если оно выходит за эти границы (380 превращает в 20)
    private int VerifyValue(int num) {
        if (num > this.maximalValue) // Если больше чем максимум
        {
            num = this.minimalValue + (num - this.maximalValue);
            return VerifyValue(num);
        }

        if (num < this.minimalValue) // Если меньше чем минимум
        {
            num = this.maximalValue + (num - this.minimalValue);
            return VerifyValue(num);
        }

        return num;
    }

    public void OnMouseDown() {
        if (parentPanelScript == null) return;

        // Отключаем ручное управление
        if (!isManual) return;

        change = true;

        Screen.showCursor = false;
        mouseStartX = Input.mousePosition.x;
    }

    public void OnMouseDrag() {
        if (!isManual) return;

        if (change) {
            //Debug.Log("Dimension: " + dimension);
            //Value += (int)((Input.mousePosition.x - mouseStartX) / (0.5f * dimension));
            Value += (int) ((Input.mousePosition.x - mouseStartX) * sliderDimension);
            mouseStartX = Input.mousePosition.x;
        }
    }

    public void OnMouseUp() {
        change = false;
        Screen.showCursor = true;

        ControlChanged();
    }

    public void OnGUI() {
        if (change && showSlider) {
            GUI.HorizontalSlider(new Rect(
                    Camera.main.WorldToScreenPoint(this.transform.position).x - sliderSizeX / 2,
                    Screen.height - Camera.main.WorldToScreenPoint(this.transform.position).y,
                    sliderSizeX,
                    sliderSizeY),
                (float) Value, minimalValue, maximalValue);
        }
    }

    bool rotatingNew;

    void ChangeValueTo(int newvalue) {
        if (this.Value == newvalue) return;

        StartCoroutine(changeValueTo(newvalue));
    }

    IEnumerator changeValueTo(int Value) {
        float tempValue;
        tempValue = (float) this.Value;
        if (this.Value != Value) {
            tempValue = (int) Mathf.Lerp(tempValue, (float) Value,
                Time.fixedDeltaTime * 1f);

            this.Value = (int) tempValue;
            yield return new WaitForFixedUpdate();
            //yield return new WaitForSeconds(0.1f);
        }
        //if (this.Value != Value)
        //{

        //    if (!rotatingNew)
        //    {
        //        float rotationSpeed = 250f;
        //        float imprecision = rotationSpeed / 100f;

        //        float endAngle = (this.startRotation.eulerAngles[(int)Axe] + (float)Value * dimension);
        //        if (endAngle > 360f || endAngle < -360f) endAngle = endAngle % 360f;

        //        float currentAngle = this.transform.eulerAngles[(int)Axe];
        //        if (currentAngle > 360f || currentAngle < -360f) currentAngle = currentAngle % 360f;

        //        while (currentAngle >= endAngle + imprecision || currentAngle <= endAngle - imprecision)
        //        {
        //            currentAngle = this.transform.eulerAngles[(int)Axe];
        //            if (currentAngle > 360f || currentAngle < -360f) currentAngle = currentAngle % 360f;

        //            // Определяем направление для кратчайшего достижения этого угла
        //            if (endAngle + (360 - currentAngle) < currentAngle - endAngle)
        //            {
        //                transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        //                yield return new WaitForSeconds(0.01f);
        //                continue;
        //            }

        //            if (currentAngle + (360 - endAngle) < endAngle - currentAngle)
        //            {
        //                transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        //                yield return new WaitForSeconds(0.01f);
        //                continue;
        //            }


        //            if (endAngle - currentAngle > currentAngle - endAngle) { transform.Rotate(0, 0, rotationSpeed * Time.deltaTime); }
        //            else { transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime); }

        //            yield return new WaitForSeconds(0.01f);
        //        }

        //        this._value = Value;

        //        rotatingNew = false;
        //    }
        //}
    }

    void Start() {
        CalculateDimension();
        //Value = minimalValue + 1;
    }

    // ----------------------------------------------------
    private float sliderSizeX = 125, sliderSizeY = 25;

    private float sliderDimension;
    private float mouseStartX;

    public GameObject GameObject {
        get { return gameObject; }
    }

    public override CoreLibrary.Core Core {
        get { return parentPanelScript.Core; }
    }

    public override Object State {
        get {
            if (parentPanelScript == null) return "NONE";
            return Value;
        }

        set {
            if (value is int) {
                try {
                    // Если это виртуальный элемент
                    if (getParentPanelScript().GetCore().isVirtual) {
                        _value = (int) value;
                        return;
                    }
                } catch {
                    Debug.LogWarning("Error during setting value to virtual control.");
                }

                Value = (int) value;
                //Model.UCoroutine(this, ChangeValueTo((int) value), "RotateSpinner");
                //StartCoroutine(ChangeValueTo((int)value));
            } else {
                Debug.Log("Type of object is not recognized. Int required");
            }
        }
    }

    public override string GetName() {
        if (parentPanelScript != null)
            return parentPanelScript.GetSpinners()[SpinnerID].GetName();
        else return "NONE";
    }

    public override string GetPanelName() {
        if (parentPanelScript != null)
            return parentPanelScript.GetName();
        else return "NONE";
    }

    public override ControlType ControlType {
        get { return ControlType.Spinner; }
    }

    public override void ControlChanged() {
        if (parentPanelScript != null) {
            parentPanelScript.ControlChanged(this);
        } else {
            Debug.Log("Cannot report about change to Parental Panel, because its not defined yet");
        }
    }
}