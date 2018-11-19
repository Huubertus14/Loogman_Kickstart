using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFox;

public class CarWashWorld : MonoBehaviour
{

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

    [Header("CheckPoints:")]
    public GameObject[] Checkpoint;

    private readonly List<xRayObjectScript> allXRayObjects = new List<xRayObjectScript>();
    private Color[] cols = new Color[] { Color.red, Color.blue, Color.green, Color.cyan };

    private Vector3 goalPos;
    private Quaternion goalRot;
    public float MovementSpeed;

    private void Start()
    {
        CreateXRayWorld();
        goalRot = transform.rotation;
        goalPos = transform.position;
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
               // allXRayObjects.Add(child.gameObject.GetComponent<xRayObjectScript>());
            }
        }
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, goalPos, Time.deltaTime * MovementSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, goalRot, Time.deltaTime * MovementSpeed);
    }

    public void ShowImpulse(Vector3 _impulsePosition)
    {
        foreach (var xRay in allXRayObjects)
        {
            int x = Random.Range(0, cols.Length);
            xRay.HitBySonar(cols[x], _impulsePosition);
        }
    }

    public void SetGoalPosition(Vector3 _pos)
    {
        goalPos = _pos;
    }

    public void SetGoalRotation(Quaternion _rot)
    {
        goalRot = _rot;
    }
}
