using System;
using System.Collections;
using MilitaryCombatSimulator;
using UnityEngine;

public class WeaponryRocket_9M37 : WeaponryRocket {
    #region Ресурсы

    static Hashtable _resources = new Hashtable();

    //public GameObject falseTarget;
    public GameObject falseTarget;

    private float timer = 3.0f;
    public GameObject instance = null;
    private GameObject placeholder;
    public static bool pspState;

    public Cooler FirstCooler { get; private set; }
    public Cooler SecondCooler { get; private set; }

    public bool IsCoolingActive { get { return FirstCooler.IsActive || SecondCooler.IsActive; } }

    //инфракрасный режим

    public override Hashtable Resources {
        get { return _resources; }
    }

    private void LoadResources() {
        //_resources = new Hashtable();

        _resources["Icon_WeaponryList"] = "Icon_9M37_Rocket";
        //.Add("Icon_WeaponryList", "Icon_9M37_Rocket"); // Иконка для списка вооружений
    }

    #endregion

    public enum RocketState {
        Wait,
        Search,
        Chase,
        Destroy
    }

    // префаб взрыва
    public GameObject explosionPrefab;

    public GameObject _target;

    // скорость ракеты
    public float Speed = 10;

    // скорость поворота ракеты
    public float TurnSpeed = 100;

    // время до взрыва
    public float ExplosionTime = 25f;

    public float DestructionRadius = 20f;

    // Угла конуса обзора ракеты
    public float SearchAngle = 10f;

    // состояние ракеты
    public RocketState rocketState = RocketState.Wait;

    public ParticleRenderer Smoke;
    public ParticleRenderer Fire;

    public override void OnWeaponryInstantiate() {
        //Debug.Log("Размещен " + Name + " [" + ID + "]");

        // Если разместили везде - нужно запихнуть в установки на клиентах
        if (Network.isServer) {
            Weaponry weaponry = null;

            foreach (var w in this.gameObject.GetComponentsInParents<Weaponry>(false)) {
                if (w.ID != -1) {
                    weaponry = w;
                    break;
                }
            }

            int launcherIndex = -1;

            WeaponryArms_Strela10_Launcher launcher = transform.parent.GetComponent<WeaponryArms_Strela10_Launcher>();
            launcherIndex = launcher.Index;

            if (weaponry && launcherIndex != -1) {
                networkView.RPC("SetArms", RPCMode.Others, weaponry.ID, launcherIndex);
            }
        }
    }

    [RPC]
    void SetArms(int weaponryID, int launcherIndex) {
        WeaponryTank_Strela10 weaponry = MCSGlobalSimulation.Weapons.List[weaponryID] as WeaponryTank_Strela10;

        if (weaponry) {
            weaponry.Arms.Launchers[launcherIndex].Projectile = this;
        }
        float fiveMinSecs = 5 * 60;
        FirstCooler = new Cooler(weaponry, this, fiveMinSecs);
        SecondCooler = new Cooler(weaponry, this, fiveMinSecs);
    }

    // Use this for initialization
    void Awake() {
        LoadResources();
        placeholder = GameObject.Find("placeholder");
    }

    Vector3 direction;

    public void FixedUpdate() {
        // Если ракета в режиме ожидания - не производим поиск цели
        if (rocketState == RocketState.Wait) return;
        else FindTarget(); // Ищем цель 

        if (rocketState == RocketState.Chase && Network.isServer) {
            // уменьшаем таймер
            ExplosionTime -= Time.deltaTime;

            // если время таймера истекло, то взрываем ракету
            if (ExplosionTime <= 0) {
                DetachEmitters();
                Destroy();
                rocketState = RocketState.Destroy;
                return;
            }

            // величина движения вперед
            Vector3 movement = transform.forward * Speed * Time.deltaTime;
            Debug.Log("Sysinfo flagLaunch 2= ");
            // если цель найдена
            if (_target) {
//			if(flagLaunch){
//				if(timer<0&&!flagTarget){
//					//Vector3 direction = _target.transform.position - transform.position;
//					_target=null;
//					Target = null;
//					timer = 3.0f;
//					flagLaunch = false;
//					
//				}
//					else  direction = _target.transform.position - transform.position;
//					timer-=Time.deltaTime;
//				}


                // направление на цель


                // максимальный угол поворота в текущий кадр
                float maxAngle = TurnSpeed * Time.deltaTime;

                // угол между направлением на цель и направлением ракеты
                float angle = Vector3.Angle(transform.forward, direction);

                if (angle <= maxAngle) {
                    // угол меньше максимального, значит поворачиваем на цель
                    transform.forward = direction.normalized;
                } else {
                    //сферическая интерполяция направления с использованием максимального угла поворота
                    transform.forward = Vector3.Slerp(transform.forward, direction.normalized, maxAngle / angle);
                }

                // расстояние до цели
                float distanceToTarget = direction.magnitude;

                // если расстояние мало, то создаем взрыв
                // у обоих нет Rigitbody
                if (distanceToTarget < DestructionRadius) {
                    DetachEmitters();
                    Destroy();
                    return;
                }
            }


            // двигаем ракету вперед
            transform.position += movement;
        }
    }

