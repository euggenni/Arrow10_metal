using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public static class MCSimCoreToolkitHelper {

    public static Transform SaveStateTransform(Transform transform, string name)
    {
        // �������� ����� ��� ������������ �������
        GameObject directory = GameObject.Find(transform.name + "(States)");

        // ���� ��� ��� - ������� �����
        if (directory == null)
        {
            directory = new GameObject();
            directory.transform.parent = transform;
            directory.name = transform.name + "(States)";
        }

        GameObject newTransform = new GameObject();
        newTransform.gameObject.name = name;
        newTransform.transform.parent = directory.transform;
        newTransform.transform.localRotation = transform.localRotation;
        //newTransform.transform.rotation = transform.rotation;
        return newTransform.transform;
    }

    public static void FillByNulls<T>(T[] list)
    {
        for (int i = 0; i < list.Length; i++)
            list[i] = default(T);
    }

    public static void FillByNulls<T>(List<T> list)
    {
        for (int i = 0; i < list.Capacity; i++)
            list.Add(default(T));
    }
}
