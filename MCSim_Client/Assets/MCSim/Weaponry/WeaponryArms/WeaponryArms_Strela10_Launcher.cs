using System;
using System.Collections;
using System.Reflection;
using MilitaryCombatSimulator;
using UnityEngine;

public class WeaponryArms_Strela10_Launcher : WeaponryArms {
    /// <summary>
    /// Индекс контейнера
    /// </summary>
    public int Index;

    public Strela10_Arms Arms;

    #region Ресурсы

    private Hashtable _resources = new Hashtable();

    public override Hashtable Resources {
        get { return _resources; }
    }


    private void LoadResources() {
        _resources = new Hashtable();

        _resources.Add("Icon_ArmsList", "Icon_9M37_Container"); // Иконка для списка вооружений
    }

    #endregion

    public override void OnWeaponryInstantiate() {
    }

    void Start() {
        ProjectileType = typeof(WeaponryRocket_9M37);
        LoadResources();
    }

    public override string Name {
        get { return "ПУ 9М37"; }
    }

    public override ArmsType ArmsClass {
        get {
            return ArmsType.RocketLauncher;
            //EnumDescription.GetCustomAttributes(typeof (ArmsType)).GetValue();
        }
    }

    public GameObject Rocket;
    private WeaponryProjectile _projectile;

    public override WeaponryProjectile Projectile {
        get { return _projectile; }
        set {
            if (value == null) {
                _projectile = null;
                return;
            }

            if (value is WeaponryRocket) {
                _projectile = value;
                Rocket = _projectile.gameObject;

                _projectile.transform.parent = transform;
                _projectile.transform.position = ProjectileResp.transform.position;
                _projectile.transform.rotation = ProjectileResp.transform.rotation;
            } else {
                Debug.LogError("[" + Name + "] не поддерживает снаряды типа [" + value.GetType().FullName + "]");
            }
        }
    }

    public override void ChargeProjectile<T>() {
        ChargeProjectile(typeof(T).FullName);
    }

    public override void ChargeProjectile(string projectileType) {
        // Если уже есть снаряд - выходим
        if (Projectile) return;

        WeaponryProjectile projectile =
            (WeaponryProjectile) Assembly.GetExecutingAssembly().CreateInstance(projectileType);

        if (projectile is WeaponryRocket) {
            ProjectileType = projectile.GetType();

            // Из фабрики вызывать
            _projectile = MCSGlobalFactory.InstantiateWeaponry(projectileType).GetComponent<WeaponryRocket>();
            _projectile.transform.parent = transform;
            _projectile.transform.position = ProjectileResp.transform.position;
            _projectile.transform.rotation = ProjectileResp.transform.rotation;

            Weaponry weaponry = _projectile.gameObject.NetworkInstantiate();
            //(WeaponryRocket)Assembly.GetExecutingAssembly().CreateInstance(projectileType.FullName);
        } else {
            Debug.LogError("[" + Name + "] не поддерживает снаряды типа [" + projectileType + "]");
        }
    }

    public override void Shoot() {
        Arms.Shoot(Index);
    }


    public override void Reload() {
        if (Network.isServer) {
            ChargeProjectile(ProjectileType.FullName);
        } else {
            Arms.Reload(Index);
        }
    }

    public override WeaponryCategory Category {
        get { return WeaponryCategory.Air; }
    }

    public override string PrefabPath {
        get { throw new NotImplementedException(); }
    }

    public override void Execute(MCSCommand cmd) {
        throw new NotImplementedException();
    }


    public override void Destroy() {
        throw new NotImplementedException();
    }
}