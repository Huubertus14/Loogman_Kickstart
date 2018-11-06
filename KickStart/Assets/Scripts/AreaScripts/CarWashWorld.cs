using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFox;

public class CarWashWorld : MonoBehaviour {

    public static CarWashWorld Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    [Header("Prefabs:")]
    public Material XRayMaterial;
    
    public List<xRayObjectScript> allXRayObjects = new List<xRayObjectScript>();
    private Color[] cols = new Color[] { Color.red, Color.blue, Color.green, Color.cyan };

    private void Start()
    {
        //CreateXRayWorld();
    }

    private void CreateXRayWorld()
    {
        Transform[] childObjects = GetComponentsInChildren<Transform>();
        foreach (var child in childObjects)
        {
            if (child.GetComponent<MeshRenderer>())
            {
                child.GetComponent<MeshRenderer>().material = XRayMaterial;
                child.gameObject.AddComponent<xRayObjectScript>();
                child.gameObject.tag = "xRayObject";
                allXRayObjects.Add(child.gameObject.GetComponent<xRayObjectScript>());
            }
        }
    }

    public void ShowImpulse(Vector3 _impulsePosition)
    {
        foreach (var xRay in allXRayObjects)
        {
            int x = Random.Range(0, cols.Length);
            xRay.HitBySonar(cols[x], _impulsePosition);
        }
    }
}
