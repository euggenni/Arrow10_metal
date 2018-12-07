using UnityEngine;
using System.Collections;

public enum UIAxe
{
    X, Y, XY
}

[ExecuteInEditMode]
public class UIPointAlign : MonoBehaviour
{

	public GameObject Point;
	public UIAxe Axe = UIAxe.XY;

	public bool Smooth = false;
	public float MoveSpeed = 5f;

	public float OffsetX = 0, OffsetY = 0;

	private Vector2 pos;
	private Transform _transform, _targetTransform;

	private void Awake()
	{
		_transform = transform;
		_targetTransform = Point.transform;
	}

	private void Update()
	{
		if (_targetTransform)
		{
			//if(AlignOnce)
			//   enabled = false;

			pos = _transform.InverseTransformPoint(_targetTransform.position);
			pos += new Vector2(OffsetX/_transform.localScale.x, OffsetY/_transform.localScale.y);
			pos = _transform.TransformPoint(pos);

			float x;
			float y;


			if (Smooth)
			{
				x = Mathf.Lerp(_transform.position.x, pos.x, MoveSpeed*Time.deltaTime);
				y = Mathf.Lerp(_transform.position.y, pos.y, MoveSpeed*Time.deltaTime);
			}
			else
			{
				x = pos.x;
				y = pos.y;
			}

			switch (Axe)
			{
				case UIAxe.X:
					_transform.position = new Vector3(x, _transform.position.y, _transform.position.z);
					break;

				case UIAxe.Y:
					_transform.position = new Vector3(_transform.position.x, y, _transform.position.z);
					break;

				case UIAxe.XY:
					_transform.position = new Vector3(x, y, _transform.position.z);
					break;

				default:
					break;
			}

			//_transform.localPosition += new Vector3(OffsetX, OffsetY, 0);
		}
	}
}
