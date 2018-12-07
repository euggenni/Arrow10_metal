using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public static class Model
{
    private static Dictionary<string, UniqueCoroutine> _coroutines = new Dictionary<string, UniqueCoroutine>();

    public static void UCoroutine(MonoBehaviour behaviour, IEnumerator enumerator, string _name)
    {
        StopUCoroutine(_name);
        _coroutines.Add(_name, new UniqueCoroutine(behaviour, enumerator, _name));
    }

    public static bool StopUCoroutine(string _name)
    {
        if (_coroutines.ContainsKey(_name))
        {
            _coroutines[_name].stop = true;
            _coroutines.Remove(_name);
            return true;
        }
        return false; //failed to stop coroutine because it doesn't exist
    }
}
