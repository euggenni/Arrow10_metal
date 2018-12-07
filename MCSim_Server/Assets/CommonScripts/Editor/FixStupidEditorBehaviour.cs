using UnityEditor;
using UnityEngine;


public class FixStupidEditorBehavior : MonoBehaviour
{
    [MenuItem("GameObject/Create Empty Child #&e")]
    private static void createEmptyParent()
    {
        GameObject go = new GameObject("GameObject");


        if (Selection.activeTransform != null)
        {
            go.transform.parent = Selection.activeTransform.parent;

            go.transform.Translate(Selection.activeTransform.position);


            Selection.activeTransform.parent = go.transform;
        }
    }


    [MenuItem("GameObject/Create Empty Duplicate #&d")]
    private static void createEmptyDuplicate()
    {
        GameObject go = new GameObject("GameObject");


        if (Selection.activeTransform != null)
        {
            go.transform.parent = Selection.activeTransform.parent;


            go.transform.Translate(Selection.activeTransform.position);
        }
    }


    [MenuItem("GameObject/Create Empty Child #&c")]
    private static void createEmptyChild()
    {
        GameObject go = new GameObject("GameObject");


        if (Selection.activeTransform != null)
        {
            go.transform.parent = Selection.activeTransform;


            go.transform.Translate(Selection.activeTransform.position);
        }
    }
}