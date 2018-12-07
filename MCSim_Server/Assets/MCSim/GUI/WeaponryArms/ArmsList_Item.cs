using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public class ArmsList_Item : MonoBehaviour {

    public UISprite WeaponryIcon;

    public UILabel WeaponryName;
    public UILabel WeaponryType;

    public UISprite ProjectileIcon;

    /// <summary>
    /// Оружие, на которое ссылается данный элемент списка
    /// </summary>
    public WeaponryArms Arms;

    public void SetArms(WeaponryArms arms)
    {
        UIAtlas atlas = WeaponryIcon.atlas;

        if (atlas)
        {
            if (arms.Resources.ContainsKey("Icon_ArmsList"))
            {
                WeaponryIcon.spriteName = arms.Resources["Icon_ArmsList"].ToString();
                WeaponryIcon.color = new Color(200, 200, 200);
                WeaponryIcon.UpdateUVs();
            }

            //try
            //{
                WeaponryProjectile projectile =
                    this.gameObject.AddComponent(arms.ProjectileType.FullName) as WeaponryProjectile;
                ProjectileIcon.spriteName = projectile.Resources["Icon_WeaponryList"].ToString();
                ProjectileIcon.color = new Color(200, 200, 200);
                ProjectileIcon.UpdateUVs();
            //}
            //catch{Debug.LogError("Не удалось загрузить изображение снаряда.");}

            //WeaponryName.text = "Name: " + weaponry.Name;
            WeaponryName.text = arms.Name;
            WeaponryType.text = "Type: " + MCSProjectHelper.GetDescription(arms.ArmsClass);
        }

        Arms = arms;
    }

	// Use this for initialization
	void Start () {
        Destroy(GetComponent<NGUIPanel>());
        transform.localScale = new Vector3(279.5f, 279.5f, 279.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
