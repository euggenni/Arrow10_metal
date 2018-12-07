using UnityEngine;
using System.Collections;

public class AppRoot : MonoBehaviour
{
    private TransformObject mTransform; // TransformObject implements rotate / pan / zoom
    
    private GameObject mGOFlat; // GO rotate around
    private const string cGONameFlat = "-770136"; 
    RaycastHit hit;
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(hit.point, -ray.direction, Color.red, 2f);

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                print(hit.transform.name);
                var rotationComponent = hit.transform.gameObject.GetComponent<IsRotate>();

                if (rotationComponent != null)
                    IsRotate.CurrentRotationObject = rotationComponent;
            }
        }
    }
}
