using UnityEngine;
using System.Collections;

public class HelicopterToolkit : MonoBehaviour {
    public string str="пока не работает, реализовано в AI_Helipopter";
    public float HellSpeed=AIHelicopter.speed;
    public float MaxSpeed = 0.4f;
    public float HellRotSpeed=AIHelicopter.turnSpeed;
    public float HellRotorSpeed = AIHelicopter.speedrotor;
  
	public Quaternion HellRotationVector=AIHelicopter.rot;
	 //Use this for initialization
	void Start () {
        HellSpeed = 0.2f;
	}
	
	// Update is called once per frame
	void Update () {
        AIHelicopter.speed=HellSpeed;
       AIHelicopter.turnSpeed=HellRotSpeed;
       AIHelicopter.speedrotor = HellRotorSpeed;
       AIHelicopter.rot = HellRotationVector;
       if ((HellSpeed < MaxSpeed)&&(AIHelicopter.distance>30f)) HellSpeed += 0.001f;
       else if (AIHelicopter.distance < 30f) HellSpeed -= 0.01f;
        
       
	}
}
