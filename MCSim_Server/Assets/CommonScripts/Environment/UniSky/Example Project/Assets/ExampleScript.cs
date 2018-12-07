using UnityEngine;
using System.Collections;

public class ExampleScript : MonoBehaviour {

	// instance of the API
	private UniSkyAPI uniSky;
	
	void Awake() {
		
		// Define instance
		uniSky = GameObject.Find("UniSkyAPI").GetComponent("UniSkyAPI") as UniSkyAPI;
		
		// Initiate and create default UniSky 
		uniSky.InstantiateUniSky();
		
		// Set some initial states 
		uniSky.SetTime(12.0f);
		uniSky.SetAmbientLighting(new Color(0.1f, 0.1f, 0.1f, 0.1f));
		uniSky.SetStormCenter(new Vector3(0,0,0));
		uniSky.SetSunShadows(LightShadows.Soft);
		
		// Functions to interpolate parameters over time
		/*
		uniSky.LerpCloudCover(0.5f, 5000.0f);
		uniSky.LerpPrecipitationLevel(0.6f, 5000.0f);
		uniSky.LerpStormCloudCover(-1.0f, 10000.0f);
		uniSky.LerpRainLevel(100, 0.2f, 10000.0f);
		uniSky.LerpStormLevel(150, 0.4f, 20000.0f);
		uniSky.LerpSunIntensity(0.2f, 10000.0f);
		uniSky.LerpFogLevel(0.02f, 20000.0f);
		uniSky.LerpAmbientLighting(new Color(0.0f, 0.0f, 0.0f, 0.0f), 5000);
		uniSky.ClearDropletBuffer();
		uniSky.LerpDropletLevel(10, 20000.0f);
		*/
	}

	void Update() {

	}
}
