using UnityEngine;
using VrFox;


public class SyncManager : MonoBehaviour
{
    public static SyncManager Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }


    private void Start()
    {
        
    }

}

