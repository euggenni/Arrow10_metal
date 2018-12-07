using System;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Collections;

public class WeaponryRocket_9M37 : WeaponryRocket
{

    #region Ресурсы

    static Hashtable _resources = new Hashtable();

    public override Hashtable Resources
    {
        get { return _resources; }
    }

    private void LoadResources()
    {
        //_resources = new Hashtable();

        _resources["Icon_WeaponryList"] = "Icon_9M37_Rocket";
        //.Add("Icon_WeaponryList", "Icon_9M37_Rocket"); // Иконка для списка вооружений
    }

    #endregion

    public enum RocketState
    {
        Wait,
        Move,
        Destroy
    }

    // префаб взрыва
    public GameObject explosionPrefab;

    private GameObject _target;
    // скорость ракеты
    public float Speed = 10;
    // скорость поворота ракеты
    public float TurnSpeed = 100;
    // время до взрыва
    public float ExplosionTime = 25f;

    public float DestructionRadius = 20f;

    //Ближняя  граница поражения
    public float UpDistanceTargetWar=-1f;
    
    // Дальняя граница поражения
    public float DownDistanceTargetWar=-1f;

    // состояние ракеты
    public RocketState rocketState = RocketState.Wait;

    public ParticleRenderer Smoke;
    public ParticleRenderer Fire;

    public override void OnWeaponryInstantiate()
    {
        //Debug.Log("Размещен " + Name + " [" + ID + "]");

        // Если разместили везде - нужно запихнуть в установки на клиентах
        if (Network.isServer)
        {
            Weaponry weaponry = null;

            foreach (var w in this.gameObject.GetComponentsInParents<Weaponry>(false))
            {
                if (w.ID != -1)
                {
                    weaponry = w;
                    break;
                }
            }

            int launcherIndex = -1;

            WeaponryArms_Strela10_Launcher launcher = transform.parent.GetComponent<WeaponryArms_Strela10_Launcher>();
            launcherIndex = launcher.Index;

            if (weaponry && launcherIndex != -1)
            {
                networkView.RPC("SetArms", RPCMode.Others, weaponry.ID, launcherIndex);
            }
        }
    }

    [RPC]
    void SetArms(int weaponryID, int launcherIndex)
    {
        WeaponryTank_Strela10 weaponry = MCSGlobalSimulation.Weapons.List[weaponryID] as WeaponryTank_Strela10;

        if (weaponry)
        {
            weaponry.Arms.Launchers[launcherIndex].Projectile = this;
        }
    }

    // Use this for initialization
    void Awake()
    {
        LoadResources();
    }
    public void ChangeTurnSpeed()
    {
        var distance = Vector3.Distance(GameObject.Find("Placeholder").transform.position,
                    _target.transform.position);
        if(DownDistanceTargetWar<0)
        {
            DownDistanceTargetWar = 6000;
        }
        if(UpDistanceTargetWar<0)
        {
            UpDistanceTargetWar = 300;
        }
        if (distance > DownDistanceTargetWar || distance < UpDistanceTargetWar)
        {
            TurnSpeed = 0f;
        }
    }

    public void FixedUpdate() {
        if ((this.rocketState == RocketState.Move) && Network.isServer) {
            this.ExplosionTime -= Time.deltaTime;
            if (this.ExplosionTime <= 0f) {
                this.DetachEmitters();
                this.Destroy();
                this.rocketState = RocketState.Destroy;
            } else {
                Vector3 vector = (Vector3)((base.transform.forward * this.Speed) * Time.deltaTime);
                if (this._target != null) {
                    Vector3 to = this._target.transform.position - base.transform.position;
                    float num = this.TurnSpeed * Time.deltaTime;
                    float num2 = Vector3.Angle(base.transform.forward, to);
                    if (num2 <= num) {
                        base.transform.forward = to.normalized;
                    } else {
                        base.transform.forward = Vector3.Slerp(base.transform.forward, to.normalized, num / num2);
                    }
                    if (to.magnitude < this.DestructionRadius) {
                        this.DetachEmitters();
                        this.Destroy();
                        return;
                    }
                }
                Transform transform = base.transform;
                transform.position += vector;
            }
        }
    }

    IEnumerator LaunchRocket()
    {
        Smoke.enabled = true;
        Fire.enabled = true;

        if (Network.isServer)
        {
            yield return new WaitForSeconds(1.5f);

            transform.parent = null; // Отсоединяем ракету от контейнера
            this.rocketState = RocketState.Move;

            //StartCoroutine(AttachCollider());

            // Прикрепляем коллайдер
            yield return new WaitForSeconds(1f);
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;
        }
    }

    /// <summary>
    /// Прикрепление коллайдера
    /// </summary>
    /// <returns></returns>
    IEnumerator AttachCollider()
    {
        yield return new WaitForSeconds(1f);

        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
    }

    void OnTriggerStay()
    {
        Destroy();
    }

    public override GameObject Target
    {
        get { return _target; }
        set { _target = value; }
    }
     [RPC]
    public override  void GetDistanceTargetWar(float upDistanceTarget,float downDistantanceTarget)
     {
         UpDistanceTargetWar = upDistanceTarget;
         DownDistanceTargetWar = downDistantanceTarget;
     }

    [RPC]
    public override void Destroy()
    {
        if (Network.isServer)
        {
            networkView.RPC("Destroy", RPCMode.Others);

            // Уничтожаем все Weaponry в указанном радиусе
            foreach (Collider hit in Physics.OverlapSphere(transform.position, DestructionRadius * 1.1f))
            {
                Weaponry weaponry;
                if (weaponry = hit.transform.gameObject.GetComponentInParents<Weaponry>(true))
                {
                    if (weaponry == this) continue;
                    weaponry.Destroy();
                }
            }
        }

        DetachEmitters();

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }

        // уничтожаем ракету
        Destroy(gameObject);
    }
    public override void LaunchWithNotLie()
    {
        networkView.RPC("Launch", RPCMode.Others);
        foreach (GameObject plane in GameObject.FindGameObjectsWithTag("Weaponry"))
        {
            if (!plane.GetComponent<WeaponryPlane>()) continue;
            if (plane.name.Contains("LieTarget")) continue;
            if (Vector3.Angle(transform.forward, (plane.transform.position - transform.position)) <= 15)
            {
                Target = plane;
            }
        }
        StartCoroutine(LaunchRocket());
    }

    [RPC]
    public override void Launch() {
        if (Network.isServer) {
            base.networkView.RPC("Launch", RPCMode.Others, new object[0]);
            foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("Weaponry")) {
                if ((obj2.GetComponent<WeaponryPlane>() != null) && (Vector3.Angle(base.transform.forward, obj2.transform.position - base.transform.position) <= 15f)) {
                    this.Target = obj2;
                    break;
                }
            }
        }
        base.StartCoroutine(this.LaunchRocket());
    }

    public override void Execute(MCSCommand cmd)
    {
        throw new System.NotImplementedException();
    }

    public override string PrefabPath
    {
        get { return "WeaponryModel/WeaponryProjectile/9M37/Rocket9M37"; }
    }

    public override string Name
    {
        get { return "ЗУР 9М37"; }
    }

    public override WeaponryCategory Category
    {
        get { return WeaponryCategory.Air; }
    }

    public void DetachEmitters()
    {
        foreach (ParticleAnimator script in GetComponentsInChildren<ParticleAnimator>())
        {
            script.transform.parent = null;
            script.particleEmitter.emit = false;
            script.autodestruct = true;
        }
    }
}
