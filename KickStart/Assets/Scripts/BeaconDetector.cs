using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Beacon;
using VrFox;



#if !UNITY_EDITOR
using HoloBeaconScanner;
#endif

public class BeaconDetector : MonoBehaviour
{

#if !UNITY_EDITOR
    Scanner beaconScanner;
#endif
    // Use this for initialization
    void Start()
    {
#if !UNITY_EDITOR
       Scanner beaconScanner = new Scanner();
        Scanner.ScannedBeaconEventHandler += BeaconFound;
        beaconScanner.StartScanning();
        GameManager.Instance.SendTextMessage("BeaCON initialiseert" , 1.5f, Vector2.zero);
#endif
    }


    public void BeaconFound(string str)
    {
        GameManager.Instance.SendTextMessage("Iets gevonden!", 1.5f, Vector2.zero);
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
                GameManager.Instance.SendTextMessage("[BEACON DETECTOR] : FOUND : " + b.Minor + " : Range: " + b.DistanceValue.ToString(), 15, Vector2.zero);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("[BEACON DETECTOR EX : " + ex.Message.ToString());
            GameManager.Instance.SendTextMessage("[BEACON DETECTOR EX : " + ex.Message.ToString(), 15, Vector2.zero);
        }
    }

    public void StartDetecting()
    {
#if !UNITY_EDITOR
        if(beaconScanner == null){
        GameManager.Instance.SendTextMessage("No beacon scanner" , 1.5f, Vector2.zero);
        }
        beaconScanner.StartScanning();
        GameManager.Instance.SendTextMessage("Start Scannen" , 1.5f, Vector2.zero);
#endif
    }

    public void StopDetecting()
    {
#if !UNITY_EDITOR
        beaconScanner.StopScanning();
        GameManager.Instance.SendTextMessage("Stop scanning" , 1.5f, Vector2.zero);
#endif
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
