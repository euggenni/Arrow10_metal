using System.Collections.Generic;
using System.Reflection;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Linq;
using System;

#pragma warning disable 0618
#pragma warning disable 0414

public class CoreToolkit : MonoBehaviour, CoreLibrary.Core
{
    private event CoreLibrary.OnControlChanged OnControlChanged;

    private void CallOnControlChanged(PanelControl control)
    {
        CoreLibrary.OnControlChanged handler = OnControlChanged;
        if (handler != null) handler(control);
    }

    public void SubscribeOnControlChanged(CoreLibrary.OnControlChanged callback)
    {
        OnControlChanged += callback;
    }

    public void UnsubscribeFromOnControlChanged(CoreLibrary.OnControlChanged callback)
    {
        try
        {
            OnControlChanged -= callback;
        }
        catch { Debug.LogError("Ошибка при отписывании от события OnControlChanged"); }
    }


    [SerializeField]
    private GameObject _handlerObject = null;
    public GameObject HandlerObject
    {
        get { return _handlerObject; }
        set
        {
            if (value == null || value == _handlerObject) return;

            if (_handler = value.GetComponent<CoreLibrary.CoreHandler>())
            {
                _handlerObject = value;
            }
            else
            {
                Debug.Log("На объекте отсутствует компонент [CoreHandler]");
            }
        }
    }

    private CoreLibrary.CoreHandler _handler;
    public CoreLibrary.CoreHandler Handler
    {
        get
        {
            if (_handler) return _handler;

            if (_handlerObject)
            {
                if (_handler = _handlerObject.GetComponent<CoreLibrary.CoreHandler>())
                {
                    return _handler;
                }
                else
                {
                    Debug.Log("На имеющемся объекте отсутствует компонент [CoreHandler]");
                }
            }
            else
            {
                Debug.Log("Отсутствует объект с [CoreHandler]");
            }

            return null;
        }
    }

    [SerializeField]
    private GameObject _weaponryObject; // Объект со скриптом Weaponry
    public GameObject WeaponryObject
    {
        get { return _weaponryObject; }
        set
        {
            if (_weaponryObject == value) return;

            if ((_weaponry = value.GetComponent<Weaponry>()) != null)
                _weaponryObject = value;
        }
    }

    private Weaponry _weaponry;
    public Weaponry Weaponry
    {
        get
        {
            if (_weaponry != null) return _weaponry;

            // Если ссылка на скрипт утеряна, но объект нет
            if (_weaponry == null && _weaponryObject != null)
                if ((_weaponry = _weaponryObject.GetComponent<Weaponry>()) != null)
                    return _weaponry;

            Debug.Log("На объекте отсутствует компонент Weaponry");
            _weaponryObject = null;
            return null;
        }
        set { _weaponry = value; }
    }

    /// <summary>
    /// Ищет среди родителей скрипт Weaponry и запоминает его.
    /// </summary>
    public void FindWeaponry()
    {
        Transform seeker = this.transform;

        // Бежим по всем родителям, ищем Weaponry
        while ((seeker = seeker.parent) != null)
        {
            if ((_weaponry = seeker.GetComponent<Weaponry>()) != null)
            {
                _weaponryObject = seeker.gameObject;
                Debug.Log("Founded [" + _weaponry.GetType().FullName + "]");
                return;
            }
        }

        //Debug.Log("Cannot find it");
    }

    [SerializeField]
    private UnityEngine.Object _libraryObject; // Объект с нужным скриптом-библиотекой

    [SerializeField]
    private GameObject _coreObject; // Объект, на который во время виртуализации будут помещаться компоненты, связанные с Core 

    [SerializeField]
    private Library _library;
    public Library Library
    {
        get
        {
            //Debug.Log("plo: " + panellibraryobject.name);
            // Мы не можем сериализовать (запомнить) объект нашего класса, но можем запомнить скрипт как объект
            // и потом по нему восстановить объект нашего класса
            if (_library != null) return _library;

            if (_library == null && _libraryObject != null)
            {
                LoadLibrary();
            }

            return _library;
        }

        set
        {
            if (_library == value) return;
            Debug.Log("Setted " + value.Name);

            _library = value;
            LoadLibrary();
        }
    }

    private bool _isvirtual;
    public bool isVirtual
    {
        get { return _isvirtual; }
    }

    [SerializeField]
    private GameObject[] _panelObject = null;

    [SerializeField]
    private List<ControlPanelToolkit> _panelScript = null;
    public List<ControlPanelToolkit> Panels
    {
        get { return _panelScript; }
    }


