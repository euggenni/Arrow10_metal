using System;
using System.Collections;
using MilitaryCombatSimulator;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Strela10_Operator_VizirController : MonoBehaviour {
    public Strela10_Operator_Node OperatorNode;
    private Strela10_Operator_CrosshairController _crosshairController;

    [SerializeField]
    public WeaponryTank_Strela10 Strela10;

    /// <summary>
    /// Окно прицеливания
    /// </summary>
    public GameObject OperatorCrosshair;

    public bool aoz;
    public bool bort;
    public bool nrz;
    public bool VizirActivated;
    private GameObject gameObjectCrosshair;
    private Camera gameObjectVizir;
    private GameObject cameraGO;
    public bool bortFlag;
    private WeaponryRocket currentRocket;
    private Vector3 lastTrackerPos;
    public float a = 5f, b = 5f;
    public int countKeyPressT;
    public bool IsRocketShot;

    private bool stateOne = false;
    private WeaponryRocket rocket;

    private double launchZoneTimer;
    private bool isLaunchZoneTimerStarted;

    enum Zoom {
        Close = 25,
        Far = 25
    }

    private Zoom zoom = Zoom.Close;
    private float speed = 1f;
    private bool keyStateB = false;
    private float _currentAngle;
    private bool firstPress = false;
    private Vector3? prevTrackerPosition;
    private bool isLaunchPressEmitted;

    public AudioSource audio1;
    public AudioSource audio2;
    public AudioSource audio3;

    public void activateOfVizir() {
        if (VizirActivated) return;

        GameObject containersFrame = OperatorNode.Host.Containers;
        OperatorNode.Camera.enabled = false;

        // Создаем ГУИ с прицелом
        gameObjectCrosshair = Instantiate(OperatorCrosshair) as GameObject;
        gameObjectCrosshair.transform.parent = UICenter.Panels.Anchor.transform;
        gameObjectCrosshair.transform.localScale = Vector3.one;

        // Получаем с него контроллер прицела для управления стрелками, кольцом захвата и т.п.
        _crosshairController = gameObjectCrosshair.GetComponent<Strela10_Operator_CrosshairController>();
        _crosshairController.Tracker.transform.localPosition = new Vector3(-190f, 100f);
        _crosshairController.Indicator.GetComponent<UISprite>().color =
            aoz ? new Color(255, 0, 0) : new Color(89, 89, 89);
        _crosshairController.Zoom.color = Color.red;
        // Создаем камеру визира
        cameraGO = new GameObject();
        cameraGO.name = "VizorCamera";
        gameObjectVizir = cameraGO.AddComponent<Camera>();
        gameObjectVizir.transform.parent = OperatorNode.Host.Tower.transform;
        gameObjectVizir.nearClipPlane = 0.1f;
        gameObjectVizir.farClipPlane = 10000f;
        gameObjectVizir.transform.localPosition = new Vector3(0.3f, -0.4f, 0.15f);
        gameObjectVizir.transform.localEulerAngles = Vector3.zero;
        gameObjectVizir.fieldOfView = (int) Zoom.Far;
        //switchIndicatorOff();
        updateLaunchZoneIndicator();
        // Активируем скрипт слежения визира за точкой фокуса
        Strela10_Operator_VizirCameraController cameraController =
            gameObjectVizir.gameObject.AddComponent<Strela10_Operator_VizirCameraController>();
        cameraController.Containers = OperatorNode.Host.Containers;
        VizirActivated = true;
    }
	
	// Старый метод с неверной логикой. Раньше стрелки загорались только при появлении цели в опт. визире
    void setPointTarget(WeaponryRocket rocket) {
		_crosshairController.LeftArrow.color = Color.grey;
		_crosshairController.RightArrow.color = Color.grey;
		
        if (rocket != null && rocket.Target) {
            if (_currentAngle >= 0 && _currentAngle <= 180) {
				Debug.Log("ANGLE >=0 and <=180 : " + _currentAngle);
                _crosshairController.RightArrow.color = Color.green;
                _crosshairController.LeftArrow.color = Color.grey;
            } else {
                if (_currentAngle > 180 && _currentAngle <= 360) {
					Debug.Log("ANGLE >180 and <=360 : " + _currentAngle);
                    _crosshairController.LeftArrow.color = Color.green;
                    _crosshairController.RightArrow.color = Color.grey;
                }
            }
        } else {
			Debug.Log("ANGLE :((((((");
			_crosshairController.LeftArrow.color = Color.grey;
			_crosshairController.RightArrow.color = Color.grey;
        }
    }
	
	void updateLeftAndRightArrow(){
		_crosshairController.LeftArrow.color = Color.grey;
		_crosshairController.RightArrow.color = Color.grey;
		
		float angl = Strela10_Operator_CoreHandler._currentAngle;
        if ((angl >= 2) && (angl <= 180)) {
            _crosshairController.RightArrow.color = Color.green;
            _crosshairController.LeftArrow.color = Color.grey;
        } else{
			if ((angl > 180) && (angl <= 358)) {
            	_crosshairController.LeftArrow.color = Color.green;
            	_crosshairController.RightArrow.color = Color.grey;
			}
        }
	}

    void updateCoord() {
        if (_crosshairController == null) {
            return;
        }
        _crosshairController.LeftArrow.enabled = true;
        _crosshairController.RightArrow.enabled = true;
        if (OperatorNode.Host.TowerHandler.TowerAngle > 0 && OperatorNode.Host.TowerHandler.TowerAngle < 9 ||
            OperatorNode.Host.TowerHandler.TowerAngle < 0) {
            _crosshairController.VizorLight1.color = Color.green;
            _crosshairController.VizorLight2.color = Color.grey;
            _crosshairController.VizorLight3.color = Color.grey;
            _crosshairController.VizorLight4.color = Color.grey;
            _crosshairController.VizorLight5.color = Color.grey;
            _crosshairController.DownArrow.enabled = true;
        } else {
            if (OperatorNode.Host.TowerHandler.TowerAngle > 9 && OperatorNode.Host.TowerHandler.TowerAngle < 27) {
                _crosshairController.VizorLight1.color = Color.grey;
                _crosshairController.VizorLight2.color = Color.red;
                _crosshairController.VizorLight3.color = Color.grey;
                _crosshairController.VizorLight4.color = Color.grey;
                _crosshairController.VizorLight5.color = Color.grey;

                if (OperatorNode.Host.TowerHandler.TowerAngle < 20)
                    _crosshairController.DownArrow.enabled = true;
                else
                    _crosshairController.DownArrow.enabled = false;
            } else {
                if (OperatorNode.Host.TowerHandler.TowerAngle >= 27 &&
                    OperatorNode.Host.TowerHandler.TowerAngle <= 50) {
                    _crosshairController.VizorLight1.color = Color.grey;
                    _crosshairController.VizorLight2.color = Color.grey;
                    _crosshairController.VizorLight3.color = Color.green;
                    _crosshairController.VizorLight4.color = Color.grey;
                    _crosshairController.VizorLight5.color = Color.grey;
                    _crosshairController.DownArrow.enabled = false;
                } else {
                    if (OperatorNode.Host.TowerHandler.TowerAngle > 50 &&
                        OperatorNode.Host.TowerHandler.TowerAngle <= 70) {
                        _crosshairController.VizorLight1.color = Color.grey;
                        _crosshairController.VizorLight2.color = Color.grey;
                        _crosshairController.VizorLight3.color = Color.grey;
                        _crosshairController.VizorLight4.color = Color.red;
                        _crosshairController.VizorLight5.color = Color.grey;
                        _crosshairController.DownArrow.enabled = false;
                        stateOne = false;
                    } else {
                        if (OperatorNode.Host.TowerHandler.TowerAngle > 70) {
                            _crosshairController.VizorLight1.color = Color.grey;
                            _crosshairController.VizorLight2.color = Color.grey;
                            _crosshairController.VizorLight3.color = Color.grey;
                            _crosshairController.VizorLight4.color = Color.grey;
                            _crosshairController.VizorLight5.color = Color.green;
                            _crosshairController.DownArrow.enabled = false;
                            if (!stateOne) {
                                Strela10_Operator_CoreHandler.CoreStatic.GetPanel("Strela10_VizorPanel").GetControl(
                                        ControlType.Tumbler, "TUMBLER_LOSE").GetComponent<SwitcherToolkit>()
                                    .ControlChanged();
                                stateOne = true;
                            }
                            _crosshairController.DownArrow.color = Color.green;
                        }
                    }
                }
            }
        }
    }

    void OnMouseDown() {
        if (Strela10_Operator_CoreHandler.CoreStatic.GetPanel("Strela10_OperatorPanel")
            .GetControl(ControlType.Indicator, "VOLTMETER_BACKLIGHT").State.Equals("ON"))
            activateOfVizir();
    }

    void Start() {
        if (Strela10) {
            var towerHandler = Strela10.TowerHandler;
            towerHandler.OnTargetAngleChanged += TowerHandler_OnTargetAngleChanged;
        }
    }

    void TowerHandler_OnTargetAngleChanged(float angle) {
        _currentAngle = angle;
    }

    public void destroyOfVizir() {
        OperatorNode.Camera.enabled = true;
        Destroy(cameraGO);
        Destroy(gameObjectCrosshair);
        VizirActivated = false;
    }


	bool firstCooler = false;
	bool secondCooler = false;
	float time = 12f;
    void Update() {
		//Тестируем ночной режим
		if (currentRocket != null)
		{
		    if (currentRocket.GetType() == typeof(WeaponryRocket_9M37))
			{
				Debug.Log("ROCKET IS 9M37!!!!");

				WeaponryRocket_9M37 rocket_9M37 = (WeaponryRocket_9M37) currentRocket;
				firstCooler = rocket_9M37.FirstCooler.IsActive;
				secondCooler = rocket_9M37.SecondCooler.IsActive;
				if (MCSSimulationHandler.UniSky != null)
				{
					time = MCSSimulationHandler.UniSky.GetTime();
				}
				Debug.Log("Time is: " + time);
			}
			else
			{
				Debug.Log("ALIEN ROCKET :((((");
			}
		}

		// Новый метод: обновляем данные для LEFT_ARROW и RIGHT_ARROW
		updateLeftAndRightArrow();
			
        // Если есть активная ракета
        rocket = OperatorNode.Host.Arms.Projectile as WeaponryRocket;
		
        stateTracker();
        // Debug.LogWarning(_currentAngle);
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) ||
             Input.GetKeyDown(KeyCode.UpArrow))) {
            if (keyStateB) {
                speed = 10F;
            } else {
                speed = 1F;
            }
        }
        if (Input.GetKeyDown(KeyCode.Z) && VizirActivated) {
            if (zoom == Zoom.Close) {
                zoom = Zoom.Far;
                gameObjectVizir.fieldOfView = (float) zoom;
                _crosshairController.Zoom.color = Color.red;
            } else if (zoom == Zoom.Far) {
                zoom = Zoom.Close;
                gameObjectVizir.fieldOfView = (float) zoom;
                _crosshairController.Zoom.color = Color.white;
            }
        }

        updateCoord();
        if (Input.GetKeyDown(KeyCode.Escape) && VizirActivated) {
            destroyOfVizir();
        }

        if (Input.GetKeyUp(KeyCode.Tab)) {
            if (!VizirActivated)
                activateOfVizir();
            else
                destroyOfVizir();
        }

        if (rocket == null && _crosshairController != null && _crosshairController.Tracker) {
            if (!bort) {
                //bortFlag = false;
                //stateZeroBort();
                keyStateB = false;
                prevTrackerPosition = null;
                //Debug.Log("BORT IS OFF");
            }
            _crosshairController.Tracker.enabled = false;
            StopBortSound();
        }
        if (rocket && !rocket.Equals(currentRocket)) {
            if (!bort) {
                //bortFlag = false;
                //stateZeroBort();
                keyStateB = false;
                prevTrackerPosition = null;
                //Debug.Log("BORT IS OFF");
            }
            if (_crosshairController != null) {
                _crosshairController.Tracker.transform.localPosition = new Vector3(-190f, 100f);
                currentRocket = rocket;
            }
        }

        if (IsRocketShot) {
            resetLaunchZoneTimer();
            OperatorNode.Host.IsTargetInsideLaunchZone = false;
            IsRocketShot = false;
            if (rocket && rocket.Target && CheckNRZTargetBelonging()) {

				if (!(time > 5 && time < 19) && firstCooler == false && secondCooler == false)
				{
					Debug.Log("BREAK, BECAUSE NIGHT!!!");
					return;
				}

                _crosshairController.Tracker.transform.localPosition = new Vector3(-190f, 100f);
                //StartCoroutine(TrakerAfterFire());
                keyStateB = true;
                //switchIndicatorOff();
                updateLaunchZoneIndicator();
                StopBortSound();
            }
        }
        if (rocket && rocket.Target) {
            if (!bort) {
                //bortFlag = false;
                //stateZeroBort();
                keyStateB = false;
                prevTrackerPosition = null;
                //Debug.Log("BORT IS OFF");
            } else if (rocket && (Input.GetKeyDown(KeyCode.B) || keyStateB || bort)) {
                bort = true;
                bortFlag = true;
                stateZeroBort();
                //Debug.Log("Нет цели у ракеты");
                keyStateB = true;
                PlayBortSound();
                updateLaunchZoneIndicator();
                //switchIndicatorOff();
            }
            if (keyStateB) {
                Strela10_Operator_CoreHandler.ConvertorButton("ON");
            }
            if (keyStateB) {
                if (ButtonKeyStatePress()) {
					if (!(time > 5 && time < 19) && firstCooler == false && secondCooler == false)
					{
						Debug.Log("BREAK, BECAUSE NIGHT 1!!!");
						return;
					}
                    if (CheckNRZTargetBelonging()) {
						// СЛЕЖЕНИЕ РУЧНОЕ-АВТОМАТ
                        var coords = gameObjectVizir.WorldToScreenPoint(rocket.Target.transform.position);
                        coords = UICenter.UICamera.ScreenToWorldPoint(coords);
                        lastTrackerPos = new Vector3(coords.x, coords.y, 0);
                        Debug.LogError("lastTrackerPos: x = " + coords.x + "; y = " + coords.y);

                      	if (isAutoTrackingMode()){
							_crosshairController.Tracker.transform.position = lastTrackerPos; //Слежение следящей марки
						} else{
                            if ((coords.x * coords.x + coords.y * coords.y) > 0.12 * 0.12) return;	
						}
                        //switchIndicatorOn();
                        //setPointTarget(rocket);
                        updateLaunchZoneIndicator();
                    } else {
                        //Debug.LogError("Play NRZ");
                        PlayNRZSound();
                    }
                }
            } else {
                if (Input.GetKeyDown(KeyCode.B)) {
                    bort = true;
                    bortFlag = true;
                    var pos = new Vector3(b * (Mathf.Sin(Time.time) - 1) + b,
                        a * (0 - Mathf.Sin(Time.time)) * Mathf.Cos(Time.time), 0);

					if (!(time > 5 && time < 19) && firstCooler == false && secondCooler == false)
					{
						Debug.Log("BREAK, BECAUSE NIGHT 2!!!");
						return;
					}

                    try {
                        _crosshairController.Tracker.transform.localPosition = Vector3.Lerp(
                            _crosshairController.Tracker.transform.localPosition, pos, speed * (Time.deltaTime));
                    } catch {
                    }
                    keyStateB = true;
                    //switchIndicatorOff();
                    updateLaunchZoneIndicator();
                }
            }


            //Vector3 TargetOnScreen = gameObjectVizir.WorldToScreenPoint(rocket.Target.transform.position);
            //_crosshairController.Tracker.transform.localPosition = new Vector3(TargetOnScreen.x - Screen.width/2, TargetOnScreen.y - Screen.height/2, 100);
        } else {
            //switchIndicatorOff();
            updateLaunchZoneIndicator();

            if (!bort) {
                //bortFlag = false;
                //stateZeroBort();
                keyStateB = false;
                prevTrackerPosition = null;
                //Debug.Log("BORT IS OFF");
            } else if (rocket && (Input.GetKeyDown(KeyCode.B) || keyStateB || bort)) {
                bort = true;
                bortFlag = true;
                stateZeroBort();
                //Debug.Log("Нет цели у ракеты");
                keyStateB = true;
                PlayBortSound();
                //switchIndicatorOff();
                updateLaunchZoneIndicator();
            }
            if (keyStateB) {
                Strela10_Operator_CoreHandler.ConvertorButton("ON");
            }
        }

        //_crosshairController.Tracker.transform.position = lastTrackerPos;
    }

    private bool IsLaunchPressed() {
        bool isLauchPressed = 
            OperatorNode.Core.GetPanel("Strela10_GuidancePanel")
                .GetControl(ControlType.Tumbler, "TUMBLER_TRACK_LAUNCH").State.Equals("LAUNCH")
            || Input.GetKeyDown(KeyCode.Space);

        if (isLauchPressed && !isLaunchPressEmitted) {
            isLaunchPressEmitted = true;
            return true;
        }

        if (!isLauchPressed) {
            isLaunchPressEmitted = false;
        }
        return false;
    }

    IEnumerator TrakerAfterFire() {
        if (rocket && rocket.Target) {
            var pos = gameObjectVizir.WorldToScreenPoint(rocket.Target.transform.position);
            _crosshairController.Tracker.transform.localPosition = new Vector3(pos.x, pos.y, 0);

            //Debug.LogWarning("Tracker jumped");
        }
        yield return new WaitForSeconds(10f);
        _crosshairController.Tracker.transform.localPosition = new Vector3(-190f, 100f);
    }

    #region Launch Zone

    void updateLaunchZoneIndicator() {
        Debug.LogError("-----> updateLaunchZoneIndicator");
        if (_crosshairController == null) {
            Debug.LogError("-----> _crosshairController is null!");
            return;
        }
        if (((aoz && !_crosshairController.DownArrow.enabled) || (!aoz && _crosshairController.DownArrow.enabled)) && isTargetInsideLaunchZone() && isTrackingPressed() && keyStateB && rocket != null && rocket.Target != null) {
			//СЛЕЖЕНИЕ РУЧНОЕ-АВТОМАТ: ТОЛЬКО ПРИ РУЧНОМ СЛЕЖЕНИИ
			if (!(time > 5 && time < 19) && firstCooler == false && secondCooler == false)
			{
				Debug.Log("BREAK, BECAUSE NIGHT 1!!!");
				return;
			}

			if (!isAutoTrackingMode() && gameObjectVizir != null){
	            var coords = gameObjectVizir.WorldToScreenPoint(rocket.Target.transform.position);
				if (coords != null){
                    Debug.LogError("-----> coords != null!");
		            coords = UICenter.UICamera.ScreenToWorldPoint(coords);
                    lastTrackerPos = new Vector3(coords.x, coords.y, 0);
					Debug.LogError("lastTrackerPos: x = " + coords.x + "; y = " + coords.y);
                    if ((coords.x * coords.x + coords.y * coords.y) > 0.12*0.12) {
						resetLaunchZoneTimer();
			            _crosshairController.Indicator.GetComponent<UISprite>().color = new Color(89, 89, 89);
			            OperatorNode.Host.IsTargetInsideLaunchZone = false;
					}
                } else Debug.LogError("-----> coords == null inside updateLaunchZoneIndicztor!!!!!!!!!!!!!!!!!!!!!");
            } else Debug.LogError("----->gameObjectVizir == null inside updateLaunchZoneIndicztor!!!!!!!!!!!!!!!!!!!!!");
			
            if (!isLaunchZoneTimerStarted) {
                var targetDistance = getTargetDistance();
                var waitSeconds = targetDistance / 1000;
                startLaunchZoneTimer(waitSeconds);
            } else {
                countdownLaunchZoneTimer();
            }
            if (isLaunchZoneTimerOver()) {
                _crosshairController.Indicator.GetComponent<UISprite>().color = new Color(255, 0, 0);
                OperatorNode.Host.IsTargetInsideLaunchZone = true;
            }
        } else {
            resetLaunchZoneTimer();
            _crosshairController.Indicator.GetComponent<UISprite>().color = new Color(89, 89, 89);
            OperatorNode.Host.IsTargetInsideLaunchZone = false;
        }
    }

    public void resetLaunchZoneTimer() {
        launchZoneTimer = 0;
        isLaunchZoneTimerStarted = false;
    }

    private void startLaunchZoneTimer(double timeSeconds) {
        launchZoneTimer = timeSeconds;
        isLaunchZoneTimerStarted = true;
    }

    private bool isLaunchZoneTimerOver() {
        return isLaunchZoneTimerStarted == true && launchZoneTimer <= 0;
    }

    private void countdownLaunchZoneTimer() {
        if (isLaunchZoneTimerStarted && launchZoneTimer > 0) {
            launchZoneTimer -= Time.deltaTime;
        }
    }

    private float getTargetDistance() {
        return Vector3.Distance(OperatorNode.Host.transform.position, rocket.Target.transform.position);
    }

    private bool isTargetInsideLaunchZone() {
        if (rocket != null && rocket.Target != null) {
            var targetDistance = getTargetDistance();
            var launchZone = rocket.GetParameterForShootTarget();
            return targetDistance >= launchZone[1] && targetDistance <= launchZone[0];
        }
        return false;
    }

    #endregion

    void resetArrows() {
        _crosshairController.LeftArrow.color = Color.grey;
        _crosshairController.RightArrow.color = Color.grey;
    }

    Boolean ButtonKeyStatePress() {
        if (keyStateB) {
            if (isTrackingPressed()) {
                return true;
            } else {
                StopNRZSound();
                return false;
            }
        }
        return false;
    }

    private bool isTrackingPressed() {
        return OperatorNode.Core.GetPanel("Strela10_GuidancePanel")
                    .GetControl(ControlType.Tumbler, "TUMBLER_TRACK_LAUNCH").State.Equals("TRACKING")
                || Input.GetKey(KeyCode.T);
    }

    public void stateTracker() {
		if (!(time > 5 && time < 19) && firstCooler == false && secondCooler == false)
		{
			Debug.Log("BREAK, BECAUSE NIGHT 1!!!");
			return;
		}

        if (nrz) {
            if (_crosshairController != null)
                _crosshairController.NRZ.color = Color.green;
        } else {
            if (_crosshairController != null)
                _crosshairController.NRZ.color = Color.grey;
        }
        WeaponryRocket rocket = OperatorNode.Host.Arms.Projectile as WeaponryRocket;
        if (rocket && rocket.Target) {
            if (ButtonKeyStatePress()) {
                if (!stateOne) {
                    Debug.Log("stateOne true");
                    Strela10_Operator_CoreHandler.CoreStatic.GetPanel("Strela10_VizorPanel").GetControl(
                        ControlType.Tumbler, "TUMBLER_LOSE").GetComponent<SwitcherToolkit>().ControlChanged();
                    stateOne = true;
                }
                if (CheckNRZTargetBelonging()) {
                    Debug.Log("CheckNRZTargetBelonging true");
					// СЛЕЖЕНИЕ РУЧНОЕ-АВТОМАТ
                    var coords = gameObjectVizir.WorldToScreenPoint(rocket.Target.transform.position);
                    coords = UICenter.UICamera.ScreenToWorldPoint(coords);
                    lastTrackerPos = new Vector3(coords.x, coords.y, 0);
                    Debug.LogError("lastTrackerPos: x = " + coords.x + "; y = " + coords.y);
					if (isAutoTrackingMode()){
						_crosshairController.Tracker.transform.position = lastTrackerPos; //Слежение следящей марки
					} else{
                        if ((coords.x * coords.x + coords.y * coords.y) > 0.12*0.12) return;	
					}
                    //switchIndicatorOn();
                    updateLaunchZoneIndicator();
                } else {
                    Debug.Log("CheckNRZTargetBelonging false");
                    Debug.LogError("Play NRZ");
                    PlayNRZSound();
                }
            } else {
                Debug.Log("ButtonKeyStatePress false");

                //switchIndicatorOff();
                updateLaunchZoneIndicator();
                if (keyStateB) {
                    Debug.Log("keyStateB true");
                    //switchIndicatorOff();
                    updateLaunchZoneIndicator();
                    stateOne = false;
                    stateZeroBort();
                }
            }
        }
    }
	
	bool isAutoTrackingMode(){
		if (Strela10_Operator_CoreHandler.CoreStatic.GetPanel("Strela10_SupportPanel")
            .GetControl(ControlType.Tumbler, "TUMBLER_TRACKING").State.Equals("AUTO")){
			return true;
		} else return false;
	}

    public void stateZeroBort() {
        var pos = new Vector3(b * (Mathf.Sin(Time.time) - 1) + b, a * (0 - Mathf.Sin(Time.time)) * Mathf.Cos(Time.time),
            0);
        var transformStartPosition = prevTrackerPosition == null
            ? _crosshairController.Tracker.transform.localPosition
            : (Vector3) (prevTrackerPosition);
        try {
            _crosshairController.Tracker.transform.localPosition =
                Vector3.Lerp(transformStartPosition, pos, speed * (Time.deltaTime));
            prevTrackerPosition = _crosshairController.Tracker.transform.localPosition;
        } catch {
        }
    }

    public bool CheckNRZTargetBelonging() {
        //Debug.LogError("Check NRZ");
        if (Strela10_Operator_CoreHandler.CoreStatic.GetPanel("Strela10_OperatorPanel").GetControl(ControlType.Tumbler, "TUMBLER_BL").State.Equals("ON")) {
            if (Strela10_Arms.VeiwIDTargetRussia != null) {
                if (rocket.Target.networkView.viewID.Equals(Strela10_Arms.VeiwIDTargetRussia)) {
                    //Debug.LogError("false;");
                    return false;
                }
            }
        }
        return true;
    }


    public void PlayBortSound() {
        //Debug.LogError("запуск борт");
        if (!audio1.isPlaying && !audio2.isPlaying) {
            audio1.Play();
            audio2.PlayDelayed(audio1.clip.length);
        }
    }

    public void StopBortSound() {
        audio1.Stop();
        audio2.Stop();
    }

    public void PlayNRZSound() {
        if (!audio3.isPlaying) {
            audio3.Play();
        }
    }

    public void StopNRZSound() {
        audio3.Stop();
    }
}