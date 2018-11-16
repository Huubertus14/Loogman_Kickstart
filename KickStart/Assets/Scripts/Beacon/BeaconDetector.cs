using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Beacon;

#if !UNITY_EDITOR
using HoloBeaconScanner;
#endif

public class BeaconScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
#if !UNITY_EDITOR
        Scanner beaconScanner = new Scanner();
        Scanner.ScannedBeaconEventHandler += BeaconFound;
        beaconScanner.StartScanning();
#endif
    }

    public void BeaconFound(string str)
    {
        try
        {
            char[] delimiterChars = { '*' };
            string[] components = str.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
            if (components[1] != "0")
            {
                Beacon b = new Beacon();
                b.Uuid = components[0];
                b.Major = components[1];
                b.Minor = components[2];
                b.Power = components[3];
                b.DistanceValue = components[4];

                Debug.Log("[BEACON DETECTOR] : FOUND : " + b.Minor + " : Range: " + b.DistanceValue.ToString());
            }
        }
        catch (Exception ex)
        {
            Debug.Log("[BEACON DETECTOR EX : " + ex.Message.ToString());
        }
    }

    public void StartDetecting()
    {
#if !UNITY_EDITOR
        beaconScanner.StartScanning();
#endif
    }

    public void StopDetecting()
    {
#if !UNITY_EDITOR
        beaconScanner.StopScanning();
#endif
    }

    // Update is called once per frame
    void Update () {
		
	}
}

[Serializable]
public class Beacon
{
    public string Uuid { get; set; }

    public string Major { get; set; }

    public string Minor { get; set; }

    public string Power { get; set; }

    public string DistanceValue { get; set; }

    public enum Distance
    {
        Unknown, Immediate, Near, Far
    };
public Distance CurrentDistance;
}
