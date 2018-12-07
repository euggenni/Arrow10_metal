using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public static class MCSProjectHelper
{

    //public static IEnumerable<Type> GetAllDerivedTypesOf(Type baseType)
    //{
    //    var types = Assembly.GetAssembly(baseType).GetTypes();
    //    return types.Where(t => t.IsSubclassOf(baseType));
    //}

    /// <summary>
    /// ���������� ��� ������, ������� ����������� �� �������� ������.
    /// </summary>
    /// <param name="baseType">������� �����.</param>
    public static IEnumerable<Type> GetAllDerivedTypesOf(Type baseType)
    {
        var types = Assembly.GetAssembly(baseType).GetTypes();

        return types.Where(baseType.IsAssignableFrom).Where(t => t != baseType);
    }

    /// <summary>
    /// ���������� �������� ����, ���� � ���� �������� �������� EnumDescription
    /// </summary>
    /// <typeparam name="T">��� ����</typeparam>
    /// <param name="enumValue">���� � ���������</param>
    public static string GetDescription<T>(T enumValue)
    {
        //BindingFlags.Public | BindingFlags.Static
        foreach (FieldInfo fieldInfo in typeof(T).GetFields().Where(fieldInfo => fieldInfo.Name.Equals(enumValue.ToString())))
        {
            //Debug.Log(">>" + fieldInfo.Name + " >> " + enumValue);
            //if (!fieldInfo.Name.Equals(enumValue.ToString())) continue;

            EnumDescription[] attrs = (EnumDescription[]) fieldInfo.GetCustomAttributes(typeof (EnumDescription), false);

            // ���� ������ ������� EnumDescription, ��������� ������ � ���������� ������, ����� ����������
            if (attrs.Length > 0) {
                return attrs[0].Text;
            }
        }
        return "NotDefined";
    }

    /// <summary>
    /// ��������� �� ������ ������ ���������
    /// </summary>
    /// <typeparam name="T">���������</typeparam>
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

    /// <summary>
    /// ���������� � ���� ���������� Weaponry
    /// </summary>
    /// <param name="weaponry">Weaponry ��� ����������</param>
    public static Weaponry NetworkInstantiate(this GameObject weaponry)
    {
        if(weaponry.GetComponent<Weaponry>())
        {
            return MCSGlobalSimulation.Instantiate(weaponry.gameObject);
        }
        return null;
    }

    /// <summary>
    /// ����� ���������� �������� ����� ���������
    /// </summary>
    /// <param name="includeThis">�������� �� ������ GameObject � �����</param>
    public static T GetComponentInParents<T>(this GameObject go, bool includeThis) where T:Component
    {
        if (go == null) return null;

        object comp = go.transform.GetComponent<T>(); 
        if(!includeThis)
            comp = go.transform.parent.GetComponent<T>();

        if (comp == null)
        {
            Transform t = go.transform.parent;

            while (t != null && comp == null)
            {
                comp = t.gameObject.GetComponent<T>();
                t = t.parent;
            }
        }

        return (T)comp;
    }

    /// <summary>
    /// ����� ���������� �������� ����� ���������
    /// </summary>
    /// <param name="includeThis">�������� �� ������ GameObject � �����</param>
    public static List<T> GetComponentsInParents<T>(this GameObject go, bool includeThis) where T : Component
    {
        List<T> list = new List<T>();

        if (go == null) return null;

        object comp = go.transform.GetComponent<T>();
        if (!includeThis)
            comp = go.transform.parent.GetComponent<T>();


        Transform t = go.transform.parent;

        while (t != null)
        {
            comp = t.gameObject.GetComponent<T>();
            
            if(comp != null)
                list.Add((T)comp);

            t = t.parent;
        }


        return list;
    }

    /// <summary>
    /// �������� ����� � ��������� ����� ��� ���� - GameObject
    /// </summary>
    /// <param name="go">����</param>
    /// <param name="type">��� �����</param>
    public static void InstantiateFlag(this GameObject go, MCSFlagType type)
    {
        //Weaponry weaponry = go.GetComponent<Weaponry>();

        switch (type)
        {
            case MCSFlagType.Ground:
                break;

            case MCSFlagType.Air:
                GameObject flag = GameObject.Instantiate(MCSUICenter.Store.Flag_WeaponryPlane) as GameObject;
                flag.GetComponent<MCSFlag>().SetTarget(go);
                break;
        }
    }

    /// <summary>
    /// �������� �� ����� ��������� ������
    /// </summary>
    /// <param name="go">������ ��� �������</param>
    /// <param name="explosion"></param>
    public static void FallAPart(this GameObject go, Vector3 explosion)
    {
        GameObject trash = new GameObject();
        trash.name = go.name + "(Trash)";
        foreach (MeshRenderer part in go.GetComponentsInChildren<MeshRenderer>())
        {
            int rand = UnityEngine.Random.Range(0, 2);
            if (rand == 1)
            {
                part.transform.parent = trash.transform;
                Rigidbody rigidbody = part.gameObject.AddComponent<Rigidbody>();
                rigidbody.mass = 100;
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
                rigidbody.velocity = go.rigidbody.velocity;

                GameObject smoke = GameObject.Instantiate(Resources.Load("Effects/SmokeTrail")) as GameObject;
                smoke.transform.parent = part.transform;
                smoke.transform.localPosition = Vector3.zero;


                rigidbody.AddExplosionForce(30000f, explosion, 300f);
                GameObject.Destroy(part, 20f);
            }
        }

        GameObject.Destroy(go, 20f);
    }


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

    ///// <summary>
    ///// ���������� ������ ����� ��������� ���������� ������
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
