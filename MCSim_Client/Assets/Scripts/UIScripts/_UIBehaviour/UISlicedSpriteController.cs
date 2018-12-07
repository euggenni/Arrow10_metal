using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UISlicedSprite))]
public class UISlicedSpriteController : UIBehaviourObject
{
	public bool HorizontalTransform = true, VerticalTransform = true;

	private UISlicedSprite _sprite;

	[SerializeField]
	protected float widthPercent, heightPercent;
		
	public UISlicedSprite Sprite
	{
		get { return _sprite ?? (_sprite = GetComponent<UISlicedSprite>()); }
	}
	
	/// <summary>
	/// Ширина окна в процентах
	/// </summary>
	public float WidthPercent
	{
		//get { return (float) Math.Round(100 * transform.localScale.x / ProjectHelper.GetMainGameViewSize().x); }
		get { return Mathf.RoundToInt(widthPercent); }
		set
		{
			Vector2 screen = ProjectHelper.GetMainGameViewSize();

			widthPercent = value;
			if (widthPercent > 100) widthPercent = 100;
			if (widthPercent < 5) widthPercent = 5;

			if(HorizontalTransform)
				UpdateUI(widthPercent * 0.01f * screen.x, transform.localScale.y);
		}
	}

	/// <summary>
	/// Высота окна в процентах
	/// </summary>
	public float HeightPercent
	{
		//get { return (float) Math.Round(100*transform.localScale.y/ProjectHelper.GetMainGameViewSize().y); }
		get { return Mathf.RoundToInt(heightPercent); }
		set
		{
			Vector2 screen = ProjectHelper.GetMainGameViewSize();

			heightPercent = value;
			if (heightPercent > 100) heightPercent = 100;
			if (heightPercent < 5) heightPercent = 5;

			if(VerticalTransform)
				UpdateUI(transform.localScale.x, heightPercent * 0.01f * screen.y);
		}
	} 

	/// <summary>
	/// Обновить элемент с учетом его ширины и высоты в процентах
	/// </summary>
	private void UpdateUI(float width, float height)
	{
		transform.localScale = new Vector3(width, height, 1);
	}

	public override void OnResolutionChanged(float width, float height)
	{
		float valueW = transform.localScale.x, valueH = transform.localScale.y;

		if(HorizontalTransform)
			valueW = widthPercent*0.01f*width;

		if (VerticalTransform)
			valueH = heightPercent*0.01f*height;

		UpdateUI(valueW, valueH);
	}
}
