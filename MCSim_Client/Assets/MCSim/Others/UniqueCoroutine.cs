using System.Collections;
using UnityEngine;

public class UniqueCoroutine : IEnumerator {
    public bool stop;
    public bool _moveNext;

    string _name;
    IEnumerator enumerator;
    MonoBehaviour behaviour;
    public readonly Coroutine coroutine;

    public UniqueCoroutine(MonoBehaviour behaviour, IEnumerator enumerator, string name) {
        this.behaviour = behaviour;
        this.enumerator = enumerator;
        this.stop = false;
        this._name = name;

        this.coroutine = this.behaviour.StartCoroutine(this);
    }

    public object Current {
        get { return enumerator.Current; }
    }

    public bool MoveNext() {
        _moveNext = enumerator.MoveNext();

        if (!_moveNext || stop) {
            Model.StopUCoroutine(_name);
            return false;
        }
        //return !Model.StopUCoroutine(_name);
        else
            return _moveNext;
    }

    public void Reset() {
        enumerator.Reset();
    }
}