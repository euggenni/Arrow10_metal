using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/// <summary>
/// Класс для управления камерой (переключения в тактический режим, переключения между целями и т.п.)
/// </summary>
[RequireComponent(typeof(Camera))]
public class MCSCameraController : MonoBehaviour {

    public enum CameraType
    {
        Free,
        Satellite
    }

    class CameraState
    {
        public Vector3 position;
        public Quaternion rotation;
        public float fieldOfView;
        public bool ortographic;

        public float nearClipPlane, farClipPlane;
    }

    Dictionary<CameraType,CameraState>  cameraStates = new Dictionary<CameraType, CameraState>();
    CameraType currentType = CameraType.Free;

	// Use this for initialization
	void Start () {
        CameraState state = new CameraState();
	    state.ortographic = false;
	    state.farClipPlane = 20000f;
	    state.nearClipPlane = 0f;
	    state.fieldOfView = 24f;
        state.position = new Vector3(0f, 2100f, 0f);
	    state.rotation = Quaternion.Euler(90, 0, 0);
        cameraStates.Add(CameraType.Satellite, state);

        state = new CameraState();
        state.ortographic = false;
        state.farClipPlane = 20000f;
        state.nearClipPlane = 0f;
        state.fieldOfView = 65f;
        state.position = new Vector3(0f, 150f, 0f);
        state.rotation = Quaternion.Euler(0, 0, 0);
        cameraStates.Add(CameraType.Free, state);
	}
	
    public void SwitchCamera(string cameraType)
    {
        CameraState state = default(CameraState);
        CameraType requiredType = CameraType.Free;

        switch (cameraType)
        {
            case "Free":
                requiredType = CameraType.Free;
                break;

            case "Satellite":
                requiredType = CameraType.Satellite;
                break;
        }

        try
        {
            // Если не сменили тип
            if(currentType.Equals(requiredType)) return;

            // Запоминаем настройки камеры в этом состоянии
            cameraStates[currentType].ortographic = camera.orthographic;
            cameraStates[currentType].farClipPlane = camera.farClipPlane;
            cameraStates[currentType].nearClipPlane = camera.nearClipPlane;
            cameraStates[currentType].fieldOfView = camera.fieldOfView;
            cameraStates[currentType].position = camera.transform.position;
            cameraStates[currentType].rotation = camera.transform.rotation;

            // Меняем тип камеры на необходимый
            currentType = requiredType;
            
            // Извлекаем настройки из массива застроек
            state = cameraStates[requiredType];

            // Применяем к камере необходимые настройки
            camera.orthographic = state.ortographic;
            camera.farClipPlane = state.farClipPlane;
            camera.nearClipPlane = state.nearClipPlane;
            camera.fieldOfView = state.fieldOfView;
            camera.transform.position = state.position;
            camera.transform.rotation = state.rotation;
        }
        catch {}
    }

	// Update is called once per frame
	void Update () {
	    
	}
}
