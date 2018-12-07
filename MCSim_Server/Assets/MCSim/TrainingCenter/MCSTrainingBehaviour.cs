using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

public class MCSTrainingBehaviour : MonoBehaviour {
    
    public  UnityEngine.Object _libraryObject;

    public MCSTrainingInfo GetInfoObject()
    {
        if (_libraryObject != null)
        {
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                // Находим объекты того же типа что и PanelLibrary - библиотеки пультов
                if (t.BaseType == typeof(MCSTrainingInfo) && t.GetConstructor(Type.EmptyTypes) != null)
                {
                    //Если имя вставляемого скрипта совпадает с именем найденного типа
                    if (t.Name.Equals(_libraryObject.name))
                    {
                        return  (MCSTrainingInfo) Assembly.GetExecutingAssembly().CreateInstance(t.FullName);                        
                    }
                }
            }
        }

        return null;
    }
}
