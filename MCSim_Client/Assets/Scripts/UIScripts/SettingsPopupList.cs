using UnityEngine;
using System.Collections;

public class SettingsPopupList : MonoBehaviour
{
    public SourceListEnum sourceList;
    public GameObject StudentsPopupListPrefab;
}

public enum SourceListEnum
{
    Platoons,
    Students,
}
