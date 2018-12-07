using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

public class MCSTrainingBehaviour : MonoBehaviour {
    public Object _libraryObject;

    public MCSTrainingInfo GetInfoObject() {
        if (_libraryObject != null) {
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes()) {
                // Находим объекты того же типа что и PanelLibrary - библиотеки пультов
                if (t.BaseType == typeof(MCSTrainingInfo) && t.GetConstructor(Type.EmptyTypes) != null) {
                    //Если имя вставляемого скрипта совпадает с именем найденного типа
                    if (t.Name.Equals(_libraryObject.name)) {
                        Debug.Log(t.Name);
                        return (MCSTrainingInfo) Assembly.GetExecutingAssembly().CreateInstance(t.FullName);
                    }
                }
            }
        }

        Debug.Log("NULL");
        return null;
    }
}