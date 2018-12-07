#pragma warning disable 0618
#pragma warning disable 0414

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


[CustomEditor(typeof(ControlPanelToolkit))]
public class ControlPanelToolkitEditor : Editor {    
    
    [SerializeField]
    ControlPanelToolkit controlpanel;

    /** Toggle to use a darker skin which matches the Unity Pro dark skin */
    public static bool useDarkSkin = false;

    bool showAddGraphMenu = false;

    static bool showSettings = false;

    //static bool debugSettings = false;
    static bool colorSettings = false;
    static bool editorSettings = false;
    static bool linkSettings = false;
    static bool editLinks = false;
    static bool aboutArea = false;
    static bool optimizationSettings = false;
    static bool customAreaColorsOpen = false;

    public static bool stylesLoaded = false;

    public EditorGUILayoutx GUILayoutx;
    public static GUISkin astarSkin;
    public static GUIStyle graphBoxStyle;
    public static GUIStyle graphDeleteButtonStyle;
    public static GUIStyle graphInfoButtonStyle;
    public static GUIStyle helpBox;
    public static GUIStyle thinHelpBox;
    public static GUIStyle upArrow;
    public static GUIStyle downArrow;

    public bool enableUndo = true;


    public static string editorAssets = "Assets/PanelToolkit/Editor/EditorAssets";


    [SerializeField] private bool showTumblers = true, showIndicators = true, showSpinners = true, showJoysticks = true;

    public static bool LoadStyles()
    {
        if (useDarkSkin)
        {
            astarSkin = AssetDatabase.LoadAssetAtPath(editorAssets + "/AstarEditorSkinDark.guiskin", typeof(GUISkin)) as GUISkin;
        }
        else
        {
            astarSkin = AssetDatabase.LoadAssetAtPath(editorAssets + "/AstarEditorSkinLight.guiskin", typeof(GUISkin)) as GUISkin;
        }

        if (astarSkin != null)
        {
            astarSkin.button = GUI.skin.button;
            GUI.skin = astarSkin;
        }
        else
        {            
            //Load skin at old path
            astarSkin = AssetDatabase.LoadAssetAtPath(editorAssets + "/AstarEditorSkin.guiskin", typeof(GUISkin)) as GUISkin;
            if (astarSkin != null)
            {
                AssetDatabase.RenameAsset(editorAssets + "/AstarEditorSkin.guiskin", "AstarEditorSkinLight");
            }
            else
            {
                return false;
            }
            //Error is shown in the inspector instead
            //Debug.LogWarning ("Couldn't find 'AstarEditorSkin' at '"+editorAssets + "/AstarEditorSkin.guiskin"+"'");

        }

        EditorGUILayoutx.defaultAreaStyle = astarSkin.FindStyle("PixelBox");

        if (EditorGUILayoutx.defaultAreaStyle == null)
        {
            return false;
        }

        EditorGUILayoutx.defaultLabelStyle = astarSkin.FindStyle("BoxHeader");
        graphBoxStyle = astarSkin.FindStyle("PixelBox3");
        graphDeleteButtonStyle = astarSkin.FindStyle("PixelButton");
        graphInfoButtonStyle = astarSkin.FindStyle("InfoButton");

        upArrow = astarSkin.FindStyle("UpArrow");
        downArrow = astarSkin.FindStyle("DownArrow");

        helpBox = GUI.skin.FindStyle("HelpBox");
        if (helpBox == null) helpBox = GUI.skin.FindStyle("Box");

        thinHelpBox = new GUIStyle(helpBox);
        thinHelpBox.contentOffset = new Vector2(0, -2);
        thinHelpBox.stretchWidth = false;
        thinHelpBox.clipping = TextClipping.Overflow;
        thinHelpBox.overflow.top += 1;

        return true;
    }

