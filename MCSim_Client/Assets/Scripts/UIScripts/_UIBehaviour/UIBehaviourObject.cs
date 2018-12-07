using UnityEngine;
using System.Collections;

public abstract class UIBehaviourObject : MonoBehaviour
{
	void Awake()
	{
		OnResolutionChanged(UIBehaviour.Resolution.x, UIBehaviour.Resolution.y);
	}

	public abstract void OnResolutionChanged(float width, float height);
}
