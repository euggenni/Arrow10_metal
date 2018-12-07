using UnityEngine;
using System.Collections;

public class GUIObject : MonoBehaviour {

    /// <summary>
    /// ��� ������
    /// </summary>
    public string Name { get; set; }

    public GameObject[] Elements;


	// Use this for initialization
	void Start () {
	
	}

    /// <summary>
    /// ���������� ������� ������ � ��������� ������
    /// </summary>
    /// <param name="controlName"></param>
    /// <returns></returns>
    public GameObject this[string controlName]
    {
        get
        {
            try
            {
                foreach (GameObject element in Elements)
                {
                    if (element.name.Equals(controlName))
                        return element;
                }
            }
            catch
            {
                //Debug.LogWarning("Problem while getting control " + controlName + " from " + gameObject.name);
            }

            return null;
        }
    }

    /// <summary>
    /// ���������� ������� ���������� ���� �� ���������� ���������
    /// </summary>
    public T GetControl<T>(string controlName) where T : Component
    {
        try
        {
            GameObject obj = this[controlName];

            Component c = obj.GetComponent<T>();

            return (T) c;
        }
        catch
        {
            //Debug.LogWarning("Error during getting component " + typeof(T) + " [" + controlName + "] from " + name);
        }

        return null;
    }
}
