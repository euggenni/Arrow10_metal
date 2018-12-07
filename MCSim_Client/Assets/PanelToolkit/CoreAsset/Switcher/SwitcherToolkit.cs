using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = System.Object;

#pragma warning disable 0414, 0108
#pragma warning disable 0618, 0168

[RequireComponent(typeof(SphereCollider))]
public class SwitcherToolkit : PanelControl {
    [SerializeField]
    private ControlPanelToolkit _parentPanelScript;

    [SerializeField] // Является ли кнопкой - достаточно лишь нажатия
    public bool isButton;

    [SerializeField] // Возвращается ли в исходное полоение
    public bool isReversible;

    [SerializeField]
    public int TumblerID = -1;

    [SerializeField]
    public bool showTooltip = true;

    /// <summary>
    /// Лист возможных состояний тумблера
    /// </summary>
    [SerializeField]
    public List<string> StatesList = new List<string>();

    public List<int> PortsList = new List<int>();

    public void OnOnChangedValue(ValueEventArgs arg) {
        var state = 0;
        if (arg.Value > 100 && arg.Value < 260) {
            state = 1;
        }

        if (TumblerStateID != state) {
            TumblerStateID = state;
            ControlChanged();
        }
    }

    public void OnChangedState(StateEventArgs arg) {
        var port = PortsList.FirstOrDefault(c => c == arg.Port);

        Debug.Log("Port: " + port);

        if (port != 0) {
            //if(PortsList.All(c => c > 0) && arg.State == true)
            //{
            TumblerStateID = PortsList.IndexOf(port);
            ControlChanged();
            //}
        } else if (arg.Port < 0) {
            port = PortsList.FirstOrDefault(c => c == Math.Abs(arg.Port));
            if (port != 0) {
                try {
                    TumblerStateID = PortsList.IndexOf(PortsList.First(c => c < 0));
                } catch {
                    return;
                }
                ControlChanged();
            }
        }
    }

    void Awake() {
        if (renderer) {
            renderer.receiveShadows = false;
            renderer.castShadows = false;
        }
    }

    void Start() {
        try {
            changeStateTo(_parentPanelScript.GetTumblers()[TumblerID].GetStatesListAsList()[tumblerStateID]);
        } catch (Exception e) {
        }
    }

    [SerializeField]
    private int tumblerStateID = 0;

    public int TumblerStateID {
        get { return tumblerStateID; }
        set {
            if (tumblerStateID == value) return;

            tumblerStateID = value;
            if (_parentPanelScript == null) {
                //Debug.Log("This tumbler have no parent panel. State cannot be changed.");
                return;
            }
            //Debug.Log("Sending");

            try {
                changeStateTo(_parentPanelScript.GetTumblers()[TumblerID].GetStatesListAsList()[value]);
            } catch {
                Debug.Log("Having trouble, when trying to switch [" + GetName() + "] state to value " + value);
            }
        }
    }

    public override CoreLibrary.Core Core {
        get { return _parentPanelScript.Core; }
    }

    public override Object State {
        get {
            //Debug.Log("TPanel3: " + _parentPanelScript);
            //if (_parentPanelScript == null) return "NONE";
            //return _parentPanelScript.GetTumblers()[TumblerID].GetStatesListAsList()[tumblerStateID];

            if (_parentPanelScript == null) return -1;
            //return tumblerStateID;
            return _parentPanelScript.GetTumblers()[TumblerID].GetStatesListAsList()[tumblerStateID];
        }

        set {
            //Debug.Log("ValueType: " + value.GetControlType());

            if (value is string) {
                if (_parentPanelScript.GetTumblers()[TumblerID].GetStatesListAsList().Contains((string) value)) {
                    tumblerStateID = _parentPanelScript.GetTumblers()[TumblerID].GetStatesListAsList()
                        .IndexOf((string) value);
                    changeStateTo((string) value);
                    return;
                }
            }

            if (value is int) {
                //Debug.Log(_parentPanelScript.ToString());
                tumblerStateID = (int) value;
                //changeStateTo(_parentPanelScript.GetTumblers()[TumblerID].GetStatesListAsList()[tumblerStateID]);
                return;
            }

            Debug.Log("Type of object is not recognized");
        }
    }

    public override string GetName() {
        return Tumbler.GetName();
    }

    public override string GetPanelName() {
        if (_parentPanelScript != null)
            return _parentPanelScript.GetName();
        else return "NONE";
    }

    public override ControlType ControlType {
        get { return ControlType.Tumbler; }
    }

    public void SetParentPanelScript(ControlPanelToolkit panelScript) {
        _parentPanelScript = panelScript;
        //Debug.Log("Setting " + _parentPanelScript.GetName());
    }

    public ControlPanelToolkit getParentPanelScript() {
        return _parentPanelScript;
    }

    public void removeParentPanelScript() {
        _parentPanelScript = null;
        Debug.Log("Clearing panel");
    }

