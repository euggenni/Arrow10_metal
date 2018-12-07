using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MilitaryCombatSimulator;

public class GUIPanel_Training : GUIPanel {

    private MCSTrainingChecker _checker;
    private MilitaryCombatSimulator.Weaponry _weaponry;
    private MCSTrainingOrder _order;

    public Transform _currentControl;

    private GameObject _mark;

    void Awake()
    {
        base.Awake();
       
        _mark = this["Marker"].gameObject;
    }

    public void OnCurrentControlChanged(PanelControl control)
    {

        if (control != null)
        {
            //Debug.LogError("CONTROL ZADAN " + control.transform.position+" "+control.name);
            _currentControl = control.transform; 
        }
        else // Когда все команды выполнены
        {                        
           // Debug.LogError("NET CONTROLA");
            _checker.OnTaskAttempt -= OnTaskAttempt;
            _checker.OnCurrentControlChanged -= OnCurrentControlChanged;

            // Отписываемся
            _currentControl = null;

            UICenter.MainCamera.enabled = true;
            UICenter.Panels.CloseGroup("Training");

            

            var pan = UICenter.Panels.Instantiate("ExamFinish");
            pan.SendMessage("Set", "Обучение пройдено!");
            pan.Show();

            //GetComponent<UIPanel>().enabled = false;
        }
    }

    void Update()
    {
        MarkCurrentSubtask();
    }
    
    public void OnTaskAttempt(bool success, OrderSubtask required,string discription)
    {

        if (success)
        {
            SetGlow(GetControl<UISlicedSprite>("Background"), Color.green);
            GetControl<UILabel>("Description").text = discription;
           
        }
        else {
            SetGlow(GetControl<UISlicedSprite>("Background"), Color.red);
            GetControl<UILabel>("Description").text = discription;             
        }
    }

    void SetGlow(UISlicedSprite sprite, Color clr)
    {
        var tc = sprite.ForceComponent<TweenColor>();
        tc.from = sprite.color;
        tc.to = clr;
        tc.duration = 0.5f;

        tc.onFinished = delegate
        {
            tc.enabled = false;
            Destroy(tc);

            var tc2 = sprite.gameObject.AddComponent<TweenColor>();
            tc2.from = tc2.color;
            tc2.to = Color.white;
            tc2.duration = 1.5f;
            tc2.onFinished = Destroy;
        };
    }

    void SetChecker(MCSTrainingChecker checker)
    {
        _checker = checker;
        checker.SubscribeOnTaskAttempt(OnTaskAttempt, OnCurrentControlChanged);

        OnCurrentControlChanged(_checker.GetCurrentControl());
        
        GetControl<UILabel>("Description").text = checker.CurrentCommand.Description;
      
    }

    private void MarkCurrentSubtask()
    {
        _mark.SetActive(_currentControl != null);

        if (_currentControl)
        {
            try
            {
                var coords = Camera.main.WorldToScreenPoint(_currentControl.position);
                coords = UICenter.UICamera.ScreenToWorldPoint(coords);
                _mark.transform.position = coords;
            }catch
            {
            }
        }
    }

    void GetWeaponry(MilitaryCombatSimulator.Weaponry weaponry)
    {
        _weaponry = weaponry;
    }


    void OnDestroy()
    {
        if (_checker)
        {
            _checker.OnTaskAttempt -= OnTaskAttempt;
            _checker.OnCurrentControlChanged -= OnCurrentControlChanged;
        }
    }

    public void OnSelectionChanged(string str)
    {
        Debug.Log(str);
    }
}
