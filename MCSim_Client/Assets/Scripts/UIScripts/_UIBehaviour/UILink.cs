using UnityEngine;
using System.Collections;

/// <summary>
/// Скрипт для привязки объекта по координатам к другому объекту без дочерне-родительских отношений
/// </summary>
public class UILink : UIBehaviourObject
{
	/// <summary>
	/// Цель, к которой прикреплен объект
	/// </summary>
	public GameObject Target;
	
	public override void OnResolutionChanged(float width, float height)
	{
		if (Target) transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, transform.position.z);
		else Debug.LogError("UILink [" + gameObject.name + "] have no Target object");
	}

	void FixedUpdate()
	{
		if (Target) transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, transform.position.z);
	}
}