    public void GetEditorSettings()
    {
        EditorGUILayoutx.fancyEffects = EditorPrefs.GetBool("EditorGUILayoutx.fancyEffects", true);
        enableUndo = EditorPrefs.GetBool("enableUndo", false);
        useDarkSkin = EditorPrefs.GetBool("UseDarkSkin", false);
    }
    public void SetEditorSettings()
    {
        EditorPrefs.SetBool("EditorGUILayoutx.fancyEffects", EditorGUILayoutx.fancyEffects);
        EditorPrefs.SetBool("enableUndo", enableUndo);
        EditorPrefs.SetBool("UseDarkSkin", useDarkSkin);
    }

    void OnEnable()
    {
        controlpanel = target as ControlPanelToolkit;

        List<Component> list = new List<Component>(controlpanel.gameObject.GetComponents(typeof(ControlPanelToolkit)));

        if (list.Count > 1)
        {
            DestroyImmediate(list[1]);
            Debug.Log("One object cannot contain two ControlPanelToolkit scritps");
        }

        GetEditorSettings();
        GUILayoutx = new EditorGUILayoutx();
        EditorGUILayoutx.editor = this;
    }

    void OnDisable()
    {
        SetEditorSettings();
    }
    public override void OnInspectorGUI()
    {
        #region Загрузка настроек
        if (!stylesLoaded)
        {
            if (!LoadStyles())
            {
                GUILayout.Label("The GUISkin 'AstarEditorSkin.guiskin' in the folder " + editorAssets + "/ was not found or some custom styles in it does not exist.\nThis file is required for the A* Pathfinding Project editor.\n\nIf you are trying to add A* to a new project, please do not copy the files outside Unity, export them as a UnityPackage and import them to this project or download the package from the Asset Store or the 'scripts only' package from the A* Pathfinding Project website.\n\n\nSkin loading is done in AstarPathEditor.cs --> LoadStyles function", "HelpBox");
                return;
            }
            else
            {
                stylesLoaded = true;
            }
        }

        bool preChanged = GUI.changed;
        GUI.changed = false;
        EditorGUILayoutx.editor = this;
        EditorGUI.indentLevel = 1;
        EditorGUIUtility.LookLikeInspector();
        //EventType eT = Event.current.type;
        #endregion

        GUILayoutx.BeginFadeArea(true, "Body", 20, EditorGUILayoutx.defaultAreaStyle);


        if (controlpanel.GetCore() != null)
        {
            EditorGUILayout.LabelField("Core: ", "");
            EditorGUILayout.ObjectField(controlpanel.GetCore().GetTransform(), typeof(Transform));
        }


        if (controlpanel.GetPanelLibraryObject() == null)
        {
            controlpanel.SetPanelLibraryObject((UnityEngine.Object)EditorGUILayout.ObjectField("PanelLibray:", controlpanel.GetPanelLibraryObject(), typeof(UnityEngine.Object)));
        }
        else
        {
            // Если выбран объект библиотеки

            GUILayoutx.BeginFadeArea(true, "Panel's Library", "Library", graphBoxStyle);

            GUILayout.Label(controlpanel.Library.Name, helpBox);

            if (GUILayout.Button("Remove " + controlpanel.Library.Name))
            {
                controlpanel.ClearPanelLibrary(); // Удаляем текущую библиотеку для это панели
                controlpanel.RemoveCore();
                return;
            }

            GUILayoutx.EndFadeArea();


            EditorGUILayout.Space();
            EditorGUILayout.Space();

            // Пока еще не выбрана панель
            if (controlpanel.Panel == null)
            {
                // Выбираем панель

                GUILayoutx.BeginFadeArea(true, "Panel", "ChoosePanel", graphBoxStyle);
                controlpanel.PanelIndex = EditorGUILayout.Popup("Choose panel:", controlpanel.PanelIndex, controlpanel.Library.GetPanelNames().ToArray()); // Индекс панели

                if (GUILayout.Button("Apply"))
                {
                    controlpanel.Panel = controlpanel.Library.Panels[controlpanel.PanelIndex]; // Удаляем текущую библиотеку для это панели
                }
                GUILayoutx.EndFadeArea();
                EditorGUILayout.Space();

            }
            // Если выбрана панель
            if (controlpanel.Panel != null)
            {
                GUILayoutx.BeginFadeArea(true, "Panel", "Panel", graphBoxStyle);

                GUILayout.Label(controlpanel.Panel.ToString(), helpBox);
                if (GUILayout.Button("Remove"))
                {
                    controlpanel.ClearPanel(); // Удаляем текущую библиотеку для это панели
                    controlpanel.RemoveCore();
                }
                GUILayoutx.EndFadeArea();

                List<string> list_control_descriptions;
                List<string> list_control_names;
                foreach (ControlType value in Enum.GetValues(typeof(ControlType)))
                {
                    // Выводим название группы объектов на панели
                    foreach (FieldInfo fieldInfo in typeof(ControlType).GetFields())
                    {
                        //Debug.Log("VALUE: " + value);
                        if (fieldInfo.GetValue(value).ToString() == value.ToString())
                        {
                            EnumDescription[] attrs = (EnumDescription[])fieldInfo.GetCustomAttributes(typeof(EnumDescription), false);
                            if (attrs.Length > 0)
                            {
                                EditorGUILayout.Space();
                                EditorGUILayout.Space();
                                //EditorGUILayout.LabelField("", "-" + attrs[0].Text + "-");

                                switch (value)
                                {
                                    case ControlType.Tumbler:
                                        showTumblers = GUILayoutx.BeginFadeArea(showTumblers, attrs[0].Text, "Tumblers", graphBoxStyle);
                                        break;

                                    case ControlType.Indicator:
                                        showIndicators = GUILayoutx.BeginFadeArea(showIndicators, attrs[0].Text, "Indicators", graphBoxStyle);
                                        break;

                                    case ControlType.Spinner:
                                        showSpinners = GUILayoutx.BeginFadeArea(showSpinners, attrs[0].Text, "Spinners", graphBoxStyle);
                                        break;

                                    case ControlType.Joystick:
                                        showJoysticks = GUILayoutx.BeginFadeArea(showJoysticks, attrs[0].Text, "Joysticks", graphBoxStyle);
                                        break;

                                    default:
                                        Debug.Log("Unrecognized controltype [" + value + "]");
                                        GUILayoutx.BeginFadeArea(true, value.ToString(), value.ToString(), graphBoxStyle);
                                        break;
                                }
                            }
                        }
                    }

                    list_control_descriptions = controlpanel.GetPanelObjectsDecriptions(value);
                    list_control_names = controlpanel.GetPanelObjectsNames(value);

                    for (int i = 0; i < list_control_descriptions.Count; i++)
                    {
                        GUILayoutx.BeginFadeArea(true,  "[" + list_control_names[i] + "] | " + list_control_descriptions[i], list_control_descriptions[i], EditorGUILayoutx.defaultAreaStyle);    
                        GUILayoutx.BeginFadeArea(true, "", list_control_descriptions[i]+"details", graphBoxStyle);

                        EditorGUILayout.BeginHorizontal();

                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.BeginHorizontal();

                        //GUILayoutx.BeginFadeArea(true, "", listnames[i]+"details", graphBoxStyle);
                        
                        controlpanel.SetGameObject(value, (GameObject)EditorGUILayout.ObjectField(controlpanel.GetGameObject(value, i), typeof(GameObject)), i);
                       
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();

                            EditorGUILayout.BeginVertical();

                            if (controlpanel.GetGameObject(value, i) != null)
                            {
                                switch (value)
                                {
                                    case ControlType.Tumbler:
                                        controlpanel.SwitcherScripts[i].TumblerStateID = EditorGUILayout.Popup(controlpanel.SwitcherScripts[i].TumblerStateID, controlpanel.GetTumblers()[i].GetStatesList());
                                        break;

                                    case ControlType.Spinner:
                                        controlpanel.SpinnerScripts[i].Value = EditorGUILayout.IntSlider(controlpanel.SpinnerScripts[i].Value, controlpanel.SpinnerScripts[i].MinimalValue, controlpanel.SpinnerScripts[i].MaximalValue);
                                        break;

                                    case ControlType.Indicator:
                                        controlpanel.IndicatorScripts[i].IndicatorStateID = EditorGUILayout.Popup(controlpanel.IndicatorScripts[i].IndicatorStateID, controlpanel.GetIndicators()[i].GetStatesList());
                                        break;

                                    case ControlType.Joystick:

                                        if (controlpanel.JoystickScripts[i].GetParentPanelScript() == null)
                                        {
                                            controlpanel.JoystickScripts[i]._parentPanelScript = controlpanel;
                                        }

                                        if (controlpanel.JoystickScripts[i]._state.X.Availiable)
                                        {
                                            EditorGUILayout.Foldout(true, "Axes X:");
                                            controlpanel.JoystickScripts[i]._state.X.Value =
                                                EditorGUILayout.IntSlider("",
                                                                          controlpanel.JoystickScripts[i]._state.X.Value,
                                                                          controlpanel.JoystickScripts[i]._state.X.Min,
                                                                          controlpanel.JoystickScripts[i]._state.X.Max);
                                        }

                                        if (controlpanel.JoystickScripts[i]._state.Y.Availiable)
                                        {
                                            EditorGUILayout.Foldout(true, "Axes Y:");
                                            controlpanel.JoystickScripts[i]._state.Y.Value =
                                                EditorGUILayout.IntSlider("",
                                                                          controlpanel.JoystickScripts[i]._state.Y.Value,
                                                                          controlpanel.JoystickScripts[i]._state.Y.Min,
                                                                          controlpanel.JoystickScripts[i]._state.Y.Max);
                                        }

                                        if (controlpanel.JoystickScripts[i]._state.Z.Availiable)
                                        {
                                            EditorGUILayout.Foldout(true, "Axes Z:");
                                            //EditorGUILayout.HelpBox("Axes Z:", MessageType.None);

                                            controlpanel.JoystickScripts[i]._state.Z.Value =
                                                EditorGUILayout.IntSlider("",
                                                                          controlpanel.JoystickScripts[i]._state.Z.Value,
                                                                          controlpanel.JoystickScripts[i]._state.Z.Min,
                                                                          controlpanel.JoystickScripts[i]._state.Z.Max);
                                        }
                                        break;

                                    default:
                                        Debug.Log("Cant recognize PanelObject type");
                                        return;
                                }

                                EditorGUILayout.EndVertical();

                                EditorGUILayout.BeginVertical();
                                if (GUILayout.Button("X", GUILayout.MinHeight(15), GUILayout.MaxHeight(15)))
                                {
                                    controlpanel.RemoveTumbler(value, i);
                                }
                                EditorGUILayout.EndVertical();
                            }
                            else
                            {
                                EditorGUILayout.EndVertical();
                            }

                            EditorGUILayout.EndHorizontal();
                            GUILayoutx.EndFadeArea();
                        GUILayoutx.EndFadeArea();
                    }

                    GUILayoutx.EndFadeArea();
                }

            }
        }

        GUI.changed = preChanged || GUI.changed;

        if (GUI.changed)
        {
            HandleUtility.Repaint();
            //EditorUtility.SetDirty(controlpanel);
        }

        GUILayoutx.EndFadeArea();
        EditorGUIUtility.LookLikeInspector();

    }

    void OnDestroy()
    {
        /* Вызывается при удалении компонента и смене фокуса на другой объект
        при вызове этого метода, в списке тумблеров панели находим текущий тумблер, и если
        на нем нет ControlPanelToolkit - удаляем его GameObject у Core */

        // Если нет родительской панели - ничего не делаем
        //if (controlpanel.GetCore() == null) return;

        //// Если на тумблере не висит скрипта SwitcherToolkit - удаляем его с панели
        //if (!controlpanel.GetCore().ContainsPanelToolkit(controlpanel))
        //{
        //    Debug.Log("Panel [" + controlpanel.GetName() + "] was removed from Core [" + controlpanel.GetCore().ToString() + "]");
        //    controlpanel.GetCore().RemovePanel(controlpanel);
        //}
    }

}
