using UnityEngine;

public class SmoothLookFrame : MonoBehaviour
{

    public Transform lookAtTarget;
    public Transform frameTarget;
    public float distance = 10.0f;
    public float height = 5.0f;
    public float damping = 2.0f;

    private Vector3 direction;
    private Vector3 wantedPosition;

    void FixedUpdate()
    {

        if (!lookAtTarget || !frameTarget)
            return;

        direction = (frameTarget.position - lookAtTarget.position);

        wantedPosition = frameTarget.position + (direction.normalized * distance);
        wantedPosition.y = wantedPosition.y + height;

        transform.position = Vector3.Lerp(transform.position, wantedPosition, damping * Time.deltaTime);

        transform.LookAt(lookAtTarget);

    }

}