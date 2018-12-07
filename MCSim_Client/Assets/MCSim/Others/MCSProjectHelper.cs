using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MilitaryCombatSimulator;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MCSProjectHelper {
    //public static IEnumerable<Type> GetAllDerivedTypesOf(Type baseType)
    //{
    //    var types = Assembly.GetAssembly(baseType).GetTypes();
    //    return types.Where(t => t.IsSubclassOf(baseType));
    //}

    /// <summary>
    /// Возвращает все классы, которые наследуются от базового класса.
    /// </summary>
    /// <param name="baseType">Базовый класс.</param>
    public static IEnumerable<Type> GetAllDerivedTypesOf(Type baseType) {
        var types = Assembly.GetAssembly(baseType).GetTypes();

        return types.Where(baseType.IsAssignableFrom).Where(t => t != baseType);
    }

    /// <summary>
    /// Возвращает описание поля, если у него прописан параметр EnumDescription
    /// </summary>
    /// <typeparam name="T">Тип поля</typeparam>
    /// <param name="enumValue">Поле с описанием</param>
    public static string GetDescription<T>(T enumValue) {
        //BindingFlags.Public | BindingFlags.Static
        foreach (FieldInfo fieldInfo in typeof(T).GetFields()
            .Where(fieldInfo => fieldInfo.Name.Equals(enumValue.ToString()))) {
            //Debug.Log(">>" + fieldInfo.Name + " >> " + enumValue);
            //if (!fieldInfo.Name.Equals(enumValue.ToString())) continue;

            EnumDescription[] attrs = (EnumDescription[]) fieldInfo.GetCustomAttributes(typeof(EnumDescription), false);

            // если найден атрибут EnumDescription, добавляем строку в выпадающий список, иначе пропускаем
            if (attrs.Length > 0) {
                return attrs[0].Text;
            }
        }
        return "NotDefined";
    }

    /// <summary>
    /// Реализует ли объект данный интерфейс
    /// </summary>
    /// <typeparam name="T">Интерфейс</typeparam>
    /// <param name="comp"></param>
    /// <returns></returns>
    public static bool ImplementedBy<T>(this Component comp) {
        if (comp.GetType().GetInterfaces().Contains(typeof(T))) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Размещение в сети указанного Weaponry
    /// </summary>
    /// <param name="weaponry">Weaponry для размещения</param>
    public static Weaponry NetworkInstantiate(this GameObject weaponry) {
        if (weaponry.GetComponent<Weaponry>()) {
            return MCSGlobalSimulation.Instantiate(weaponry.gameObject);
        }
        return null;
    }

    /// <summary>
    /// Поиск указанного элемента среди родителей
    /// </summary>
    /// <param name="includeThis">Включать ли данный GameObject в поиск</param>
    public static T GetComponentInParents<T>(this GameObject go, bool includeThis) where T : Component {
        if (go == null) return null;

        object comp = go.transform.GetComponent<T>();
        if (!includeThis)
            comp = go.transform.parent.GetComponent<T>();

        if (comp == null) {
            Transform t = go.transform.parent;

            while (t != null && comp == null) {
                comp = t.gameObject.GetComponent<T>();
                t = t.parent;
            }
        }

        return (T) comp;
    }

    /// <summary>
    /// Поиск указанного элемента среди родителей
    /// </summary>
    /// <param name="includeThis">Включать ли данный GameObject в поиск</param>
    public static List<T> GetComponentsInParents<T>(this GameObject go, bool includeThis) where T : Component {
        List<T> list = new List<T>();

        if (go == null) return null;

        object comp = go.transform.GetComponent<T>();
        if (!includeThis)
            comp = go.transform.parent.GetComponent<T>();


        Transform t = go.transform.parent;

        while (t != null) {
            comp = t.gameObject.GetComponent<T>();

            if (comp != null)
                list.Add((T) comp);

            t = t.parent;
        }


        return list;
    }


    /// <summary>
    /// Шатануть на части указанный объект
    /// </summary>
    /// <param name="go">Объект для шатания</param>
    /// <param name="explosion"></param>
    public static void FallAPart(this GameObject go, Vector3 explosion) {
        GameObject trash = new GameObject();
        trash.name = go.name + "(Trash)";


        foreach (MeshRenderer part in go.GetComponentsInChildren<MeshRenderer>()) {
            int rand = Random.Range(0, 2);
            if (rand == 1) {
                part.transform.parent = trash.transform;
                part.gameObject.ExplodeInRandomDirection();

                GameObject.Destroy(part, 20f);
            }
        }

        go.ExplodeInRandomDirection();
        GameObject.Destroy(go, 20f);
    }

    /// <summary>
    /// Толкнуть GameObject в случайном направлении
    /// </summary>
    /// <param name="go"></param>
    private static void ExplodeInRandomDirection(this GameObject go) {
        Rigidbody rigidbody;
        if (!(rigidbody = go.rigidbody))
            rigidbody = go.AddComponent<Rigidbody>();

        rigidbody.mass = 100;
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        rigidbody.velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f),
                                 Random.Range(-1f, 1f)) * 10f;

        GameObject smoke = GameObject.Instantiate(Resources.Load("Effects/SmokeTrail")) as GameObject;
        smoke.transform.parent = go.transform;
        smoke.transform.localPosition = Vector3.zero;

        rigidbody.AddExplosionForce(30000f, go.transform.position, 300f);
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
}