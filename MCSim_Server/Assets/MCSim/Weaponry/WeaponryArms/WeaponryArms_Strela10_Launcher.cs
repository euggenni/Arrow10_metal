using System;
using System.Reflection;
using UnityEngine;
using System.Collections;
using MilitaryCombatSimulator;

public class WeaponryArms_Strela10_Launcher : WeaponryArms {

    /// <summary>
    /// ������ ����������
    /// </summary>
    public int Index;

    public Strela10_Arms Arms;

    #region �������

    private Hashtable _resources = new Hashtable();

    public override Hashtable Resources
    {
        get { return _resources; }
    }
    

    private void LoadResources()
    {
        _resources = new Hashtable();

        _resources.Add("Icon_ArmsList", "Icon_9M37_Container"); // ������ ��� ������ ����������
    }

    #endregion


    public override void OnWeaponryInstantiate()
    {
    }

    void Start()
    {
        ProjectileType = typeof (WeaponryRocket_9M37);
        LoadResources();
    }

    public override string Name
    {
        get { return "�� 9�37"; }
    }

    public override ArmsType ArmsClass
    {
        get { return ArmsType.RocketLauncher;
            //EnumDescription.GetCustomAttributes(typeof (ArmsType)).GetValue();
        }
    }

    public GameObject Rocket;
    private WeaponryProjectile _projectile;
    public override WeaponryProjectile Projectile
    {
        get { return _projectile; }
        set
        {
            if (value == null)
            {
                _projectile = null;
                return;
            }

            if (value is WeaponryRocket) {
                _projectile = value;
                Rocket = _projectile.gameObject;

                _projectile.transform.parent = transform;
                _projectile.transform.position = ProjectileResp.transform.position;
                _projectile.transform.rotation = ProjectileResp.transform.rotation;
            }
            else {
                Debug.LogError("[" + Name + "] �� ������������ ������� ���� [" + value.GetType().FullName + "]");
            }
        }
    }

    public override void ChargeProjectile<T>()
    {
        ChargeProjectile(typeof (T).FullName);
    }

    public override void ChargeProjectile(string projectileType)
    {
        // ���� ��� ���� ������ - �������
        if (Projectile) return;


        WeaponryProjectile projectile = (WeaponryProjectile)Assembly.GetExecutingAssembly().CreateInstance(projectileType);
        
        if (projectile is WeaponryRocket)
        {
            ProjectileType = projectile.GetType();

            // �� ������� ��������
            _projectile = MCSGlobalFactory.InstantiateWeaponry(projectileType).GetComponent<WeaponryRocket>();
            _projectile.transform.parent = transform;
            _projectile.transform.position = ProjectileResp.transform.position;
            _projectile.transform.rotation = ProjectileResp.transform.rotation;

            _projectile.gameObject.NetworkInstantiate();
            //(WeaponryRocket)Assembly.GetExecutingAssembly().CreateInstance(projectileType.FullName);
        }
        else
        {
            Debug.LogError("[" + Name + "] �� ������������ ������� ���� [" + projectileType + "]");
        }
    }

    public override void Shoot()
    {
        Arms.Shoot(Index);
    }

    public override void Reload()
    {
        if (Network.isServer)
        {
           ChargeProjectile(ProjectileType.FullName);
        }
        else
        {
            Arms.Reload(Index);
        }
    }



    public override WeaponryCategory Category
    {
        get { throw new System.NotImplementedException(); }
    }

    public override string PrefabPath
    {
        get { throw new System.NotImplementedException(); }
    }

    public override void Execute(MCSCommand cmd)
    {
        throw new System.NotImplementedException();
    }


    public override void Destroy()
    {
    }
}