    public override Double[] GetParameterForShootTarget() {
        Debug.LogError("вычисляем границы пуска");
        /* var distance = Vector3.Distance(GameObject.Find("placeholder").transform.position,
                     _target.transform.position);*/
        //var speeds=_target.rigidbody.velocity.sqrMagnitude/100f;
        if (_target.name.Contains("AH64")) {
            //В дальнейшем воткнуться условия вк ик 
            Double[] a = {5000, 1000};
            return a;
        }
        if (_target.name.Contains("F15")) {
            Double[] a = {8000, 2000};
            return a;
        }
        if (_target.name.Contains("UH60")) {
            Double[] a = {5000, 1000};
            return a;
        }
        if (_target.name.Contains("Missile")) {
            Double[] a = {2500, 1300};
            return a;
        }
        if (_target.name.Contains("F111")) {
            Double[] a = {7000, 2000};
            return a;
        }
        Double[] a2 = {5000, 800};
        return a2;
    }

    [RPC]
    public override void GetDistanceTargetWar(float upDistanceTarget, float downDistantanceTarget) {
        networkView.RPC("GetDistanceTargetWar", RPCMode.Server, upDistanceTarget, downDistantanceTarget);
    }

    IEnumerator LaunchRocket() {
        Smoke.enabled = true;
        Fire.enabled = true;

        transform.parent = null;

        if (Network.isServer) {
            Debug.Log("Ракета запущена на сервере");

            this.rocketState = RocketState.Chase;

            yield return new WaitForSeconds(1.5f);

            //rigidbody.isKinematic = false;
        }
    }

    public override GameObject Target {
        get { return _target; }
        set { _target = value; }
    }

    [RPC]
    public override void Destroy() {
        if (Network.isServer) {
            networkView.RPC("Destroy", RPCMode.Others);

            // Уничтожаем все Weaponry в указанном радиусе
            foreach (Collider hit in Physics.OverlapSphere(transform.position, DestructionRadius * 1.1f)) {
                Weaponry weaponry;
                if (weaponry = hit.transform.gameObject.GetComponentInParents<Weaponry>(true)) {
                    Debug.Log("Weaponry " + weaponry.Name);
                    if (weaponry == this) continue;
                    Debug.Log("Шлем инфу об уничтожении");
                    weaponry.Destroy();
                }
            }
        }

        DetachEmitters();

        if (explosionPrefab != null) {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }

        // уничтожаем ракету
        Destroy(gameObject);
    }

    [RPC]
    public override void Launch() {
        if (Network.isServer) {
            networkView.RPC("Launch", RPCMode.Others);
        }

        StartCoroutine(LaunchRocket());
    }

    public override void Execute(MCSCommand cmd) {
        throw new NotImplementedException();
    }

    public override string PrefabPath {
        get { return "WeaponryModel/WeaponryProjectile/9M37/Rocket9M37"; }
    }

    public override string Name {
        get { return "ЗУР 9М37"; }
    }

    public override WeaponryCategory Category {
        get { return WeaponryCategory.Air; }
    }

    public void DetachEmitters() {
        foreach (ParticleAnimator script in GetComponentsInChildren<ParticleAnimator>()) {
            script.transform.parent = null;
            script.particleEmitter.emit = false;
            script.autodestruct = true;
        }
    }


    public override GameObject FindTarget() {
        GameObject targetToCatch = null;

        foreach (Weaponry weaponry in MCSGlobalSimulation.Weapons.List.Values) {
            if (weaponry != null && weaponry is WeaponryPlane && weaponry.gameObject.activeInHierarchy) {
                if (Vector3.Angle(transform.forward, (weaponry.gameObject.transform.position - transform.position)) <=
                    SearchAngle) {
                    RaycastHit hit;
                    if (!Physics.Linecast(transform.position, weaponry.transform.position, out hit)) {
                        // Debug.Log("Дальность " + Vector3.Distance(placeholder.transform.position, weaponry.transform.position));
                        if (Vector3.Distance(placeholder.transform.position, weaponry.transform.position) <
                            weaponry.GetComponent<DistaceMark>().distance) {
                            TurnSpeed = 100f;
                            targetToCatch = weaponry.gameObject;
                            if (targetToCatch.name.Contains("LieTargetOut") && pspState) {
                                continue;
                            }
                            if (targetToCatch.name.Contains("Cloud")) {
                                continue;
                            }

                            if (!targetToCatch.name.Contains("LieTargetOut") && !pspState) {
                                continue;
                            }
                            return Target = weaponry.gameObject;
                        } else {
                            TurnSpeed = 10f;
                            targetToCatch = weaponry.gameObject;
                            if (targetToCatch.name.Contains("LieTargetOut") && pspState) {
                                continue;
                            }
                            if (targetToCatch.name.Contains("Cloud")) {
                                continue;
                            }
                            if (!targetToCatch.name.Contains("LieTargetOut") && !pspState) {
                                continue;
                            }
                            return Target = weaponry.gameObject;
                        }
                    }
                }
            }
        }

        return Target = targetToCatch;
    }
}