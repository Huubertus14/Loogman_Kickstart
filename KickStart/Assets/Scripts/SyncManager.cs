using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SyncManager : MonoBehaviour {

    public static SyncManager Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    public void OnSignalInput()
    {
        
    }


}
