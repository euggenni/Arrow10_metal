using System;
using UnityEngine;

public enum MCSFlagType {
    Ground,
    Air
}

public class MCSUICenter : MonoBehaviour {
    public static GameObject Anchor;

    private static Camera _guiCamera;

    /// <summary>
    /// Камера, отрисовывающая GUI.
    /// </summary>
    public static Camera GUICamera {
        get {
            if (!_guiCamera)
                _guiCamera = GameObject.Find("GUI Camera").GetComponent<Camera>();

            return _guiCamera;
        }
    }

    private static Camera _mainCamera;

    /// <summary>
    /// Главная камера.
    /// </summary>
    public static Camera MainCamera {
        get {
            if (!_mainCamera)
                _mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

            return _mainCamera;
        }
    }

    /// <summary>
    /// Коэффициент разрешения
    /// </summary>
    public static float sizeK = 1f;

    private static GameObject _lightHouse;

    /// <summary>
    /// Точка-маяк
    /// </summary>
    public static GameObject LightHouse {
        get { return _lightHouse; }
        set { _lightHouse = value; }
    }

    /// <summary>
    /// Уровень GUI Маяка
    /// </summary>
    public static GameObject LightHouseGUI { get; private set; }

    /// <summary>
    /// Объект с ссылками на контейнеры и префабы
    /// </summary>
    public static GUILibrary Store;

    /// <summary>
    /// Возвращает GameObject, находящийся под указателем мыши. Необходим коллайдер на этом объекте, чтобы он определился.
    /// </summary>
    /// <returns></returns>
    public static GameObject MouseOverObject() {
        Ray r1 = GUICamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit1;
        if (Physics.Raycast(r1, out hit1, 1000f)) {
            return hit1.transform.gameObject;
        }

        return null;
    }


    /// <summary>
    /// Открыть окно с указанным именем
    /// </summary>
    /// <param name="windowName"></param>
    void OpenWindow(string windowName) {
        GameObject go = null;

        // Если уже находится в памяти - выводим, центрируем
        try {
            (Store.Windows["Window_WeaponryMenu"].GetComponent<MCSUIObject>()).Show();
            return;
        } catch (Exception) {
        }

        switch (windowName) {
            case "WeaponryMenu":
                go = (GameObject) Instantiate(Store.Prefab_Window_WeaponryMenu);
                Store.Windows["Window_WeaponryMenu"] = go;
                break;

            default:
                Debug.Log("Не найдено окно с именем [" + windowName + "]");
                return;
        }

        if (go) {
            go.transform.parent = Store.Container_GUI_Main.transform;
            go.transform.localScale = Vector3.one;
            go.layer = LayerMask.NameToLayer("GUI");
        }
    }

    void Start() {
        Anchor = gameObject.transform.Find("GUI Camera/Anchor").gameObject;
    }
}