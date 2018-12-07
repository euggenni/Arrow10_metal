using UnityEngine;
using System.Collections;

public class UISizer : MonoBehaviour
{

    private Camera _camera;

    private Vector3 _startScale;
    private float _startOrtographicSize;

	void Start ()
	{
	    Transform go = this.gameObject.transform;
        
        while(_camera == null)
        {
            _camera = go.GetComponent<Camera>();

            go = go.parent;
            if (go == null) break;
        }

        if(_camera)
        {
            if(_camera.isOrthoGraphic)
            {
                _startScale = transform.localScale;
                _startOrtographicSize = _camera.orthographicSize;
            }
            else
            {
                this.enabled = false;
            }
        }
        else
        {
            Debug.Log("Камера не найдена среди родительский объектов, скрипт прекращает свою работу.");
            this.enabled = false;
        }

	}
	
	void Update () {
        if (_camera.enabled)
        {
            float dif = _camera.orthographicSize / _startOrtographicSize;
            transform.localScale = _startScale*dif; // Коэффициент от разрешения экрана
        }
	}
}