    public void SetPanelObject(int index, GameObject panelObject)
    {
        if (panelObject == null) return;

        if (panelObject.GetComponent<ControlPanelToolkit>() != null)
        {
            var script = panelObject.GetComponent<ControlPanelToolkit>();

            if (script.Panel != null)
            {
                //Debug.Log(script.Panel.GetType().ToString());
                //Debug.Log(Library.Panels[index].ToString());
                if (script.Panel.GetType().ToString() == Library.Panels[index].ToString())
                {
                    _panelScript[index] = script;
                    _panelObject[index] = panelObject;

                    // Устанавливаем обратную связь панели с ядром
                    script.SetCoreObject(this.gameObject);
                }
                else
                {
                    Debug.LogWarning("Type of this panel is [" + script.Panel.GetType() + "] doesnt equals required type [" +
                              Library.Panels[index] + "]");
                }
            }
            else
            {
                Debug.LogWarning("Panel is not set for this ControlPanelToolkit");
            }
        }
        else
        {
            Debug.LogWarning("This object doesnt contains ControlPanelToolkit script. Added a new one...");

            ControlPanelToolkit cpt = panelObject.AddComponent<ControlPanelToolkit>();
            cpt.SetPanelLibraryObject(_libraryObject);

            cpt.Panel = Library.Panels[index];

            // Устанавливаем обратную связь панели с ядром
            cpt.SetCoreObject(this.gameObject);

            _panelScript[index] = cpt;
            _panelObject[index] = panelObject;
        }
    }
    public GameObject GetPanelObject(int index)
    {
        return _panelObject[index];
    }

