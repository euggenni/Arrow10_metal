using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class CapToolkit : MonoBehaviour {

    enum State
    {
        Start, End
    }

    private bool rotatingNew = false;

    public Transform StartRotation;
    public Transform EndRotation;

    public float Seconds = 0.5f;

    private State current_position = State.Start;

	void Start () {
        if (StartRotation == null) return;

        transform.rotation = StartRotation.rotation;
	}

    void OnMouseDown()
    {
        if (current_position.Equals(State.Start))
        {
            StartCoroutine(RotateObject(EndRotation, Seconds));
            current_position = State.End;
        }
        else
        {
            StartCoroutine(RotateObject(StartRotation, Seconds));
            current_position = State.Start;
        }
    }



    IEnumerator RotateObject(Transform thisTransform, float seconds)
    {
        if (!rotatingNew)
        {
            rotatingNew = true;

            Quaternion startRotation = transform.rotation;
            Quaternion endRotation = thisTransform.rotation;

            float t = 0.0f;
            float rate = 1.0f / seconds;

            while (t < 1.0f)
            {                
                t += Time.deltaTime * rate;
                transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
                yield return 0;
            }

            rotatingNew = false;
        }
    }
}
