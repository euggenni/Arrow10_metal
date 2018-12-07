using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

/* Класс для индикаторов - ламопчек, табло и т.п. */

#pragma warning disable 0414, 0108
#pragma warning disable 0618
public class IndicatorToolkit : PanelControl {
    [SerializeField]
    private string name = "";

    void Start() {
        IndicatorStateID = indicatorStateID;
    }

    [SerializeField]
    private Color[] colors;

    public Color[] Colors {
        get { return colors; }
        set { colors = value; }
    }


    /// <summary>
    /// Лист возможных состояний индикатора
    /// </summary>
    [SerializeField]
    public List<string> StatesList = new List<string>();

    [SerializeField]
    private ControlPanelToolkit parentPanelScript = null
        ; // Указатель на скрипт панели, к которой принадлежит этот тумблер

    [SerializeField]
    public bool isManual; // Возможно ли ручное переключение

    [SerializeField]
    public int IndicatorID = -1; // ID индикатора в списке индикаторов на панели

    [SerializeField]
    private int indicatorStateID = 0; // ID текущего состояния индикатора в списке состояний этого индикатора

    public int IndicatorStateID {
        get { return indicatorStateID; }
        set {
            //if (indicatorStateID == value) return;


            indicatorStateID = value;
            if (parentPanelScript == null) {
                return;
            }
            changeStateTo(parentPanelScript.GetIndicators()[IndicatorID].GetStatesListAsList()[value]);

            if (ChangedIndicatorState != null)
                ChangedIndicatorState.Invoke(new StateEventArgs(Port, StatesList[value] == "ON" ? true : false));
        }
    }

    public int Port { get; set; }

    public event Action<StateEventArgs> ChangedIndicatorState;

    public GameObject GameObject {
        get { return gameObject; }
    }

    public override CoreLibrary.Core Core {
        get { return parentPanelScript.Core; }
    }

    // Работа с родительской панелью
    public override Object State {
        get {
            if (parentPanelScript == null) return "NONE";
            return parentPanelScript.GetIndicators()[IndicatorID].GetStatesListAsList()[indicatorStateID];
        }
        set {
            //if (value.GetControlType().ToString().Equals("System.String"))
            if (value.GetType().Equals(typeof(String))) {
                //Debug.Log("Индикатор [" + GetName() + "] принял значение [" + value + "]");

                if (parentPanelScript.GetIndicators()[IndicatorID].GetStatesListAsList().Contains((string) value)) {
                    IndicatorStateID = parentPanelScript.GetIndicators()[IndicatorID].GetStatesListAsList()
                        .IndexOf((string) value);
                    changeStateTo((string) value);
                    return;
                }
            }

            if (value.GetType().Equals(typeof(int))) {
                IndicatorStateID = (int) value;
                changeStateTo(parentPanelScript.GetIndicators()[IndicatorID].GetStatesListAsList()[indicatorStateID]);
                return;
            }

            Debug.Log("Type of object is not recognized. Indicator [" + GetName() + "], type: " + value.GetType());
        }
    }

    public override string GetName() {
        return name;
        //return Indicator.GetName();
    }

    public override string GetPanelName() {
        if (parentPanelScript != null)
            return parentPanelScript.GetName();
        else return "NONE";
    }

    public override ControlType ControlType {
        get { return ControlType.Tumbler; }
    }

    public void setParentPanelScript(ControlPanelToolkit panelScript) {
        parentPanelScript = panelScript;
    }

    public ControlPanelToolkit getParentPanelScript() {
        return parentPanelScript;
    }

    public void removeParentPanelScript() {
        parentPanelScript = null;
    }

    private Tumbler _indicatorInfo = null;

    [SerializeField]
    public Tumbler Indicator {
        get {
            if (parentPanelScript == null) {
                Debug.Log("Parent is not set for this Indicator");
                return null;
            }

            //Debug.Log("ID:" + IndicatorID);

            return parentPanelScript.GetIndicators()[IndicatorID];
        }
        set {
            name = value.GetName();

            StatesList = value.GetStatesListAsList();
            int length = StatesList.Count;

            Colors = new Color[length];

            isGlowing = new bool[length];

            for (int i = 0; i < length; i++) {
                Colors[i] = renderer.sharedMaterial.color;
                isGlowing[i] = false;
            }

            List<Tumbler> list = parentPanelScript.GetIndicators();

            foreach (Tumbler tmblr in list) {
                if (tmblr.GetName().Equals(value.GetName())) {
                    IndicatorID = list.IndexOf(tmblr);
                    break;
                }
            }
        }
    } // Загрузка объекта индикатора по его ID  

    public void SetColor(Color clr, int index) {
        colors[index] = clr;
    }

    public Color[] GetColors() {
        return colors;
    }


    [SerializeField]
    public bool[] isGlowing;

    public void setGlowing(bool value, int index) {
        if (isGlowing[index] == value) return;

        isGlowing[index] = value;
    }

    public bool getGlowing(int index) {
        return isGlowing[index];
    }

    #region Обработка нажатия

    void OnMouseDown() {
        if (parentPanelScript == null) return;

        if (isManual)
            IndicatorStateID = NextID();
    }

    public int NextID() {
        int nextID = indicatorStateID + 1;

        if (nextID >= parentPanelScript.GetIndicators()[IndicatorID].GetStatesListAsList().Count) return 0;
        else return nextID;
    }

    public void changeStateTo(string state) {
        Tumbler tmblr = Indicator;

        for (int i = 0; i < tmblr.GetStatesListAsList().Count; i++) {
            if (tmblr.GetStatesListAsList()[i].Equals(state)) {
                //Debug.Log("Current shader for №" + i + " is " + Shaders[i].name);
                if (Application.isPlaying) {
                    Shader shdr = Shader.Find("Diffuse");
                    if (isGlowing[i]) shdr = Shader.Find("Self-Illumin/Diffuse");

                    transform.renderer.material.shader = shdr;
                    transform.renderer.material.color = Colors[i];

                    //transform.renderer.material.shader = Shaders[i];
                    //transform.renderer.material.color = Colors[i];
                    return;
                } else {
                    //Debug.Log("[" + GetName() + "] changed state to " + indicatorStateID);
                }
            }
        }
        //Debug.Log("State [" + state + "] of indicator is not exist");
    }

    public override void ControlChanged() {
        if (parentPanelScript != null) {
            parentPanelScript.ControlChanged(this);
        } else {
            Debug.Log("Cannot report about change to Parental Panel, because its not defined yet");
        }
    }

    #endregion
}