    [SerializeField]
    public Tumbler _tumbler;

    public Tumbler Tumbler {
        get {
            if (_parentPanelScript == null) {
                Debug.Log("Parent is not set for this Tumbler");
                return null;
            }

            return _parentPanelScript.GetTumblers()[TumblerID];
        }

        set {
            StatesList = value.GetStatesListAsList();
            //_positions = new Transform[StatesList.Count];

            // Массив с положениями
            Array.Resize(ref _positions, value.GetStatesList().Length);

            List<Tumbler> list = _parentPanelScript.GetTumblers();

            foreach (Tumbler tmblr in list)
                if (tmblr.GetName().Equals(value.GetName())) {
                    TumblerID = list.IndexOf(tmblr);
                    //_tumbler = tmblr;
                    break;
                }

            // Запоминаем список
            //_states = value.GetStatesListAsList();
        }
    }

    [SerializeField]
    int _statesCount = 1;

    [SerializeField]
    public int StatesCount {
        get { return States_Transforms.Length; }
        set {
            if (value == 0 || _statesCount == value) return;
            _statesCount = value;
        }
    }

    // Массив с положениями переключателя
    [SerializeField]
    public Transform[] _positions = null;

    public Transform[] States_Transforms {
        get { return _positions; }
        set { _positions = value; }
    }

    #region Обработка нажатия

    float mouseX = 0, mouseY = 0;
    List<GameObject> gt_list;

    IEnumerator ShowTooltip() {
        gt_list = new List<GameObject>();

        for (int i = 0; i < gt_list.Count; i++) {
            Destroy(gt_list[i]);
        }
        gt_list.Clear();

        states_name = getParentPanelScript().GetTumblers()[TumblerID].GetStatesDescriptions().ToArray();

        GUIText txt;

        float x = (Camera.main.WorldToScreenPoint(States_Transforms[0].position).x / Screen.width);
        float y = (Camera.main.WorldToScreenPoint(States_Transforms[0].position).y / Screen.height);

        gt_list.Add(new GameObject("GUIText_Button_" + this.GetName()));

        Destroy(gt_list[0], 1f);

        gt_list[0].transform.position = new Vector3(x, y, 25);
        gt_list[0].AddComponent<GUIText>();
        gt_list[0].layer = LayerMask.NameToLayer("Ignore Raycast");

        gt_list[0].guiText.fontSize = Screen.height / 20;
        if (gt_list[0].guiText.fontSize > 25) gt_list[0].guiText.fontSize = 25;
        gt_list[0].guiText.fontStyle = FontStyle.Bold;

        gt_list[0].guiText.pixelOffset = new Vector2(-states_name[0].Length * gt_list[0].guiText.fontSize * 0.225f, 0);
        gt_list[0].guiText.text = states_name[tumblerStateID];

        gt_list[0].transform.Translate(0, 0.05f, 0);
        iTween.MoveAdd(gt_list[0], new Vector3(0, 0.05f, 0), 2f);
        yield return new WaitForSeconds(1f);

        //ClearTooltips();
    }

    void ClearTooltips() {
        if (gt_list != null) {
            for (int i = 0; i < gt_list.Count; i++)
                Destroy(gt_list[i]);
            gt_list.Clear();
        }
    }

    void OnMouseDown() {
        if (_positions == null) {
            Debug.Log("Have no positions for this tumbler " + GetName());
            return;
        }

        if (_parentPanelScript == null) return;

        if (isButton) {
            // Если кнопки
            if (!isReversible) // Если не возвращается
            {
                TumblerStateID = NextID();
                ClearTooltips();
                StartCoroutine(ShowTooltip());
                return;
            } else {
                int id = NextID();

                transform.localRotation = States_Transforms[id].localRotation;
                transform.position = States_Transforms[id].position;

                tumblerStateID = id;
                ControlChanged();

                ClearTooltips();
                StartCoroutine(ShowTooltip());
                return;
            }
        }

        mouseX = Input.mousePosition.x;
        mouseY = Input.mousePosition.y;

        if (showTooltip) {
            gt_list = new List<GameObject>();
            states_name = getParentPanelScript().GetTumblers()[TumblerID].GetStatesDescriptions().ToArray();

            GUIText txt;
            GameObject go;


            for (int i = 0; i < states_name.Length; i++) {
                float x = (Camera.main.WorldToScreenPoint(States_Transforms[i].position).x / Screen.width);
                float y = (Camera.main.WorldToScreenPoint(States_Transforms[i].position).y / Screen.height);

                go = new GameObject("GUIText " + i);
                go.transform.position = new Vector3(x, y, 0);
                go.AddComponent<GUIText>();

                gt_list.Add(go);

                gt_list[i].guiText.fontSize = Screen.height / 20;
                if (gt_list[i].guiText.fontSize > 25) gt_list[i].guiText.fontSize = 25;
                gt_list[i].guiText.fontStyle = FontStyle.Bold;

                if (i == tumblerStateID)
                    gt_list[i].guiText.material.color = Color.green;

                gt_list[i].guiText.pixelOffset =
                    new Vector2(-states_name[i].Length * gt_list[i].guiText.fontSize * 0.225f, 0);
                gt_list[i].guiText.text = states_name[i];
            }
        }
    }

