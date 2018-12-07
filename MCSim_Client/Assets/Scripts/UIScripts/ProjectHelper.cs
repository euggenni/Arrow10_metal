using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System.Collections;

public static class ProjectHelper
{

    //MCSSerializer
	public static Vector2 GetMainGameViewSize()
	{
		System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
		System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
		System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
		return (Vector2)Res;
	}

    //public static IEnumerable<Type> GetAllDerivedTypesOf(Type baseType)
    //{
    //    var types = Assembly.GetAssembly(baseType).GetTypes();
    //    return types.Where(t => t.IsSubclassOf(baseType));
    //}

    /// <summary>
    /// Возвращает все классы, которые наследуются от базового класса.
    /// </summary>
    /// <param name="baseType">Базовый класс.</param>
    public static IEnumerable<Type> GetAllDerivedTypesOf(Type baseType)
    {
        var types = Assembly.GetAssembly(baseType).GetTypes();

        return types.Where(baseType.IsAssignableFrom).Where(t => t != baseType);
    }

    /// <summary>
    /// Реализует ли объект данный интерфейс
    /// </summary>
    /// <typeparam name="T">Интерфейс</typeparam>
    /// <param name="comp"></param>
    /// <returns></returns>
    public static bool ImplementedBy<T>(this Component comp)
    {
        if (comp.GetType().GetInterfaces().Contains(typeof(T)))
        {
            return true;
        }
        return false;
    }

        
    ///// <summary>
    ///// Уничтожить объект через указанное количество секунд
    ///// </summary>
    ///// <param name="go"></param>
    ///// <param name="seconds"></param>
    //public static void DestroyInSeconds(this GameObject go, float seconds)
    //{
        
    //}

    //private static IEnumerator IDestroyInSeconds(GameObject go, float seconds)
    //{
    //    yield return new WaitForSeconds(seconds);

    //    GameObject.Destroy(go);
    //}


	 public static T ForceComponent<T>(this MonoBehaviour mb) where T : Component
	 {
		 return ForceComponent<T>(mb.gameObject);
	 }

	 public static T ForceComponent<T>(this GameObject go) where T : Component
	 {
		 T component = null;
		 component = go.GetComponent<T>();
		 if (!component) component = go.AddComponent<T>();

		 return component;
	 }
}