    // Загрузка Library из указанного объекта _libraryObject
    private bool LoadLibrary()
    {
        if (_libraryObject == null && _library == null)
        {
            Debug.Log("Не можем получить библиотеку, т.к. не указан объект, содержащий ее");
            return false;
        }


        // Если создаем Library из кода, то _libraryObject = null, и дальнейший код вызовет ошибку
        // Поэтому, если библиотека уже указана, загружаем для нее параметры
        if (_library != null)
        {
            try
            {
                Debug.Log("!!PanelsCount: " + _library.Panels.Count);

                _panelObject = new GameObject[_library.Panels.Count];
                _panelScript = new List<ControlPanelToolkit>(_library.Panels.Count);

                MCSimCoreToolkitHelper.FillByNulls(_panelObject);
                MCSimCoreToolkitHelper.FillByNulls(_panelScript);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        // * Загружаем Library из объекта * //
        List<Type> list;
        foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
        {
            // Находим объекты того же типа что и PanelLibrary - библиотеки пультов
            if (t.BaseType == typeof(Library) && t.GetConstructor(Type.EmptyTypes) != null)
            {
                //Если имя вставляемого скрипта совпадает с именем найденного типа
                if (t.Name.Equals(_libraryObject.name))
                {
                    _library = (Library)Assembly.GetExecutingAssembly().CreateInstance(t.FullName);
                    //Debug.Log("Создан инстанс для " + _library);
                    //Debug.Log("PanelsCount: " + _library.Panels.Count);

                    try
                    {
                        // Если уже созданы списки панелей - выходим, иначе они сбросятся. А так, они сбрасываются когда мы нажимаем Remove у Library
                        if (_panelObject == null || _panelObject.Length == 0)
                        {
                            _panelObject = new GameObject[_library.Panels.Count];
                            _panelScript = new List<ControlPanelToolkit>(_library.Panels.Count);

                            MCSimCoreToolkitHelper.FillByNulls(_panelObject);
                            MCSimCoreToolkitHelper.FillByNulls(_panelScript);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }

                    return true;
                }
            }
        }

        _libraryObject = null;
        return false;
    }

    // Установка объекта, содержащего класс-библиотеку
    public void SetLibraryObject(UnityEngine.Object obj)
    {
        if (obj == null) return;
        if (obj.Equals(_libraryObject)) { return; }

        _libraryObject = obj;
        LoadLibrary();
        Debug.Log("Загрузили библиотеку");
    }

    // Получение объекта, содержащего класс-библиотеку
    public UnityEngine.Object GetLibraryObject()
    {
        return _libraryObject;
    }

    public void RemoveLibrary()
    {
        //Debug.Log("Очищаем библиотеку панели и саму панель");

        //if(Panels != null)
        foreach (ControlPanelToolkit panel in Panels)
        {
            if (panel != null) panel.RemoveCore();
        }

        _library = null;
        _libraryObject = null;
        _panelObject = null;
        _panelScript = null;

        _handlerObject = null;
        _handler = null;
    }

    public void RemovePanel(int index)
    {
        if (Panels[index] != null)
            _panelScript[index].RemoveCore();

        _panelObject[index] = null;
        _panelScript[index] = null;
    }


    /// <summary>
    /// Возвращает ControlPanelToolkit панели
    /// </summary>
    /// <param name="PanelName">Имя панели</param>
    public ControlPanelToolkit GetPanel(string PanelName)
    {
	    ControlPanelToolkit res = null;

	    foreach (var panel in Panels)
	    {
		    if (panel.GetName().Equals(PanelName))
			    res = panel;
	    }
        //ControlPanelToolkit res = Panels.First(toolkit => toolkit.GetName().Equals(PanelName));

        if (res == null){
            Debug.LogWarning("Panel with name [" + PanelName + "] was not found");
        }

        return res;
    }

    public Transform GetTransform()
    {
        return this.transform;
    }

    public void ControlChanged(PanelControl control)
    {
        if (Handler == null)
        {
            Debug.Log("Handler не установлен для Core [" + Library.Name + "] на GO [" + gameObject.name + "]");
            return;
        }

        Handler.ControlChanged(control);

        CallOnControlChanged(control); // !!!!!!!!!!!!!
    }

    public bool ContainsPanelToolkit(ControlPanelToolkit cpt)
    {
        return _panelScript.Contains(cpt);
    }

    public void RemovePanel(ControlPanelToolkit cpt)
    {
        RemovePanel(_panelScript.IndexOf(cpt));
    }

    public void SendCommandMsg(string command)
    {
    }

	/// <summary>
	/// Создает виртуальное Ядро. Происходит инициализация всех панелей и их контролов.
	/// </summary>
	public void Virtualize()
	{
		if (_library == null)
		{
			if (_libraryObject != null) LoadLibrary();
			else
			{
				Debug.LogWarning("Library of components for this Core is missing.");
				return;
			}
		}

		Weaponry = gameObject.GetComponentInParents<Weaponry>(false);

		// Создаем объект-хранилище
		_coreObject = new GameObject("Core_" + Library.Name + "(Virtual)");
		//_coreObject.transform.parent = Weaponry.transform;

		//if (_coreObject.collider)
		//    _coreObject.collider.isTrigger = true;

		// Создаем массив панелей
		_panelScript = new List<ControlPanelToolkit>();

		foreach (PanelLibrary pl in Library.Panels)
		{
			ControlPanelToolkit cpt;
			_panelScript.Add(cpt = _coreObject.AddComponent<ControlPanelToolkit>());

			cpt.Library = Library; // Загружаем библиотеку в ControlPanelToolkit
			cpt.Panel = pl; // Указываем панель в ControlPanelToolkit
			cpt.Core = this;

			int index = 0;
			//Debug.Log(">>>>>>>>>>>>>>>" + cpt.GetName()+ "");
			//Debug.Log("Tumblers:");
			foreach (Tumbler tmblr in pl.GetTumblers())
			{
				SwitcherToolkit st = _coreObject.AddComponent<SwitcherToolkit>();
				st.SetParentPanelScript(cpt);
				st.Tumbler = tmblr; // Указываем тумблер

				cpt.SwitcherScripts[index++] = st;
			}

			//Debug.Log("Spinners:");
			index = 0;
			foreach (Spinner spnnr in pl.GetSpinners())
			{
				SpinnerToolkit st = _coreObject.AddComponent<SpinnerToolkit>();
				st.SetParentPanelScript(cpt);

				st.SpinnerID = index;
				st.MinimalValue = spnnr.GetMinValue();
				st.MaximalValue = spnnr.GetMaxValue();

				cpt.SpinnerScripts[index++] = st;
			}


			// * Индикаторы не нужны, на сервере не обязательно знать их значение * //

			//foreach (Tumbler indctr in pl.GetIndicators())
			//{
			//    IndicatorToolkit it = _coreObject.AddComponent<IndicatorToolkit>();
			//    st.SetParentPanelScript(cpt);

			//    st.SpinnerID = index;
			//    st.MinimalValue = spnnr.GetMinValue();
			//    st.MaximalValue = spnnr.GetMaxValue();

			//    Debug.Log("State of [" + st.GetName() + " is [" + st.State + "]");

			//    index++;
			//}

			index = 0;
			//Debug.Log("Joysticks:");
			foreach (Joystick jstck in pl.GetJoysticks())
			{
				JoystickToolkit jt = _coreObject.AddComponent<JoystickToolkit>();
				jt.SetParentPanelScript(cpt);
				jt.SetName(jstck.GetName());
				jt.ControlID = index;

				cpt.JoystickScripts[index++] = jt;
			}

			_isvirtual = true;
		}
	}

	/// <summary>
    /// Создает виртуальное Ядро. Происходит инициализация всех панелей и их контролов.
    /// </summary>
    /// <param name="library">Библиотека, на основе которой будет создаваться виртуальное Ядро</param>
    public void Virtualize(Library library)
    {
        _library = library;
        if (LoadLibrary())
        {
            Virtualize();
        }
    }

    /// <summary>
    /// Возвращает имя ядра
    /// </summary>
    public string Name
    {
        get
        {
            if (Library != null) return Library.Name;
            else return "NotNamedCore";
        }
    }
}