    public void OnMouseUp() {
        if (isButton) {
            if (isReversible) {
                TumblerStateID = 0;
                transform.localRotation = States_Transforms[TumblerStateID].localRotation;
                transform.position = States_Transforms[TumblerStateID].position;

                ControlChanged();

                ClearTooltips();
                StartCoroutine(ShowTooltip());
                return;
            }

            ControlChanged();
            return;
        }

        ControlChanged();
        ClearTooltips();
    }

    string[] states_name;

    void OnMouseDrag() {
        if (Tumbler == null) {
            Debug.Log("This tumbler has no settings or panel");
            return;
        }

        // Если это кнопка то нам не нужно обрабатывать перетаскивание
        if (isButton) return;

        if (mouseX == Input.mousePosition.x && mouseY == Input.mousePosition.y) return;

        if (!rotatingNew) {
            TumblerStateID = ClosestTriggerID();

            for (int i = 0; i < gt_list.Count; i++) {
                gt_list[i].guiText.material.color = Color.white;
                if (i == tumblerStateID)
                    gt_list[i].guiText.material.color = Color.green;
            }
        }
    }

    int ClosestTriggerID() {
        int id = -1;
        float mindistance = float.PositiveInfinity;
        float curdistance;

        for (int i = 0; i < StatesCount; i++) {
            // Если трансформа для какого то состояния нет, то переходим дальше
            if (_positions[i] == null) continue;

            curdistance = (Camera.main.WorldToScreenPoint(States_Transforms[i].position) - Input.mousePosition)
                .sqrMagnitude;

            if (curdistance < mindistance) {
                mindistance = curdistance;
                id = i;
            }
        }

        return id;
    }

    public int NextID() {
        int nextID = tumblerStateID + 1;

        if (nextID >= _parentPanelScript.GetTumblers()[TumblerID].GetStatesListAsList().Count) return 0;
        else return nextID;
    }

    string ClosestTriggerState() {
        string state = "Some state transform is missing";

        int id = -1;
        float mindistance = float.PositiveInfinity;
        float curdistance;

        for (int i = 0; i < StatesCount; i++) {
            // Если трансформа для какого то состояния нет, то отменяем поиск, иначе он не полноценный
            if (_positions[i] == null) return state;

            curdistance = (Camera.main.WorldToScreenPoint(States_Transforms[i].position) - Input.mousePosition)
                .sqrMagnitude;

            if (curdistance < mindistance) {
                mindistance = curdistance;
                id = i;
            }
        }

        return Tumbler.GetStatesList()[id];
    }

    public void changeStateTo(string state) {
        try {
            if (getParentPanelScript().GetCore().isVirtual) return;
        } catch {
        }

        Tumbler tmblr = Tumbler;

        //Debug.Log(tmblr.ToString());
        for (int i = 0; i < tmblr.GetStatesListAsList().Count; i++) {
            if (tmblr.GetStatesListAsList()[i].Equals(state)) {
                if (States_Transforms[i] == null) // Если нет трансформа для этой позиции
                {
                    //Debug.Log("Have no Transform for state [" + state + "]. Please, check out SwticherToolkit and set up all Transforms");
                    return;
                }
                //Debug.Log("Changing to [" + state + "]");

                if (Application.isPlaying) {
                    StartCoroutine(RotateObject(this.transform, States_Transforms[i], 0.05f));
                }

                if (Application.isEditor)
                    transform.localRotation = States_Transforms[i].localRotation;
                return;
            }
        }
        Debug.Log("State [" + state + "] of switcher is not exist");
    }


    public IEnumerator RotateObject(Transform start, Transform end, float seconds) {
        if (!rotatingNew) {
            rotatingNew = true;

            Quaternion startRotation = start.localRotation;
            Quaternion endRotation = end.localRotation;
            float t = 0.0f;
            float rate = 1.0f / seconds;
            //audio.PlayOneShot(Sounds.soundTurn);
            while (t < 1.0f) {
                t += Time.deltaTime * rate;
                if (isButton)
                    this.transform.position = Vector3.Lerp(start.transform.position, end.transform.position, t);
                else
                    this.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
                yield return 0;
            }

            rotatingNew = false;
        }
    }

    #endregion

    public override void ControlChanged() {
        if (_parentPanelScript != null) {
            _parentPanelScript.ControlChanged(this);
        } else {
            Debug.Log("Cannot report about change to Parental Panel, because its not defined yet");
        }
    }

    bool rotatingNew = false;
}